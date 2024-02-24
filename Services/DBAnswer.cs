using System.Data;
using System.Collections;
using Npgsql;
using NpgsqlTypes;
using System.Threading.Tasks;
using System;

namespace CourseProgram.Services
{
    public class DBAnswer : IEnumerable, IDisposable
    {
        public DateTime start = DateTime.MinValue;
        public DateTime end = DateTime.MinValue;
        private bool _hasanswer = false;

        public bool HasAnswer
        {
            get { return _hasanswer; }
        }

        public TimeSpan ResponseTime
        {
            get { return end - start; }
        }

        private DataTable? data;

        public bool HasData { get { return data != null; } }
        public int Columns => HasData ? data.Columns.Count : 0;
        public int Rows => HasData ? data.Rows.Count : 0;

        public DBAnswer()
        {
            data = null;
        }

        public void Dispose()
        {
            if (data != null)
                data.Dispose();
        }

        public void SetAnswer()
        {
            _hasanswer = true;
        }

        public DataTable DataTable => data;

        public IEnumerator GetEnumerator()
        {
            if (HasData)
            {
                int length = data.Rows.Count;

                for (int index = 0; index < length; index++)
                    yield return data.Rows[index];
            }
        }

        public void GetData(string query, NpgsqlConnection connection)
        {
            start = DateTime.Now;
            using (NpgsqlCommand cmd = new NpgsqlCommand(query, connection))
            {
                using (NpgsqlDataReader reader = cmd.ExecuteReader())
                {
                    data = new DataTable();
                    data.Locale = System.Globalization.CultureInfo.InvariantCulture;
                    data.Load(reader);
                }
            }
            end = DateTime.Now;
        }

        /// <summary>
        /// Формирует запрос с параметрами. Принимает null, bitmap, byte[]. Если значения DateTime = DateTime.MaxValue , то превращается в null
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="param"></param>
        private void FillParams(NpgsqlCommand cmd, params object[] param)
        {
            if (param == null) param = new object[] { DBNull.Value };

            for (int i = 0; i < param.Length; i++)
            {
                if (param[i] == null) param[i] = DBNull.Value;
                //if (param[i] is System.Drawing.Bitmap) param[i] = DBConnection.ImageToByteArray((System.Drawing.Bitmap)param[i]);
                if (param[i] is System.DateTime && ((DateTime)param[i]) == DateTime.MaxValue) param[i] = DBNull.Value;
                if (param[i] is System.DateTimeOffset && ((DateTimeOffset)param[i]) == DateTimeOffset.MaxValue) param[i] = DBNull.Value;
                if (param[i] is System.Int32 && ((System.Int32)param[i] == Int32.MinValue)) param[i] = DBNull.Value;

                string paramname = "@" + (i + 1).ToString();
                NpgsqlDbType paramType;

                switch (param[i].GetType().ToString())
                {
                    case "System.DateTimeOffset": paramType = NpgsqlDbType.TimestampTz; break;
                    case "System.DateTime": paramType = NpgsqlDbType.Timestamp; break;
                    case "System.Int64": paramType = NpgsqlDbType.Bigint; break;
                    case "System.Int32": paramType = NpgsqlDbType.Integer; break;
                    case "System.Int16": paramType = NpgsqlDbType.Smallint; break;
                    case "System.String":
                        {
                            if (((string)param[i]).Length < 4000)
                                paramType = NpgsqlDbType.Varchar;
                            else
                                paramType = NpgsqlDbType.Text;
                        }
                        break;
                    case "System.DBNull": paramType = NpgsqlDbType.Unknown; break;
                    case "System.Single": paramType = NpgsqlDbType.Real; break;
                    case "System.Double": paramType = NpgsqlDbType.Double; break;
                    case "System.Byte[]": paramType = NpgsqlDbType.Bytea; break;
                    case "System.Decimal": paramType = NpgsqlDbType.Numeric; break;
                    default:
                        {
                            throw (new Exception("Не найден NpgsqlDbType для объекта " + param[i].GetType().ToString()));
                        }
                }

                if (param[i] == DBNull.Value)
                {
                    cmd.Parameters.Add(paramname, NpgsqlDbType.Integer).Value = DBNull.Value;
                    //cmd.Parameters.Add(paramname, DBNull.Value); // System.Data.SqlTypes.SqlBinary.Null);
                }
                else
                    cmd.Parameters.Add(paramname, paramType).Value = param[i];
            }
        }

        public void GetDataParam(string query, NpgsqlConnection connection, params object[] param)
        {
            start = DateTime.Now;
            using (NpgsqlCommand cmd = new NpgsqlCommand(query, connection))
            {
                FillParams(cmd, param);

                using (NpgsqlDataReader reader = cmd.ExecuteReader())
                {
                    data = new DataTable();
                    data.Locale = System.Globalization.CultureInfo.InvariantCulture;
                    data.Load(reader);
                }
            }
            end = DateTime.Now;
        }

        public void GetDataParamTimeout(string query, NpgsqlConnection connection, int commandTimeout_, params object[] param)
        {
            start = DateTime.Now;
            using (NpgsqlCommand cmd = new NpgsqlCommand(query, connection))
            {
                FillParams(cmd, param);

                cmd.CommandTimeout = (int)commandTimeout_;

                using (NpgsqlDataReader reader = cmd.ExecuteReader())
                {
                    data = new DataTable();
                    data.Locale = System.Globalization.CultureInfo.InvariantCulture;
                    data.Load(reader);
                }
            }
            end = DateTime.Now;
        }

        public void GetDataParam(NpgsqlTransaction transaction, string query, NpgsqlConnection connection, params object[] param)
        {
            start = DateTime.Now;
            using (NpgsqlCommand cmd = new NpgsqlCommand(query, connection))
            {
                cmd.Transaction = transaction;
                FillParams(cmd, param);
                using (NpgsqlDataReader reader = cmd.ExecuteReader())
                {
                    data = new DataTable();
                    data.Locale = System.Globalization.CultureInfo.InvariantCulture;
                    data.Load(reader);
                }
            }
            end = DateTime.Now;
        }

        public void ExecParam(string query, NpgsqlConnection connection, int? commandTimeout_, params object[] param)
        {
            start = DateTime.Now;
            using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
            {
                FillParams(command, param);

                if (commandTimeout_ != null && (int)commandTimeout_ > 0)
                    command.CommandTimeout = (int)commandTimeout_;

                command.ExecuteNonQuery();
            }
            end = DateTime.Now;
        }

        public async Task ExecParamAsync(string query, NpgsqlConnection connection, int? commandTimeout_, params object[] param)
        {
            start = DateTime.Now;
            using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
            {
                FillParams(command, param);

                if (commandTimeout_ != null && (int)commandTimeout_ > 0)
                    command.CommandTimeout = (int)commandTimeout_;

                await command.ExecuteNonQueryAsync();
            }
            end = DateTime.Now;
        }

        public void ExecParam(NpgsqlTransaction transaction, string query, NpgsqlConnection connection, params object[] param)
        {
            start = DateTime.Now;
            using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
            {
                command.Transaction = transaction;
                FillParams(command, param);
                command.ExecuteNonQuery();
            }
            end = DateTime.Now;
        }

        public static implicit operator DataRow(DBAnswer answer)
        {
            if (answer.HasData && answer.data.Rows.Count > 0) return answer.data.Rows[0];
            return null;
        }

        public int GetScalarInt
        {
            get
            {
                if (HasData && data.Rows.Count > 0)
                {
                    if (data.Rows[0].Table.Columns.Count > 0)
                        return DBConnection.GetIntOrNull(data.Rows[0].ItemArray[0], int.MinValue);
                }
                return int.MinValue;
            }
        }

        public Int64 GetScalarInt64
        {
            get
            {
                if (HasData && data.Rows.Count > 0)
                {
                    if (data.Rows[0].Table.Columns.Count > 0)
                        return DBConnection.GetInt64OrNull(data.Rows[0].ItemArray[0], Int64.MinValue);
                }
                return Int64.MinValue;
            }
        }
    }
}