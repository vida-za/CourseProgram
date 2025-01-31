using CourseProgram.Models;
using CourseProgram.Services.DBServices;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using static CourseProgram.Models.Constants;

namespace CourseProgram.Services.DataServices
{
    public class OrderDataService : BaseService<Order>
    {
        public OrderDataService() : base(User.Username, User.Password) { }

        public async Task<IEnumerable<Order>> GetOrdersByHaulAsync(int HaulID)
        {
            using (var query = new Query(CommandTypes.TableFunction, "GetOrdersByHaul"))
            {
                try
                {
                    items.Clear();

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
                            CreateElement(row);
                        }
                    }

                    return await Task.FromResult(items);
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

        public override void CreateElement(DataRow row)
        {
            items.Add(new Order(GetInt(row["КодЗаказа"], 0),
                GetInt(row["КодЗаявки"], 0),
                GetDateTime(row["ДатаЗаказа"], DateTime.MinValue),
                GetDateTimeOrNull(row["ДатаЗагрузки"]),
                GetDateTimeOrNull(row["ДатаВыгрузки"]),
                GetFloatOrNull(row["Стоимость"]),
                GetString(row["Статус"], string.Empty),
                GetStringOrNull(row["Договор"])
                ));
        }
    }
}