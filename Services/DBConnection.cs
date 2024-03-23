using System;
using System.Data;
using System.Diagnostics;
using System.Threading.Tasks;
using Npgsql;

namespace CourseProgram.Services
{
    public class DBConnection
    {
        public string Server; //сервер БД
        public string Database; //путь к БД
        public string Login; //логин
        public string Password; //пароль

        volatile private int _ConnectionState; //состояние соединения: 0 нет соединения, 1 - установлено, -1 - разорвано
        public int ConnectionState => _ConnectionState;

        private int _reconnectTime = 7;
        public int ReconnectTime // время повторного соединения в секундах
        {
            get => _reconnectTime;
            set => _reconnectTime = value > 0 ? value : 5;
        }

        private NpgsqlConnection cnn; //соединение с БД
        public NpgsqlConnectionStringBuilder connstr;
        private readonly string ThreadName;
        private readonly bool _serverLinkError = false;
        public bool ServerLinkError => _serverLinkError;

        private readonly object lockThis = new();

        public NpgsqlConnection? GetConnection => ConnectionState == 1 ? cnn : null;

        public DBConnection(string server, string filePath, string login, string pass, string thread = "Base")
        {
            Server = server;
            Database = filePath;
            Login = login;
            Password = pass;
            ThreadName = thread;
        }

        public DBConnection NewConnection(string name) => new(Server, Database, Login, Password, name);

        public void Open()
        {
            Close();

            connstr = CreateConnStr();
            cnn = new NpgsqlConnection(connstr.ConnectionString);
            cnn.Open();

            if (cnn.State.HasFlag(System.Data.ConnectionState.Open))
                _ConnectionState = 1;
        }

        public async Task OpenAsync()
        {
            Close();
            try
            {
                connstr = CreateConnStr();
                cnn = new NpgsqlConnection(connstr.ConnectionString);
                await cnn.OpenAsync();
            }
            catch(NpgsqlException ex)
            {
                Debug.WriteLine(ex.Message);
                Close();
            }


            if (cnn.State.HasFlag(System.Data.ConnectionState.Open))
                _ConnectionState = 1;
        }

        public void Close()
        {
            _ConnectionState = 0;

            if (cnn != null)
            {
                cnn.Close();
                cnn.Dispose();
            }
        }

        #region Queries
        public DBAnswer GetDataTable(string query) //для получения целых таблиц или табл. функций
        {
            DBAnswer result = new();
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
            catch (PostgresException)
            {
                _ConnectionState = -1;
            }
            throw (new Exception("Нет связи с " + ThreadName));
        }

        public DBAnswer GetDataTableParam(string query, params object[] param) //для получения табличных данных с параметром
        {
            DBAnswer result = new();
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
            catch (PostgresException)
            {
                _ConnectionState = -1;
            }
            return result;
        }

        public DBAnswer ExecParam(NpgsqlTransaction transaction, string query, params object[] param) //для разных запросов с параметром
        {
            DBAnswer result = new();
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
            catch (PostgresException)
            {
                _ConnectionState = -1;
            }
            return result;
        }

        public async Task<DBAnswer> ExecParamAsync(string query, params object[] param) //для разных запросов с параметром (асинхронно)
        {
            DBAnswer result = new();
            try
            {
                if (ConnectionState == 1)
                {
                    await result.ExecParamAsync(query, cnn, null, param);
                    result.SetAnswer();
                }

            }
            catch (PostgresException)
            {
                _ConnectionState = -1;
            }
            return result;
        }

        public int ExecSQL_Force(string query) //-
        {
            lock (lockThis)
            {
                try
                {
                    NpgsqlCommand command = new(query, cnn);
                    command.ExecuteNonQuery();
                    return 1;
                }
                catch (PostgresException)
                {
                    return -1;
                }
            }
        }

        public object? Query_Scalar(string query) //для скалярок
        {
            lock (lockThis)
            {
                if (ConnectionState == 1)
                {
                    try
                    {
                        NpgsqlCommand command = new(query, cnn);
                        return command.ExecuteScalar();
                    }
                    catch (PostgresException)
                    {
                        _ConnectionState = -1;
                        return null;
                    }
                }
                else return null;
            }
        }

        public object? Query_ScalarEx(NpgsqlTransaction trans, string query) //для скалярок
        {
            lock (lockThis)
            {
                if (ConnectionState == 1)
                {
                    try
                    {
                        NpgsqlCommand command = new(query, cnn)
                        {
                            Transaction = trans
                        };
                        return command.ExecuteScalar();
                    }
                    catch (PostgresException ex)
                    {
                        _ConnectionState = -1;
                        throw (new Exception("SQL exception occurred: [" + query + "]" + ex.Message));
                    }
                }
                else
                    throw (new Exception("Нет соединения!"));
            }
        }
#endregion

        private NpgsqlConnectionStringBuilder CreateConnStr()
        {
            var con_str = new NpgsqlConnectionStringBuilder
            {
                Host = Server,
                Database = Database,
                Multiplexing = false,
                Enlist = true,
                Pooling = true,
                Username = Login,
                Password = Password
            };

            return con_str;
        }

        #region Object to
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
        #endregion
    }
}