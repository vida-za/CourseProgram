using Npgsql;
using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using static CourseProgram.Models.Constants;

namespace CourseProgram.Services.DBServices
{
    public class Connection : IAsyncDisposable
    {
        private readonly string Server;
        private readonly string Database;
        private readonly string Login;
        private readonly string Password;

        volatile private int connectionState;
        public int ConnectionState => connectionState;

        private NpgsqlConnection cnn;
        private NpgsqlConnectionStringBuilder ConnectionString;

        private readonly SemaphoreSlim _connectionLock = new SemaphoreSlim(1, 1);

        public Connection(string server, string database, string login, string password)
        {
            Server = server;
            Database = database;
            Login = login;
            Password = password;
            ConnectionString = CreateConnectionString();
        }

        public Connection(Connection baseCon)
        {
            Server = baseCon.Server;
            Database = baseCon.Database;
            Login = baseCon.Login;
            Password = baseCon.Password;
            ConnectionString = CreateConnectionString();
        }

        public async Task OpenAsync()
        {
            await _connectionLock.WaitAsync();
            try
            {
                if (cnn != null && cnn.State == System.Data.ConnectionState.Open)
                    return;

                await CloseAsync();
                cnn = new NpgsqlConnection(ConnectionString.ConnectionString);
                await cnn.OpenAsync();

                if (cnn.State != System.Data.ConnectionState.Open)
                {
                    throw new InvalidOperationException("Не удалось открыть соединение.");
                }

                connectionState = 1;
            }
            catch (NpgsqlException ex)
            {
                await LogManager.Instance.WriteLogAsync($"Ошибка подключения: {ex.Message}");
                await CloseAsync();
            }
            finally
            {
                _connectionLock.Release();
            }
        }

        public async ValueTask DisposeAsync()
        {
            if (cnn != null) 
            {
                await cnn.CloseAsync();
                await cnn.DisposeAsync();
            }
        }

        public async Task CloseAsync()
        {
            connectionState = 0;

            if (cnn != null && cnn.State == System.Data.ConnectionState.Open)
            {
                await cnn.CloseAsync();
                await cnn.DisposeAsync();
            }
        }

        private async Task LogConnectionStateAsync()
        {
            await LogManager.Instance.WriteLogAsync($"Connection state: {cnn?.State.ToString() ?? "No connection"}");
        }

        public async Task<T> ExecuteQueryAsync<T>(Query query) 
        {
            try
            {
                if (cnn == null || cnn.State != System.Data.ConnectionState.Open)
                    await OpenAsync();

                query.CollectQuery();

                using NpgsqlCommand command = new NpgsqlCommand(query.ToString(), cnn);
                await LogManager.Instance.WriteLogAsync($"Выполняется запрос {command.CommandText}");

                switch (query.GetType())
                {
                    case CommandTypes.SelectQuery:
                    case CommandTypes.TableFunction:
                        {
                            command.CommandType = CommandType.Text;
                            using (NpgsqlDataReader reader = await command.ExecuteReaderAsync())
                            {
                                if (typeof(T) == typeof(DataTable))
                                {
                                    DataTable data = new DataTable
                                    {
                                        Locale = System.Globalization.CultureInfo.InvariantCulture,
                                    };
                                    data.Load(reader);
                                    return (T)(object)data;
                                }
                                throw new InvalidCastException($"Invalid type parameter: Expected {typeof(T)}, but got DataTable");
                            }
                        }

                    case CommandTypes.InsertQuery:
                    case CommandTypes.UpdateQuery:
                    case CommandTypes.DeleteQuery:
                    case CommandTypes.Procedure:
                        {
                            if (typeof(T) == typeof(int))
                            {
                                int result = await command.ExecuteNonQueryAsync();
                                return (T)(object)result;
                            }
                            throw new InvalidCastException($"Invalid type parameter: Expected {typeof(T)}, but got int");
                        }

                    case CommandTypes.ScalarFunction:
                        {
                            if (typeof(T) == typeof(int))
                            {
                                object? result = await command.ExecuteScalarAsync();
                                if (result != null && int.TryParse(result.ToString(), out int scalarResult)) 
                                {
                                    return (T)(object)scalarResult;
                                }
                                throw new InvalidCastException($"Failed to convert result to int");
                            }
                            throw new InvalidCastException($"Invalid type parameter: Expected {typeof(T)}, but got int");
                        }

                    default:
                        throw new InvalidOperationException($"Unknown type query: {query.GetType()}");

                }
            }
            catch (PostgresException ex)
            {
                await LogManager.Instance.WriteLogAsync($"Postgres error: {ex.Message}");
                await CloseAsync();
                await LogConnectionStateAsync();
                throw;
            }
            catch (InvalidOperationException ex)
            {
                await LogManager.Instance.WriteLogAsync($"Invalid operation: {ex.Message}");
                await LogConnectionStateAsync();
                throw;
            }
            catch (InvalidCastException ex)
            {
                await LogManager.Instance.WriteLogAsync($"Invalid Cast: {ex.Message}");
                await LogConnectionStateAsync();
                throw;
            }
            catch (Exception ex)
            {
                var queryText = query?.ToString() ?? "Query is null or failed to generate";
                await LogManager.Instance.WriteLogAsync($"General error: {ex.Message}");
                await LogManager.Instance.WriteLogAsync($"Query Error: {queryText}");
                await CloseAsync();
                await LogConnectionStateAsync();
                throw;
            }
        }

        private NpgsqlConnectionStringBuilder CreateConnectionString()
        {
            return new NpgsqlConnectionStringBuilder
            {
                Host = Server,
                Database = Database,
                Multiplexing = false,
                Enlist = true,
                Pooling = true,
                Username = Login,
                Password = Password,
            };
        }
    }
}