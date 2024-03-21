using System;
using System.Data;
using System.Threading.Tasks;
using Npgsql;

namespace CourseProgram.Services
{
    public class DBConnection
    {
        public string Server = "localhost:5433"; //сервер БД
        public string Database = "CourseDB"; //путь к БД
        public string? Login; //логин
        public string? Password; //пароль

        volatile private int _ConnectionState; //состояние соединения: 0 нет соединения, 1 - установлено, -1 - разорвано
        public int ConnectionState
        {
            get { return _ConnectionState; }
        }

        private int _reconnectTime = 7;
        public int ReconnectTime // время повторного соединения в секундах
        {
            set { _reconnectTime = value > 0 ? value : 5; }
            get { return _reconnectTime; }
        }

        public int TimeOut; // время таймаута в милисекундах
        public DateTime LastConnectionTry; // Время последней попытки соединения

        private NpgsqlConnection cnn; //соединение с БД
        public bool WindowsAuth = false;
        public NpgsqlConnectionStringBuilder connstr;

        private bool _ServerLinkError = false;
        public bool ServerLinkError
        {
            get { return _ServerLinkError; }
        }

        private System.Object lockThis = new System.Object();

        private bool WatchLoop;// цикл присмотра

        public delegate bool UpdateTarget(object sender, EventArgs e);
        public event UpdateTarget m_UpdateTarget;  // Метод вызываемый при подтверждении наличия связи
        public event UpdateTarget m_ReconnectTarget; // Метод вызываемый после подключения к БД или реконекта

        public bool isTransaction = false;

        //public LogBank Log = null;

        public bool NeedTimeStampPing = true;
        private DateTime LastConnectionCheck = DateTime.MinValue;

        public string ThreadName;

        System.Timers.Timer WorkTimer;
        TimeSpan Interval = TimeSpan.FromMilliseconds(30);
        private bool IsDisposed = false;

        public DBConnection(int ReCheckTime, UpdateTarget Synchron, UpdateTarget Reconnect, string ConnectName) //LogBank l_Log
        {
            ThreadName = ConnectName;
            //Log = l_Log;
            ReconnectTime = ReCheckTime;
            m_UpdateTarget = Synchron;
            m_ReconnectTarget = Reconnect;
            WatchLoop = true;
            ResyncNow();
            InitTimer();
        }

        private void InitTimer()
        {
            IsDisposed = false;
            WorkTimer = new System.Timers.Timer(Interval.TotalMilliseconds);
            WorkTimer.Disposed += OnDispose;
            WorkTimer.Elapsed += WorkTimer_Elapsed;
            WorkTimer.AutoReset = false;
            WorkTimer.Start();
        }

        private void OnDispose(object sender, EventArgs e)
        {
            IsDisposed = true;
        }

        private void WorkTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (IsDisposed) return;
            W_Work();
            if (WatchLoop) WorkTimer.Start();
        }

        public NpgsqlConnection? getConnection => ConnectionState == 1 ? cnn : null;

        public DBConnection(DBConnection cn, out NpgsqlTransaction? trans, string ConnectName)
        {
            ThreadName = ConnectName;
            WindowsAuth = cn.WindowsAuth;
            Server = cn.Server;
            Database = cn.Database;
            Login = cn.Login;
            Password = cn.Password;
            //Log = cn.Log;
            isTransaction = true;
            NeedTimeStampPing = false;
            _ConnectionState = ConnectDatabase();
            if ((cnn != null) && (ConnectionState == 1)) trans = cnn.BeginTransaction();
            else trans = null;
        }

        public DBConnection(DBConnection cn, string ConnectName)
        {
            ThreadName = ConnectName;
            WindowsAuth = cn.WindowsAuth;
            Server = cn.Server;
            Database = cn.Database;
            Login = cn.Login;
            Password = cn.Password;
            //Log = cn.Log;
            ReconnectTime = cn.ReconnectTime;
            NeedTimeStampPing = false;
            m_UpdateTarget = null;
            m_ReconnectTarget = null;
            WatchLoop = true;
            ResyncNow();

            InitTimer();
        }

        // Простое соединение, только для запросов. Следить за разрывами и востанавливать соединение нужно вручную
        public DBConnection(string server, string filePath, bool windowsAuth, string login, string pass, string connectName)
        {
            ThreadName = connectName;
            Server = server;
            Database = filePath;
            Login = login;
            Password = pass;
            WindowsAuth = windowsAuth;
            WatchLoop = false;
        }

        //Саш, удобно. Не создаёт таймеров, открытие/закрытие контролируем сами и транзакции нет. Тестировал - всё норм работает.Не хватает IDisposable для using{} 
        public DBConnection NewConnection(string name)
        {
            return new DBConnection(Server, Database, WindowsAuth, Login, Password, name);
        }

        // Использовать только если используется простое соединение
        public void Open()
        {
            Close();

            connstr = CreateConnStr(WindowsAuth);
            cnn = new NpgsqlConnection(connstr.ConnectionString);
            cnn.Open();

            if (cnn.State.HasFlag(System.Data.ConnectionState.Open))
                _ConnectionState = 1;
        }

        public async Task OpenAsync()
        {
            Close();

            connstr = CreateConnStr(WindowsAuth);
            cnn = new NpgsqlConnection(connstr.ConnectionString);
            await cnn.OpenAsync();

            if (cnn.State.HasFlag(System.Data.ConnectionState.Open))
                _ConnectionState = 1;
        }
        public void ResyncNow()
        {
            NeedForceLoadTry = true;
            LastConnectionTry = DateTime.Now.AddSeconds(-ReconnectTime * 2);
        }

        public void Close()
        {
            WatchLoop = false;
            if (WorkTimer != null)
                WorkTimer.Stop();

            _ConnectionState = 0;

            if (cnn != null)
            {
                cnn.Close();
                cnn.Dispose();
            }
        }

        public void CancelConnect()
        {
            _ConnectionState = 0;
        }

        public void AutoConnect() //-V3013
        {
            _ConnectionState = -1;
        }

        public void Reset()
        {
            _ConnectionState = -1;
            ResyncNow();
        }

        private void SaveLog(string text)
        {
            //if (Log != null) Log.Log(text);
        }

        public volatile bool NeedForceLoadTry = false;

        private void W_Work()
        {
            try
            {
                if (DateTime.Now.AddSeconds(-ReconnectTime) > LastConnectionTry || NeedForceLoadTry)
                {
                    if (ConnectionState != 0)
                    {
                        lock (lockThis)
                        {
                            NeedForceLoadTry = false;

                            if (ConnectionState != 1)
                            {
                                var Cs = ConnectDatabase();
                                switch (Cs)
                                {
                                    case 1:
                                        _ConnectionState = ExecSQL_Force("Select version();");
                                        //ExecSQL_Force("SET ARITHABORT ON");
                                        m_ReconnectTarget?.Invoke(this, null);
                                        SaveLog("Восстановили соединение с БД (" + ThreadName + ")");
                                        break;
                                    case 0:
                                        SaveLog("Соединение с БД не установлено: логин или пароль неверны (" + ThreadName + ")");
                                        _ConnectionState = Cs;
                                        break;
                                    case -1:
                                        SaveLog("Соединение с БД не установлено: сервер или имя базы неверны (" + ThreadName + ")");
                                        _ConnectionState = Cs;
                                        break;
                                }
                            }

                            if (ConnectionState == 1)
                            {
                                if (NeedTimeStampPing)
                                    Query_Scalar("SELECT CURRENT_TIMESTAMP;");
                                m_UpdateTarget?.Invoke(this, null);
                            }
                        }
                        LastConnectionTry = DateTime.Now;
                    }
                }
            }
            catch (Exception ex)
            {
                SaveLog("Ошибка в обработке соединения: " + ex.Message);
                // неизвесные ошибки
            }
        }

        private string ParamToString(params object[] items)
        {
            string result = "";
            for (int i = 0; i < items.Length; i++)
            {
                if (i > 0) result += ", ";
                result += items[i].ToString();
            }
            return result;
        }

        /**
          * @brief Метод получает таблицу из БД
          * @param query SQL запрос
          * @retval DataTable таблица с данными
          */
        public DBAnswer GetDataTable(string query)
        {
            DBAnswer result = new DBAnswer();
            try
            {
                lock (lockThis)
                {
                    if (ConnectionState == 1)
                    {
                        result.GetData(query, cnn);
                        result.SetAnswer();
                    }
                }
            }
            catch (PostgresException ex)
            {
                _ConnectionState = -1;
                SaveLog("SQL exception occurred: [" + query + "]" + ex.Message);
            }
            return result;
        }

        // Тот же запрос что и выше только с возвратом эксепшена, если произошла ошибка
        public DBAnswer GetDataTableEx(string query)
        {
            DBAnswer result = new DBAnswer();
            try
            {
                lock (lockThis)
                {
                    if (ConnectionState == 1)
                    {
                        result.GetData(query, cnn);
                        result.SetAnswer();
                        return result;
                    }
                }
            }
            catch (PostgresException ex)
            {
                _ConnectionState = -1;
                SaveLog("SQL exception occurred: [" + query + "]" + ex.Message);
            }
            throw (new Exception("Нет связи с " + ThreadName));
        }

        public DBAnswer GetDataTableParam(string query, params object[] param)
        {
            DBAnswer result = new DBAnswer();
            try
            {
                lock (lockThis)
                {
                    if (ConnectionState == 1)
                    {
                        result.GetDataParam(query, cnn, param);
                        result.SetAnswer();
                    }
                }
            }
            catch (PostgresException ex)
            {
                _ConnectionState = -1;
                SaveLog("SQL exception occurred:: [" + query + "][" + ParamToString(param) + "]" + ex.Message);
            }
            return result;
        }

        public DBAnswer GetDataTableParamTimeOut(string query, int commandTimeout_, params object[] param)
        {
            DBAnswer result = new DBAnswer();
            try
            {
                lock (lockThis)
                {
                    if (ConnectionState == 1)
                    {
                        result.GetDataParamTimeout(query, cnn, commandTimeout_, param);
                        result.SetAnswer();
                    }
                }
            }
            catch (PostgresException ex)
            {
                _ConnectionState = -1;
                SaveLog("SQL exception occurred:: [" + query + "][" + ParamToString(param) + "]" + ex.Message);
            }
            return result;
        }

        public DBAnswer GetDataTableParam(NpgsqlTransaction transaction, string query, params object[] param)
        {
            DBAnswer result = new DBAnswer();
            try
            {
                lock (lockThis)
                {
                    if (ConnectionState == 1)
                    {
                        result.GetDataParam(transaction, query, cnn, param);
                        result.SetAnswer();
                    }
                }
            }
            catch (PostgresException ex)
            {
                _ConnectionState = -1;
                SaveLog("SQL exception occurred: [" + query + "][" + ParamToString(param) + "]" + ex.Message);
            }
            return result;
        }

        public DBAnswer ExecParam(NpgsqlTransaction transaction, string query, params object[] param)
        {
            DBAnswer result = new DBAnswer();
            try
            {
                lock (lockThis)
                {
                    if (ConnectionState == 1)
                    {
                        result.ExecParam(transaction, query, cnn, param);
                        result.SetAnswer();
                    }
                }
            }
            catch (PostgresException ex)
            {
                _ConnectionState = -1;
                SaveLog("SQL exception occurred: [" + query + "][" + ParamToString(param) + "]" + ex.Message);
            }
            return result;
        }

        /** 
          * @brief Метод выполняет транзакцию
          * @param query SQL запрос
          * @retval int @arg 0 - нет связи с БД, @arg 1 - SQL транзакция выполнена
          */
        public int ExecSQL(string query)
        {
            lock (lockThis)
            {
                if (ConnectionState == 1)
                {
                    NpgsqlTransaction? trans = null;

                    try
                    {
                        trans = cnn.BeginTransaction();
                        NpgsqlCommand command = new NpgsqlCommand(query, cnn);
                        command.Transaction = trans;
                        command.ExecuteNonQuery();
                        trans.Commit();
                        return 1;
                    }

                    catch (PostgresException ex)
                    {
                        _ConnectionState = -1;
                        SaveLog("SQL exception occurred: [" + query + "]" + ex.Message);
                        //откатываем 
                        if (trans != null) trans.Rollback();
                        return 0;
                    }
                }
                else return 0;
            }
        }


        public DBAnswer ExecParam(string query, params object[] param)
        {
            DBAnswer result = new DBAnswer();
            try
            {
                lock (lockThis)
                {
                    if (ConnectionState == 1)
                    {
                        result.ExecParam(query, cnn, null, param);
                        result.SetAnswer();
                    }
                }
            }
            catch (PostgresException ex)
            {
                _ConnectionState = -1;
                SaveLog("SQL exception occurred: [" + query + "][" + ParamToString(param) + "]" + ex.Message);
            }
            return result;
        }


        public async Task<DBAnswer> ExecParamAsync(string query, params object[] param)
        {
            DBAnswer result = new DBAnswer();
            try
            {
                if (ConnectionState == 1)
                {
                    await result.ExecParamAsync(query, cnn, null, param);
                    result.SetAnswer();
                }

            }
            catch (PostgresException ex)
            {
                _ConnectionState = -1;
                SaveLog("SQL exception occurred: [" + query + "][" + ParamToString(param) + "]" + ex.Message);
            }
            return result;
        }

        public DBAnswer ExecParamTimeOut(string query, int? commandTimeout_, params object[] param)
        {
            DBAnswer result = new DBAnswer();
            try
            {
                lock (lockThis)
                {
                    if (ConnectionState == 1)
                    {
                        result.ExecParam(query, cnn, commandTimeout_, param);
                        result.SetAnswer();
                    }
                }
            }
            catch (PostgresException ex)
            {
                _ConnectionState = -1;
                SaveLog("SQL exception occurred: [" + query + "][" + ParamToString(param) + "]" + ex.Message);
            }
            return result;
        }

        public int ExecSQL_Force(string query)
        {
            lock (lockThis)
            {
                try
                {
                    NpgsqlCommand command = new NpgsqlCommand(query, cnn);
                    command.ExecuteNonQuery();
                    return 1;
                }
                catch (PostgresException ex)
                {
                    SaveLog("SQL exception occurred: [" + query + "]" + ex.Message);
                    return -1;
                }
            }
        }


        public int ExecSQLEx(string query)
        {
            lock (lockThis)
            {
                if (ConnectionState == 1)
                {
                    NpgsqlTransaction trans = cnn.BeginTransaction();

                    try
                    {
                        NpgsqlCommand command = new NpgsqlCommand(query, cnn);
                        command.Transaction = trans;
                        command.ExecuteNonQuery();
                        trans.Commit();
                        return 1;
                    }

                    catch (PostgresException ex)
                    {
                        _ConnectionState = -1;
                        //откатываем 
                        if (trans != null) trans.Rollback();
                        throw (new Exception(ex.Message));
                    }
                }
                else
                    throw (new Exception("Нет соединения!"));
            }
        }
        /** 
          * @brief Метод выполняет транзакцию
          * @param trans транзакция
          * @param query SQL запрос
          * @retval int @arg 0 - нет связи с БД, @arg 1 - SQL транзакция выполнена
          */
        public int ExecSQL(NpgsqlTransaction trans, string query)
        {
            lock (lockThis)
            {
                if (ConnectionState == 1)
                {
                    try
                    {
                        NpgsqlCommand command = new NpgsqlCommand(query, cnn);
                        command.Transaction = trans;
                        command.ExecuteNonQuery();
                        return 1;
                    }
                    catch (PostgresException ex)
                    {
                        _ConnectionState = -1; // Разрыв соединения
                        SaveLog("SQL exception occurred: [" + query + "]" + ex.Message);
                        //откатываем 
                        if (trans != null) trans.Rollback();
                        return 0;
                    }
                }
                else return 0;
            }
        }

        public int ExecSQLEx(NpgsqlTransaction trans, string query)
        {
            lock (lockThis)
            {
                if (ConnectionState == 1)
                {
                    try
                    {
                        NpgsqlCommand command = new NpgsqlCommand(query, cnn);
                        command.Transaction = trans;
                        command.ExecuteNonQuery();
                        return 1;
                    }
                    catch (PostgresException ex)
                    {
                        _ConnectionState = -1; // Разрыв соединения
                        //откатываем 
                        if (trans != null) trans.Rollback();
                        throw (new Exception(ex.Message));
                    }
                }
                else throw (new Exception("Нет соединения"));
            }
        }
        /** 
          * @brief Метод получает 1 значение (объект) из БД
          * @param query SQL запрос
          * @retval object объект-значение из БД
          */
        public object? Query_Scalar(string query)
        {
            lock (lockThis)
            {
                if (ConnectionState == 1)
                {
                    try
                    {
                        NpgsqlCommand command = new NpgsqlCommand(query, cnn);
                        //  command.CommandTimeout = 120;
                        return command.ExecuteScalar();
                    }
                    catch (PostgresException ex)
                    {
                        _ConnectionState = -1; // Разрыв соединения
                        SaveLog("SQL exception occurred: [" + query + "]" + ex.Message);
                        return null;
                    }
                }
                else return null;
            }
        }

        public object? Query_Scalar_Force(string query)
        {
            lock (lockThis)
            {
                try
                {
                    NpgsqlCommand command = new NpgsqlCommand(query, cnn);
                    return command.ExecuteScalar();
                }
                catch (PostgresException ex)
                {
                    _ConnectionState = -1; // Разрыв соединения
                    SaveLog("SQL exception occurred: [" + query + "]" + ex.Message);
                    return null;
                }
            }
        }

        /** 
          * @brief Метод получает 1 значение (объект) из БД
          * @param trans транзакция
          * @param query SQL запрос
          * @retval object объект-значение из БД
          */
        public object? Query_Scalar(NpgsqlTransaction trans, string query)
        {
            lock (lockThis)
            {
                if (ConnectionState == 1)
                {
                    try
                    {
                        NpgsqlCommand command = new NpgsqlCommand(query, cnn);
                        command.Transaction = trans;
                        return command.ExecuteScalar();
                    }
                    catch (PostgresException ex)
                    {
                        _ConnectionState = -1; // Разрыв соединения
                        SaveLog("SQL exception occurred: [" + query + "]" + ex.Message);
                        return null;
                    }
                }
                else return null;
            }
        }

        public object? Query_ScalarEx(NpgsqlTransaction trans, string query)
        {
            lock (lockThis)
            {
                if (ConnectionState == 1)
                {
                    try
                    {
                        NpgsqlCommand command = new NpgsqlCommand(query, cnn);
                        command.Transaction = trans;
                        return command.ExecuteScalar();
                    }
                    catch (PostgresException ex)
                    {
                        _ConnectionState = -1; // Разрыв соединения
                        throw (new Exception("SQL exception occurred: [" + query + "]" + ex.Message));
                    }
                }
                else
                    throw (new Exception("Нет соединения!"));
            }
        }

        private NpgsqlConnectionStringBuilder CreateConnStr(bool WA)
        {
            var con_str = new NpgsqlConnectionStringBuilder();
            con_str.Host = Server;
            con_str.Database = Database;
            con_str.Multiplexing = false; // отключаем MARS для паралельных транзакций
            con_str.Enlist = true;
            con_str.Pooling = true;
            // con_str.ConnectTimeout = 30;
            con_str.Username = Login;
            con_str.Password = Password;

            return con_str;
        }

        /** 
          * @brief Установка соединения с БД
          * @param нет
          * @retval int @arg -1 - таймаут соединения, @arg 0 - не верные логин/пароль, @arg 1 - соединение прошло успешно
          */
        public int ConnectDatabase()
        {
            lock (lockThis)
            {
                try
                {
                    if (cnn != null) cnn.Close();

                    //если соединение не открыто или не создано
                    if ((cnn == null) || (cnn.State != System.Data.ConnectionState.Open))
                    {
                        connstr = CreateConnStr(WindowsAuth);

                        cnn = new NpgsqlConnection(connstr.ConnectionString);
                        cnn.Open();
                        //ConnectionState = 1; // Установили соединение

                        // if (m_UpdateTarget != null) m_UpdateTarget();
                    }
                    _ServerLinkError = false;
                    return 1;
                }

                catch (PostgresException ex)
                {
                    if (ex.SqlState == "08000")
                    {
                        // Нет связи с сервером, продолжаем попытки соединится
                        _ServerLinkError = true;
                        return -1;
                    }

                    //error 28P01 - логин или пасс не верен
                    if (ex.SqlState == "28P01")
                        return 0;
                    else
                    {
                        _ServerLinkError = false;
                        TimeOut = cnn.ConnectionTimeout;
                        return -1;
                    }
                }
            }
        }

        public static byte GetByteOrNull(object value, byte nullvalue)
        {
            if (Convert.DBNull != value && value != null)
                return Convert.ToByte(value);
            else return nullvalue;
        }

        public static int GetIntOrNull(object value, int nullvalue)
        {
            if (Convert.DBNull != value && value != null)
                return Convert.ToInt32(value);
            else return nullvalue;
        }

        public static Int64 GetInt64OrNull(object value, Int64 nullvalue)
        {
            if (Convert.DBNull != value && value != null)
                return Convert.ToInt64(value);
            else return nullvalue;
        }

        public static string? GetStringOrNull(object value, string nullvalue)
        {
            if (Convert.DBNull != value && value != null)
                return Convert.ToString(value);
            else return nullvalue;
        }

        //public static byte[] ImageToByteArray(System.Drawing.Image image)
        //{
        //    MemoryStream ms = new MemoryStream();
        //    image.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
        //    return ms.ToArray();
        //}

        //public static System.Drawing.Image GetImageOrNull(object value, System.Drawing.Image nullvalue)
        //{
        //    if (Convert.DBNull != value && value != null)
        //    {
        //        try
        //        {
        //            MemoryStream ms = new MemoryStream(value as byte[]);
        //            return System.Drawing.Image.FromStream(ms);
        //        }
        //        catch
        //        {
        //        }
        //    }
        //    return nullvalue;
        //}

        public static DateTimeOffset GetDateTimeOffset(object value) => DateTimeOffset.Parse(input: Convert.ToString(value));

        public static int TakeID(DBAnswer ans)
        {
            if (ans.HasAnswer && ans.Rows > 0)
            {
                DataRow scalar = ans;
                return GetIntOrNull(scalar["id"], -1);
            }
            return -2;
        }

        public static DateTimeOffset GetDateTimeOffsetOrNull(object value, DateTimeOffset nullvalue)
        {
            if (Convert.DBNull != value && value != null)
                return DateTimeOffset.Parse(input: Convert.ToString(value));
            else return nullvalue;
        }

        public static double GetDoubleOrNull(object value, double nullvalue)
        {
            if (Convert.DBNull != value && value != null)
                return Convert.ToDouble(value);
            else return nullvalue;
        }

        public static float GetFloatOrNull(object value, float nullvalue)
        {
            if (Convert.DBNull != value && value != null)
                return (float)Convert.ToDouble(value);
            else return nullvalue;
        }

        public static DateTime GetDateTimeOrNull(object value, DateTime nullvalue)
        {
            if (Convert.DBNull != value && value != null)
                return Convert.ToDateTime(value);
            else return nullvalue;
        }

        public static DateOnly GetDateOnlyOrNull(object value, DateOnly nullvalue)
        {
            if (Convert.DBNull != value && value != null)
                return DateOnly.FromDateTime(Convert.ToDateTime(value));
            else return nullvalue;
        }

        /** 
          * @brief Установка соединения с БД
          * @param ConnectionString строка подключения
          * @retval int @arg -1 - таймаут соединения, @arg 0 - не верные логин/пароль, @arg 1 - соединение прошло успешно
          */
        public int ConnectDatabase(string ConnectionString)
        {
            lock (lockThis)
            {
                cnn = new NpgsqlConnection(ConnectionString);

                try
                {
                    if (cnn.State != System.Data.ConnectionState.Open)
                    {
                        cnn = new NpgsqlConnection(ConnectionString);
                        cnn.Open();
                        _ConnectionState = 1;
                    }
                    return 1;
                }

                catch (PostgresException ex)
                {
                    _ConnectionState = 0;
                    //error 28P01 - логин или пасс не верен
                    if (ex.SqlState == "28P01")
                        return 0;
                    else
                    {
                        TimeOut = cnn.ConnectionTimeout;
                        return -1;
                    }
                }
            }
        }
    }
}