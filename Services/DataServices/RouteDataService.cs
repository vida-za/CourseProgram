using CourseProgram.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using static CourseProgram.Models.Constants;

namespace CourseProgram.Services.DataServices
{
    public class RouteDataService : BaseService<Route>, IDataService<Route>
    {
        public RouteDataService()
        {
            cnn = new DBConnection(Server, Database, User.Username, User.Password, "RouteLoad");
        }

        public async Task<int> FindMaxEmptyID()
        {
            int i = 0;
            var temp = await GetItemsAsync();
            for (i = 1; i < temp.Count(); ++i)
            {
                if (!temp.Any(d => d.ID == i))
                    return await Task.FromResult(i);
            }
            return await Task.FromResult(i);
        }

        public async Task<bool> AddItemAsync(Route item)
        {
            query = "Insert Into " + Route.GetTable() + "(\"КодМаршрута\", \"КодМашины\", \"КодВодителя\", \"Тип\", \"Расстояние\", \"Статус\", \"ВремяВыполнения\") Values(@1, @2, @3, @4, @5, @6);";

            try
            {
                await cnn.OpenAsync();
                var res = await cnn.ExecParamAsync(query, item.ID, item.MachineID, item.DriverID, item.Type, item.Distance, item.Status, DateTime.Now);
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

        public async Task<bool> DeleteItemAsync(int id)
        {
            var current = await GetItemAsync(id);
            if (current == null) return false;

            query = "Delete From " + Route.GetTable() + " Where \"КодМаршрута\" = @1;";

            try
            {
                await cnn.OpenAsync();
                var res = await cnn.ExecParamAsync(query, id);
            }
            catch (Exception) { }
            finally
            {
                cnn.Close();
                query = string.Empty;
            }
            return await Task.FromResult(true);
        }

        public async Task<Route> GetItemAsync(int id)
        {
            query = "Select * From " + Route.GetTable() + " Where \"КодМаршрута\" = @1;";

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
            return new Route();
        }

        public async Task<IEnumerable<Route>> GetItemsAsync(bool forceRefresh = false)
        {
            query = "Select * From " + Route.GetTable() + ";";

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
            query = "Select * From " + Route.GetTable() + " Where \"КодВодителя\" = @1 and \"Статус\" != \'Отменен\';";

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

        public async Task<bool> UpdateItemAsync(Route item)
        {
            query = "Update " + Route.GetTable() + " Set";
            var oldItem = items.FirstOrDefault(d => d == item);
            if (oldItem == null) return await Task.FromResult(false);

            var values = new List<object>();
            var propertiesToUpdate = new Dictionary<string, object>();
            int countModify = 0;

            PropertyInfo[] properties = typeof(Route).GetProperties();
            foreach (PropertyInfo property in properties)
            {
                var oldValue = property.GetValue(oldItem);
                var newValue = property.GetValue(item);
                if (!Equals(oldValue, newValue))
                {
                    propertiesToUpdate[property.Name] = newValue;
                    CreatingUpdateQuery(property.Name, ref countModify, ref values, newValue);
                }
            }

            if (countModify == 0) return await Task.FromResult(false);

            query += ";";

            try
            {
                await cnn.OpenAsync();
                var res = await cnn.ExecParamAsync(query);
                if (res.HasAnswer)
                {
                    items.Remove(oldItem);
                    items.Add(item);
                }
            }
            catch (Exception) { }
            finally
            {
                cnn.Close();
                query = string.Empty;
            }
            return await Task.FromResult(true);
        }

        private async Task<int> GetCountRowsAsync()
        {
            query = "Select Count(*) From " + Route.GetTable() + ";";

            try
            {
                await cnn.OpenAsync();
                var res = cnn.Query_Scalar(query);
                return await Task.FromResult(DBConnection.GetIntOrNull(res, 0));
            }
            catch (Exception) { }
            finally
            {
                cnn.Close();
                query = string.Empty;
            }
            return await Task.FromResult(0);
        }

        private void CreatingUpdateQuery(string column, ref int index, ref List<object> list, object value)
        {
            if (index > 0) query += ",";
            index++;
            query += $"\"{column}\" = @{index}";
            list.Add(value);
        }
    }
}
