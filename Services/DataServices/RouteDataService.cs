using CourseProgram.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using static CourseProgram.Models.Constants;

namespace CourseProgram.Services.DataServices
{
    public class RouteDataService : BaseService<Route>
    {
        public RouteDataService()
        {
            cnn = new DBConnection(Server, Database, User.Username, User.Password, "RouteLoad");
            temp = new Route();
        }

        public override async Task<bool> AddItemAsync(Route item)
        {
            query = $"Insert Into {temp.GetTable()} ({temp.GetSelectors()}) Values(@1, @2, @3, @4, @5, @6, @7);";

            try
            {
                await cnn.OpenAsync();
                var res = await cnn.ExecParamAsync(query, item.ID, item.MachineID, item.DriverID, item.Type, item.Distance, item.Status, item.CompleteTime);
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

        public override async Task<IEnumerable<Route>> GetFullTableAsync(bool forceRefresh = false)
        {
            query = $"Select {temp.GetSelectors()} From {temp.GetTable()};";

            try
            {
                items.Clear();
                await cnn.OpenAsync();
                foreach (DataRow row in cnn.GetDataTable(query))
                {
                    items.Add(new Route(
                        DBConnection.GetIntOrNull(row["КодМаршрута"], 0),
                        DBConnection.GetIntOrNull(row["КодМашины"], 0),
                        DBConnection.GetIntOrNull(row["КодВодителя"], 0),
                        DBConnection.GetStringOrNull(row["Тип"], string.Empty),
                        DBConnection.GetFloatOrNull(row["Расстояние"], 0),
                        DBConnection.GetStringOrNull(row["Статус"], string.Empty),
                        DBConnection.GetDateTimeOrNull(row["ВремяВыполнения"], DateTime.MinValue)
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

        public override async Task<Route> GetItemAsync(int id)
        {
            query = $"Select {temp.GetSelectors()} From {temp.GetTable()} Where \"КодМаршрута\" = @1;";

            try
            {
                await cnn.OpenAsync();
                DataRow row = cnn.GetDataTableParam(query, id);

                foreach (Route route in items)
                {
                    if (route.ID == DBConnection.GetIntOrNull(row["КодМаршрута"], 0)) return route;
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

        public override async Task<IEnumerable<Route>> GetItemsAsync(bool forceRefresh = false)
        {
            query = $"Select {temp.GetSelectors()} From {temp.GetTable()};";

            try
            {
                items.Clear();
                await cnn.OpenAsync();
                foreach (DataRow row in cnn.GetDataTable(query))
                {
                    items.Add(new Route(
                        DBConnection.GetIntOrNull(row["КодМаршрута"], 0),
                        DBConnection.GetIntOrNull(row["КодМашины"], 0),
                        DBConnection.GetIntOrNull(row["КодВодителя"], 0),
                        DBConnection.GetStringOrNull(row["Тип"], string.Empty),
                        DBConnection.GetFloatOrNull(row["Расстояние"], 0),
                        DBConnection.GetStringOrNull(row["Статус"], string.Empty),
                        DBConnection.GetDateTimeOrNull(row["ВремяВыполнения"], DateTime.MinValue)
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

        public async Task<IEnumerable<Route>> GetItemsByDriverAsync(int id)
        {
            query = $"Select {temp.GetSelectors()} From {temp.GetTable()} Where \"КодВодителя\" = @1 and \"Статус\" != \'Отменен\';";

            try
            {
                items.Clear();
                await cnn.OpenAsync();
                foreach (DataRow row in cnn.GetDataTableParam(query, id))
                {
                    items.Add(new Route(
                        DBConnection.GetIntOrNull(row["КодМаршрута"], 0),
                        DBConnection.GetIntOrNull(row["КодМашины"], 0),
                        DBConnection.GetIntOrNull(row["КодВодителя"], 0),
                        DBConnection.GetStringOrNull(row["Тип"], string.Empty),
                        DBConnection.GetFloatOrNull(row["Расстояние"], 0),
                        DBConnection.GetStringOrNull(row["Статус"], string.Empty),
                        DBConnection.GetDateTimeOrNull(row["ВремяВыполнения"], DateTime.MinValue)
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

        public async Task<IEnumerable<Route>> GetItemsByMachineAsync(int id)
        {
            query = $"Select {temp.GetSelectors()} From {temp.GetTable()} Where \"КодМашины\" = @1 and \"Статус\" != \'Отменен\';";

            try
            {
                items.Clear();
                await cnn.OpenAsync();
                foreach (DataRow row in cnn.GetDataTableParam(query, id))
                {
                    items.Add(new Route(
                        DBConnection.GetIntOrNull(row["КодМаршрута"], 0),
                        DBConnection.GetIntOrNull(row["КодМашины"], 0),
                        DBConnection.GetIntOrNull(row["КодВодителя"], 0),
                        DBConnection.GetStringOrNull(row["Тип"], string.Empty),
                        DBConnection.GetFloatOrNull(row["Расстояние"], 0),
                        DBConnection.GetStringOrNull(row["Статус"], string.Empty),
                        DBConnection.GetDateTimeOrNull(row["ВремяВыполнения"], DateTime.MinValue)
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