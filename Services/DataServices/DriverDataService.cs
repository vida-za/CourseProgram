using CourseProgram.Models;
using CourseProgram.Services.DBServices;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using static CourseProgram.Models.Constants;

namespace CourseProgram.Services.DataServices
{
    public class DriverDataService : BaseService<Driver>
    {
        public DriverDataService() : base(User.Username, User.Password) { }

        public async Task<IEnumerable<Driver>> GetActDriversAsync()
        {
            using (var query = new Query(CommandTypes.SelectQuery, Driver.GetTable()))
            {
                try
                {
                    var tempItems = new List<Driver>();

                    query.AddFields(Driver.GetFieldNames());
                    query.WhereClause.IsNull("ДатаОкончания");

                    DataTable data;

                    await using (var con = new Connection(connection))
                    {
                        await con.OpenAsync();
                        data = await con.ExecuteQueryAsync<DataTable>(query);
                    }

                    if (data != null)
                    {
                        foreach (DataRow row in data.Rows)
                            tempItems.Add(await CreateElement(row));
                    }

                    return await Task.FromResult(tempItems);
                }
                catch (Exception ex)
                {
                    await LogManager.Instance.WriteLogAsync($"Error in {nameof(GetItemsAsync)}: {ex.Message}");
                    throw;
                }
                finally
                {
                    query?.Dispose();
                }
            }
        }

        public async Task<IEnumerable<Driver>> GetDisDriversAsync()
        {
            using (var query = new Query(CommandTypes.SelectQuery, Driver.GetTable()))
            {
                try
                {
                    var tempItems = new List<Driver>();

                    query.AddFields(Driver.GetFieldNames());
                    query.WhereClause.IsNotNull("ДатаОкончания");

                    DataTable data;

                    await using (var con = new Connection(connection))
                    {
                        await con.OpenAsync();
                        data = await con.ExecuteQueryAsync<DataTable>(query);
                    }

                    if (data != null)
                    {
                        foreach (DataRow row in data.Rows)
                            tempItems.Add(await CreateElement(row));
                    }

                    return await Task.FromResult(tempItems);
                }
                catch (Exception ex)
                {
                    await LogManager.Instance.WriteLogAsync($"Error in {nameof(GetItemsAsync)}: {ex.Message}");
                    throw;
                }
                finally
                {
                    query?.Dispose();
                }
            }
        }

        public override Task<Driver> CreateElement(DataRow row)
        {
            return Task.FromResult(new Driver(
                GetInt(row["КодВодителя"], 0),
                GetString(row["ФИО"], string.Empty),
                GetDateOnlyOrNull(row["ДатаРождения"]),
                GetString(row["ПаспортныеДанные"], string.Empty),
                GetStringOrNull(row["Телефон"]),
                GetDateOnly(row["ДатаНачала"], DateOnly.MinValue),
                GetDateOnlyOrNull(row["ДатаОкончания"])));
        }
    }
}