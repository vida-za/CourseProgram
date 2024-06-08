using CourseProgram.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using static CourseProgram.Models.Constants;

namespace CourseProgram.Services.DataServices
{
    public class WorkerDataService : BaseService<Worker>
    {
        public WorkerDataService() 
        {
            cnn = new DBConnection(Server, Database, User.Username, User.Password, "WorkerLoad");
            temp = new Worker();
        }

        public override async Task<bool> AddItemAsync(Worker item)
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

            query = $"Update {temp.GetTable()} Set \"ДатаОкончания\" = current_timestamp Where \"КодСотрудника\" = @1;";

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

        public override async Task<IEnumerable<Worker>> GetFullTableAsync(bool forceRefresh = false)
        {
            query = $"Select {temp.GetSelectors()} From {temp.GetTable()};";

            try
            {
                items.Clear();
                await cnn.OpenAsync();
                foreach (DataRow row in cnn.GetDataTable(query))
                {
                    items.Add(new Worker(
                        DBConnection.GetIntOrNull(row["КодСотрудника"], 0),
                        DBConnection.GetStringOrNull(row["ФИО"], string.Empty),
                        DBConnection.GetDateOnlyOrNull(row["ДатаРождения"], DateOnly.MinValue),
                        DBConnection.GetStringOrNull(row["ПаспортныеДанные"], string.Empty),
                        DBConnection.GetStringOrNull(row["Телефон"], string.Empty),
                        DBConnection.GetDateOnlyOrNull(row["ДатаНачала"], DateOnly.MinValue),
                        DBConnection.GetDateOnlyOrNull(row["ДатаОкончания"], DateOnly.MinValue)
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

        public override async Task<Worker> GetItemAsync(int id)
        {
            query = $"Select {temp.GetSelectors()} From {temp.GetTable()} Where \"КодСотрудника\" = @1;";

            try
            {
                await cnn.OpenAsync();
                DataRow row = cnn.GetDataTableParam(query, id);

                foreach (Worker wrk in items)
                {
                    if (wrk.ID == DBConnection.GetIntOrNull(row["КодСотрудника"], 0)) return wrk;
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

        public override async Task<IEnumerable<Worker>> GetItemsAsync(bool forceRefresh = false)
        {
            query = $"Select {temp.GetSelectors()} From {temp.GetTable()};";

            try
            {
                items.Clear();
                await cnn.OpenAsync();
                foreach (DataRow row in cnn.GetDataTable(query))
                {
                    items.Add(new Worker(
                        DBConnection.GetIntOrNull(row["КодСотрудника"], 0),
                        DBConnection.GetStringOrNull(row["ФИО"], string.Empty),
                        DBConnection.GetDateOnlyOrNull(row["ДатаРождения"], DateOnly.MinValue),
                        DBConnection.GetStringOrNull(row["ПаспортныеДанные"], string.Empty),
                        DBConnection.GetStringOrNull(row["Телефон"], string.Empty),
                        DBConnection.GetDateOnlyOrNull(row["ДатаНачала"], DateOnly.MinValue),
                        DBConnection.GetDateOnlyOrNull(row["ДатаОкончания"], DateOnly.MinValue)
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