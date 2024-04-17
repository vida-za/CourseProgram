using CourseProgram.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using static CourseProgram.Models.Constants;

namespace CourseProgram.Services.DataServices
{
    public class OrderDataService : BaseService<Order>
    {
        public OrderDataService()
        {
            cnn = new DBConnection(Server, Database, User.Username, User.Password, "OrderLoad");
            temp = new Order();
        }

        public override async Task<bool> AddItemAsync(Order item)
        {
            query = $"Insert Into {temp.GetTable()} ({temp.GetSelectors()}) Values(@1, @2, @3, @4, @5, @6, @7, @8, @9);";

            try
            {
                await cnn.OpenAsync();
                var res = await cnn.ExecParamAsync(
                    query,
                    item.ID,
                    item.WorkerID,
                    item.ClientID,
                    item.TimeOrder,
                    item.TimeLoad,
                    item.TimeOnLoad,
                    item.Price,
                    item.Status,
                    item.File);
                if (res.HasAnswer) items.Add(item);
            }
            catch (Exception) { }
            finally
            {
                cnn.Close();
                query = string.Empty;
            }
            return await Task.FromResult(true);
        }

        public override async Task<IEnumerable<Order>> GetFullTableAsync(bool forceRefresh = false)
        {
            query = $"Select {temp.GetSelectors()} From {temp.GetTable()};";

            try
            {
                items.Clear();
                await cnn.OpenAsync();
                foreach (DataRow row in cnn.GetDataTable(query))
                {
                    items.Add(new Order(
                        DBConnection.GetIntOrNull(row["КодЗаказа"], 0),
                        DBConnection.GetIntOrNull(row["КодСотрудника"], 0),
                        DBConnection.GetIntOrNull(row["КодЗаказчика"], 0),
                        DBConnection.GetDateTimeOrNull(row["ДатаЗаказа"], DateTime.MinValue),
                        DBConnection.GetDateTimeOrNull(row["ДатаЗагрузки"], DateTime.MinValue),
                        DBConnection.GetDateTimeOrNull(row["ДатаВыгрузки"], DateTime.MinValue),
                        DBConnection.GetFloatOrNull(row["Стоимость"], 0),
                        DBConnection.GetStringOrNull(row["Статус"], string.Empty),
                        DBConnection.GetStringOrNull(row["Договор"], string.Empty)
                        ));
                }
            }
            catch (Exception) { }
            finally
            {
                cnn.Close();
                query = string.Empty;
            }
            return await Task.FromResult(items);
        }

        public override async Task<Order> GetItemAsync(int id)
        {
            query = $"Select {temp.GetSelectors()} From {temp.GetTable()} Where \"КодЗаказа\" = @1;";

            try
            {
                await cnn.OpenAsync();
                DataRow row = cnn.GetDataTableParam(query, id);

                foreach (Order order in items)
                {
                    if (order.ID == DBConnection.GetIntOrNull(row["КодЗаказа"], 0)) return order;
                }
            }
            catch (Exception) { }
            finally
            {
                cnn.Close();
                query = string.Empty;
            }
            return temp;
        }

        public override async Task<IEnumerable<Order>> GetItemsAsync(bool forceRefresh = false)
        {
            query = $"Select {temp.GetSelectors()} From {temp.GetTable()};";

            try
            {
                items.Clear();
                await cnn.OpenAsync();
                foreach (DataRow row in cnn.GetDataTable(query))
                {
                    items.Add(new Order(
                        DBConnection.GetIntOrNull(row["КодЗаказа"], 0),
                        DBConnection.GetIntOrNull(row["КодСотрудника"], 0),
                        DBConnection.GetIntOrNull(row["КодЗаказчика"], 0),
                        DBConnection.GetDateTimeOrNull(row["ДатаЗаказа"], DateTime.MinValue),
                        DBConnection.GetDateTimeOrNull(row["ДатаЗагрузки"], DateTime.MinValue),
                        DBConnection.GetDateTimeOrNull(row["ДатаВыгрузки"], DateTime.MinValue),
                        DBConnection.GetFloatOrNull(row["Стоимость"], 0),
                        DBConnection.GetStringOrNull(row["Статус"], string.Empty),
                        DBConnection.GetStringOrNull(row["Договор"], string.Empty)
                        ));
                }
            }
            catch (Exception) { }
            finally
            {
                cnn.Close();
                query = string.Empty;
            }
            return await Task.FromResult(items);
        }
    }
}