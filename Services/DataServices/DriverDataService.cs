using CourseProgram.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using static CourseProgram.Models.Constants;

namespace CourseProgram.Services.DataServices
{
    public class DriverDataService : BaseService<Driver>
    {
        public DriverDataService() 
        {
            cnn = new DBConnection(Server, Database, User.Username, User.Password, "DriverLoad");
            temp = new Driver();
        }

        public override async Task<bool> AddItemAsync(Driver item)
        {
            query = $"Insert Into {temp.GetTable()} ({temp.GetSelectors()}) Values(@1, @2, @3, @4, @5, @6, @7);";

            try
            {
                await cnn.OpenAsync();
                var res = await cnn.ExecParamAsync(query, item.ID, item.FIO, item.BirthDay, item.Passport, item.Phone, item.DateStart, item.DateEnd);
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

        public override async Task<bool> DeleteItemAsync(int id)
        {
            var current = await GetItemAsync(id);
            if (current == null) return false;
            if (current.DateEnd != DateOnly.MinValue && current.DateEnd <= DateOnly.FromDateTime(DateTime.Now)) return false;

            query = $"Update {temp.GetTable()} Set \"ДатаОкончания\" = current_timestamp Where \"КодВодителя\" = @1;";

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

        public override async Task<Driver> GetItemAsync(int id)
        {
            query = $"Select {temp.GetSelectors()} From {temp.GetTable()} Where \"КодВодителя\" = @1;";

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
            return temp;
        }

        public override async Task<IEnumerable<Driver>> GetFullTableAsync(bool forceRefresh = false)
        {
            query = $"Select {temp.GetSelectors()} From {temp.GetTable()};";

            try
            {
                items.Clear();
                await cnn.OpenAsync();
                foreach (DataRow row in cnn.GetDataTable(query))
                {
                    items.Add(new Driver(
                        DBConnection.GetIntOrNull(row["КодВодителя"], 0),
                        DBConnection.GetStringOrNull(row["ФИО"], string.Empty),
                        DBConnection.GetDateOnlyOrNull(row["ДатаРождения"], DateOnly.MinValue),
                        DBConnection.GetStringOrNull(row["ПаспортныеДанные"], string.Empty),
                        DBConnection.GetStringOrNull(row["Телефон"], string.Empty),
                        DBConnection.GetDateOnlyOrNull(row["ДатаНачала"], DateOnly.MinValue),
                        DBConnection.GetDateOnlyOrNull(row["ДатаОкончания"], DateOnly.MinValue),
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

        public override async Task<IEnumerable<Driver>> GetItemsAsync(bool forceRefresh = false)
        {
            query = $"Select {temp.GetSelectors()}, \"GetTownNowByDriver\"(\"КодВодителя\") as \"Город\" From {temp.GetTable()} Where \"ДатаОкончания\" is Null;";

            try
            {
                items.Clear();
                await cnn.OpenAsync();
                foreach (DataRow row in cnn.GetDataTable(query))
                {
                    items.Add(new Driver(
                        DBConnection.GetIntOrNull(row["КодВодителя"], 0),
                        DBConnection.GetStringOrNull(row["ФИО"], string.Empty),
                        DBConnection.GetDateOnlyOrNull(row["ДатаРождения"], DateOnly.MinValue),
                        DBConnection.GetStringOrNull(row["ПаспортныеДанные"], string.Empty),
                        DBConnection.GetStringOrNull(row["Телефон"], string.Empty),
                        DBConnection.GetDateOnlyOrNull(row["ДатаНачала"], DateOnly.MinValue),
                        DBConnection.GetDateOnlyOrNull(row["ДатаОкончания"], DateOnly.MinValue),
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
                        DBConnection.GetDateOnlyOrNull(row["ДатаРождения"], DateOnly.MinValue),
                        DBConnection.GetStringOrNull(row["ПаспортныеДанные"], string.Empty),
                        DBConnection.GetStringOrNull(row["Телефон"], string.Empty),
                        DBConnection.GetDateOnlyOrNull(row["ДатаНачала"], DateOnly.MinValue),
                        DBConnection.GetDateOnlyOrNull(row["ДатаОкончания"], DateOnly.MinValue),
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
            query = $"Select {temp.GetSelectors()} From {temp.GetTable()} Where \"ДатаОкончания\" is not Null;";

            try
            {
                items.Clear();
                await cnn.OpenAsync();
                foreach (DataRow row in cnn.GetDataTable(query))
                {
                    items.Add(new Driver(
                        DBConnection.GetIntOrNull(row["КодВодителя"], 0),
                        DBConnection.GetStringOrNull(row["ФИО"], string.Empty),
                        DBConnection.GetDateOnlyOrNull(row["ДатаРождения"], DateOnly.MinValue),
                        DBConnection.GetStringOrNull(row["ПаспортныеДанные"], string.Empty),
                        DBConnection.GetStringOrNull(row["Телефон"], string.Empty),
                        DBConnection.GetDateOnlyOrNull(row["ДатаНачала"], DateOnly.MinValue),
                        DBConnection.GetDateOnlyOrNull(row["ДатаОкончания"], DateOnly.MinValue),
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
    }
}