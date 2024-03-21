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

        public bool HasAnswer => _hasanswer;

        public TimeSpan ResponseTime => end - start;

        private DataTable? data;

        public bool HasData => data != null;
        public int Columns => HasData ? data.Columns.Count : 0;
        public int Rows => HasData ? data.Rows.Count : 0;

        public DBAnswer()
        {
            data = null;
        }

        public void Dispose()
        {
            data?.Dispose();
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

        #region Commands

        public void GetData(string query, NpgsqlConnection connection)
        {
            start = DateTime.Now;
            using (NpgsqlCommand cmd = new(query, connection))
            {
                using NpgsqlDataReader reader = cmd.ExecuteReader();
                data = new DataTable
                {
                    Locale = System.Globalization.CultureInfo.InvariantCulture
                };
                data.Load(reader);
            }
            end = DateTime.Now;
        }

        /// <summary>
        /// Формирует запрос с параметрами. Принимает null, bitmap, byte[]. Если значения DateTime = DateTime.MaxValue , то превращается в null
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="param"></param>
        private static void FillParams(NpgsqlCommand cmd, params object[] param)
        {
            param ??= new object[] { DBNull.Value };

            for (int i = 0; i < param.Length; i++)
            {
                if (param[i] == null) param[i] = DBNull.Value;
                if (param[i] is DateTime time && time == DateTime.MaxValue) param[i] = DBNull.Value;
                if (param[i] is DateTimeOffset offset && offset == DateTimeOffset.MaxValue) param[i] = DBNull.Value;
                if (param[i] is Int32 && ((int)param[i] == Int32.MinValue)) param[i] = DBNull.Value;

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
                }
                else
                    cmd.Parameters.Add(paramname, paramType).Value = param[i];
            }
        }

        public void GetDataParam(NpgsqlTransaction transaction, string query, NpgsqlConnection connection, params object[] param)
        {
            start = DateTime.Now;
            using (NpgsqlCommand cmd = new(query, connection))
            {
                cmd.Transaction = transaction;
                FillParams(cmd, param);
                using NpgsqlDataReader reader = cmd.ExecuteReader();
                data = new DataTable
                {
                    Locale = System.Globalization.CultureInfo.InvariantCulture
                };
                data.Load(reader);
            }
            end = DateTime.Now;
        }

        public async Task ExecParamAsync(string query, NpgsqlConnection connection, int? commandTimeout_, params object[] param)
        {
            start = DateTime.Now;
            using (NpgsqlCommand command = new(query, connection))
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
            using (NpgsqlCommand command = new(query, connection))
            {
                command.Transaction = transaction;
                FillParams(command, param);
                command.ExecuteNonQuery();
            }
            end = DateTime.Now;
        }

#endregion

        public static implicit operator DataRow(DBAnswer answer) => answer.HasData && answer.data.Rows.Count > 0 ? answer.data.Rows[0] : null;

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