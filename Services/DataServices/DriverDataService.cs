using CourseProgram.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using static CourseProgram.Models.Constants;

namespace CourseProgram.Services.DataServices
{
    public class DriverDataService : BaseService<Driver>, IDataService<Driver>
    {
        //private readonly List<Driver> _driversFree;

        public DriverDataService() 
        {
            cnn = new DBConnection(Server, Database, User.Username, User.Password, "DriverLoad");

            //_driversFree = new List<Driver>();
        }

        public async Task<int> FindMaxEmptyID()
        {
            int i = 0;
            var temp = await GetFullTableAsync();
            for (i = 1; i < temp.Count(); ++i)
            {
                if (!temp.Any(d => d.ID == i))
                    return await Task.FromResult(i);
            }
            return await Task.FromResult(i);
        }

        public async Task<bool> AddItemAsync(Driver item)
        {
            query = "Insert Into " + Driver.GetTable() + "(\"КодВодителя\", \"ФИО\", \"ДатаРождения\", \"ПаспортныеДанные\", \"Телефон\", \"ДатаНачала\") Values(@1, @2, @3, @4, @5, @6);";

            try
            {
                await cnn.OpenAsync();
                var res = await cnn.ExecParamAsync(query, item.ID, item.FIO, item.BirthDay, item.Passport, item.Phone, item.DateStart);
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
            if (current.DateEnd != DateTime.MinValue && current.DateEnd <= DateTime.Now) return false;

            query = "Update " + Driver.GetTable() + "Set \"ДатаОкончания\" = current_timestamp Where \"КодВодителя\" = @1;";

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

        public async Task<Driver> GetItemAsync(int id)
        {
            query = "Select \"КодВодителя\", " + Driver.GetSelectors() + " From " + Driver.GetTable() + " Where \"КодВодителя\" = @1;";

            try
            {
                await cnn.OpenAsync();
                DataRow row = cnn.GetDataTableParam(query, id);

                foreach (Driver drv in items)
                {
                    if (drv.ID == DBConnection.GetIntOrNull(row["КодВодителя"], 0)) return drv;
                }
            }
            catch (Exception) { }
            finally
            {
                cnn.Close();
                query = string.Empty;
            }
            return new Driver();
        }

        public async Task<IEnumerable<Driver>> GetFullTableAsync(bool forceRefresh = false)
        {
            query = "Select \"КодВодителя\", " + Driver.GetSelectors() + " From " + Driver.GetTable() + ";";

            try
            {
                items.Clear();
                await cnn.OpenAsync();
                foreach (DataRow row in cnn.GetDataTable(query))
                {
                    items.Add(new Driver(
                        DBConnection.GetIntOrNull(row["КодВодителя"], 0),
                        DBConnection.GetStringOrNull(row["ФИО"], string.Empty),
                        DBConnection.GetDateTimeOrNull(row["ДатаРождения"], DateTime.MinValue),
                        DBConnection.GetStringOrNull(row["ПаспортныеДанные"], string.Empty),
                        DBConnection.GetStringOrNull(row["Телефон"], string.Empty),
                        DBConnection.GetDateTimeOrNull(row["ДатаНачала"], DateTime.MinValue),
                        DBConnection.GetDateTimeOrNull(row["ДатаОкончания"], DateTime.MinValue),
                        string.Empty
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

        public async Task<IEnumerable<Driver>> GetItemsAsync(bool forceRefresh = false)
        {
            query = "Select \"КодВодителя\", " + Driver.GetSelectors() + ", \"GetTownNowByDriver\"(\"КодВодителя\") as \"Город\" From " + Driver.GetTable() + " Where \"ДатаОкончания\" is Null;";

            try
            {
                items.Clear();
                await cnn.OpenAsync();
                foreach (DataRow row in cnn.GetDataTable(query))
                {
                    items.Add(new Driver(
                        DBConnection.GetIntOrNull(row["КодВодителя"], 0),
                        DBConnection.GetStringOrNull(row["ФИО"], string.Empty),
                        DBConnection.GetDateTimeOrNull(row["ДатаРождения"], DateTime.MinValue),
                        DBConnection.GetStringOrNull(row["ПаспортныеДанные"], string.Empty),
                        DBConnection.GetStringOrNull(row["Телефон"], string.Empty),
                        DBConnection.GetDateTimeOrNull(row["ДатаНачала"], DateTime.MinValue),
                        DBConnection.GetDateTimeOrNull(row["ДатаОкончания"], DateTime.MinValue),
                        DBConnection.GetStringOrNull(row["Город"], string.Empty)
                    ));
                }
            }
            catch(Exception) { }
            finally
            {
                cnn.Close();
                query = string.Empty;
            }
            return await Task.FromResult(items);
        }

        public async Task<IEnumerable<Driver>> GetFreeDriversAsync(bool forceRefresh = false)
        {
            query = "Select * From \"GetFreeDrivers\"();";

            try
            {
                items.Clear();
                await cnn.OpenAsync();
                foreach (DataRow row in cnn.GetDataTable(query))
                {
                    items.Add(new Driver(
                        DBConnection.GetIntOrNull(row["КодВодителя"], 0),
                        DBConnection.GetStringOrNull(row["ФИО"], string.Empty),
                        DBConnection.GetDateTimeOrNull(row["ДатаРождения"], DateTime.MinValue),
                        DBConnection.GetStringOrNull(row["ПаспортныеДанные"], string.Empty),
                        DBConnection.GetStringOrNull(row["Телефон"], string.Empty),
                        DBConnection.GetDateTimeOrNull(row["ДатаНачала"], DateTime.MinValue),
                        DBConnection.GetDateTimeOrNull(row["ДатаОкончания"], DateTime.MinValue),
                        DBConnection.GetStringOrNull(row["Город"], string.Empty)
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

        public async Task<IEnumerable<Driver>> GetDisDriversAsync(bool forceRefresh = false)
        {
            query = "Select \"КодВодителя\", " + Driver.GetSelectors() + " From " + Driver.GetTable() + " Where \"ДатаОкончания\" is not Null;";

            try
            {
                items.Clear();
                await cnn.OpenAsync();
                foreach (DataRow row in cnn.GetDataTable(query))
                {
                    items.Add(new Driver(
                        DBConnection.GetIntOrNull(row["КодВодителя"], 0),
                        DBConnection.GetStringOrNull(row["ФИО"], string.Empty),
                        DBConnection.GetDateTimeOrNull(row["ДатаРождения"], DateTime.MinValue),
                        DBConnection.GetStringOrNull(row["ПаспортныеДанные"], string.Empty),
                        DBConnection.GetStringOrNull(row["Телефон"], string.Empty),
                        DBConnection.GetDateTimeOrNull(row["ДатаНачала"], DateTime.MinValue),
                        DBConnection.GetDateTimeOrNull(row["ДатаОкончания"], DateTime.MinValue),
                        string.Empty
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

        public async Task<bool> UpdateItemAsync(Driver item)
        {
            query = "Update " + Driver.GetTable() + " Set";
            var oldItem = items.FirstOrDefault(d => d == item);
            if (oldItem == null) return await Task.FromResult(false); 

            var values = new List<object>();
            var propertiesToUpdate = new Dictionary<string, object>();
            int countModify = 0;

            PropertyInfo[] properties = typeof(Driver).GetProperties();
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
            query = "Select Count(*) From " + Driver.GetTable() + ";";

            try
            {
                await cnn.OpenAsync();
                var res = cnn.Query_Scalar(query);
                return await Task.FromResult(DBConnection.GetIntOrNull(res, 0));
            }
            catch(Exception) { }
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
            query += $"\"{column}|\" = @{index}";
            list.Add(value);
        }
    }
}