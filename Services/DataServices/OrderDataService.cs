using CourseProgram.Models;
using CourseProgram.Services.DBServices;
using CourseProgram.Stores;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using static CourseProgram.Models.Constants;

namespace CourseProgram.Services.DataServices
{
    public class OrderDataService : BaseService<Order>
    {
        public OrderDataService(DataStore dataStore) : base(User.Username, User.Password, dataStore) { }

        public async Task<IEnumerable<Order>> GetOrdersByHaulAsync(int HaulID)
        {
            using (var query = new Query(CommandTypes.TableFunction, "GetOrdersByHaul"))
            {
                try
                {
                    var tempItems = new List<Order>();

                    query.AddFields(Order.GetFieldNames());
                    query.AddParameter("HaulID", HaulID);

                    DataTable data;

                    await using (var con = new Connection(connection))
                    {
                        await con.OpenAsync();
                        data = await con.ExecuteQueryAsync<DataTable>(query);
                    }

                    if (data != null)
                    {
                        foreach (DataRow row in data.Rows)
                        {
                            tempItems.Add(await CreateElement(row));
                        }
                    }

                    return await Task.FromResult(tempItems);
                }
                catch (Exception ex)
                {
                    await LogManager.Instance.WriteLogAsync($"Error in {nameof(GetOrdersByHaulAsync)}: {ex.Message}");
                    throw;
                }
                finally
                {
                    query?.Dispose();
                }
            }
        }

        public override Task<Order> CreateElement(DataRow row)
        {
            return Task.FromResult(new Order(GetInt(row["КодЗаказа"], 0),
                GetInt(row["КодЗаявки"], 0),
                GetDateTime(row["ДатаЗаказа"], DateTime.MinValue),
                GetDateTimeOrNull(row["ДатаЗагрузки"]),
                GetDateTimeOrNull(row["ДатаВыгрузки"]),
                GetFloatOrNull(row["Стоимость"]),
                GetString(row["Статус"], string.Empty),
                GetStringOrNull(row["КонтактПогрузки"]),
                GetStringOrNull(row["КонтактРазгрузки"])
                ));
        }
    }
}