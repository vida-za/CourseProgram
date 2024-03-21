﻿using CourseProgram.Models;
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
        public DriverDataService() 
        {
            cnn = new DBConnection(Server, Database, User.Username, User.Password, "DriverLoad");
        }

        public int FindMaxID()
        {
            Driver driver = items.OrderByDescending(d => d.ID).FirstOrDefault();
            return driver != null ? driver.ID : 0;
        }

        public async Task<bool> AddItemAsync(Driver item)
        {
            query = "Insert Into " + Driver.GetTable() + "(" + Driver.GetSelectors() + ") Values(@1, @2, @3, @4, @5, @6);";

            try
            {
                await cnn.OpenAsync();
                var res = await cnn.ExecParamAsync(query, item.FIO, item.BirthDay, item.Passport, item.Phone, item.DateStart, item.DateEnd);
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
            query = "Delete From " + Driver.GetTable() + " Where \"КодВодителя\" = @1;";

            try
            {
                await cnn.OpenAsync();
                var res = await cnn.ExecParamAsync(query, id);
                if (res.HasAnswer)
                    foreach (Driver drv in items)
                    {
                        if (drv.ID == id)
                            items.Remove(drv);
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

        public async Task<Driver> GetItemAsync(int id)
        {
            query = "Select \"КодВодителя\", " + Driver.GetSelectors() + " From " + Driver.GetTable() + " Where \"КодВодителя\" = @1;";

            try
            {
                await cnn.OpenAsync();
                DataRow row = await cnn.ExecParamAsync(query, id);

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

        public async Task<IEnumerable<Driver>> GetItemsAsync(bool forceRefresh = false)
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
                        DBConnection.GetDateTimeOrNull(row["ДатаОкончания"], DateTime.MinValue)
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

        private void CreatingUpdateQuery(string column, ref int index, ref List<object> list, object value)
        {
            if (index > 0) query += ",";
            index++;
            query += $"\"{column}|\" = @{index}";
            list.Add(value);
        }
    }
}