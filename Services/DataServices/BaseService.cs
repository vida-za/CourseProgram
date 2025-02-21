using CourseProgram.Models;
using CourseProgram.Services.DBServices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using static CourseProgram.Models.Constants;

namespace CourseProgram.Services.DataServices
{
    public abstract class BaseService<T> : IDataService<T> where T : IModel, IEquatable<T>
    {
        protected Connection connection;

        protected List<T> items = new List<T>();
        protected T temp;
        protected int FreeID;

        protected string Server = ConfigurationManager.AppSettings["Server"];
        protected string Database = ConfigurationManager.AppSettings["DatabaseName"];

        public BaseService() 
        {
            connection = new Connection(Server, Database, User.Username, User.Password);
        }

        public BaseService(string username, string password)
        {
            connection = new Connection(Server, Database, username, password);
        }

        public async Task FindMaxEmptyID()
        {
            int i = 0;
            FreeID = -1;
            var temp = await GetItemsAsync();
            for (i = 1; i <= temp.Count(); ++i)
            {
                if (!temp.Any(d => d.ID == i))
                    FreeID = i;
            }
            if (FreeID == -1)
                FreeID = i;

            //var temp = await GetItemsAsync();
            //FreeID = Enumerable.Range(1, temp.Count() + 1).Except(temp.Select(d => d.ID)).FirstOrDefault();
        }

        public virtual async Task<bool> DeleteItemAsync(int id)
        {
            using (var queryDel = new Query(CommandTypes.DeleteQuery, T.GetTable()))
            {
                try
                {
                    var current = await GetItemAsync(id);
                    if (current == null) return await Task.FromResult(false);

                    queryDel.WhereClause.Equals(T.GetSelectorID(), id.ToString());

                    await using (var con = new Connection(connection))
                    {
                        await con.OpenAsync();
                        int res = await con.ExecuteQueryAsync<int>(queryDel);
                        return res > 0;
                    }
                }
                catch (Exception ex)
                {
                    await LogManager.Instance.WriteLogAsync($"Error in {nameof(DeleteItemAsync)}: {ex.Message}");
                    throw;
                }
                finally
                {
                    queryDel?.Dispose();
                }
            }
        }

        public virtual async Task<bool> UpdateItemAsync(T item)
        {
            using (var queryUpd = new Query(CommandTypes.UpdateQuery, T.GetTable()))
            {

                try
                {
                    await GetItemsAsync();

                    var oldItem = items.FirstOrDefault(d => d.Equals(item));
                    if (oldItem == null)
                        return await Task.FromResult(false);

                    queryUpd.WhereClause.Equals(T.GetSelectorID(), item.ID.ToString());

                    int countModify = 0;

                    PropertyInfo[] properties = typeof(T).GetProperties();
                    foreach (PropertyInfo property in properties)
                    {
                        string columnName = property.GetCustomAttribute<DisplayNameAttribute>()?.DisplayName;
                        var oldValue = property.GetValue(oldItem);
                        if (property.PropertyType.BaseType.Name == "Enum")
                            oldValue = GetEnumDescription(property.GetValue(oldItem));

                        var newValue = property.GetValue(item);
                        if (property.PropertyType.BaseType.Name == "Enum")
                            newValue = GetEnumDescription(property.GetValue(item));

                        if (!Equals(oldValue, newValue) && columnName != null)
                        {
                            queryUpd.AddParameter(columnName, newValue);
                            countModify++;
                        }
                    }

                    if (countModify == 0) 
                        return await Task.FromResult(false);

                    await using (var con = new Connection(connection))
                    {
                        await con.OpenAsync();
                        int res = await con.ExecuteQueryAsync<int>(queryUpd);
                        return res > 0;
                    }
                }
                catch (Exception ex)
                {
                    await LogManager.Instance.WriteLogAsync($"Error in {nameof(UpdateItemAsync)}: {ex.Message}");
                    throw;
                }
                finally
                {
                    queryUpd?.Dispose();
                }
            }
        }

        public virtual async Task<int> AddItemAsync(T item)
        {
            using (var queryIns = new Query(CommandTypes.InsertQuery, T.GetTable()))
            {
                FillInsertParams(queryIns, item);

                try
                {
                    await using (var con = new Connection(connection))
                    {
                        await con.OpenAsync();
                        int res = await con.ExecuteQueryAsync<int>(queryIns);
                        if (res > 0)
                            return FreeID;
                        else 
                            return 0;
                    }
                }
                catch (Exception ex)
                {
                    await LogManager.Instance.WriteLogAsync($"Error in {nameof(AddItemAsync)}: {ex.Message}");
                    throw;
                }
                finally
                {
                    queryIns?.Dispose();
                }
            }
        }

        public virtual async Task<T> GetItemAsync(int id)
        {
            using (var queryOneSel = new Query(CommandTypes.SelectQuery, T.GetTable()))
            {

                try
                {
                    queryOneSel.AddFields(T.GetSelectorID());
                    queryOneSel.WhereClause.Equals(T.GetSelectorID(), id.ToString());

                    DataTable data;
                    DataRow dataRow;

                    await using (var con = new Connection(connection))
                    {
                        await con.OpenAsync();
                        data = await con.ExecuteQueryAsync<DataTable>(queryOneSel);
                    }

                    if (data?.Rows.Count == 1 && data != null)
                    {
                        dataRow = data.Rows[0];

                        foreach (T item in items)
                        {
                            if (item.ID == GetInt(dataRow[T.GetSelectorID()], 0))
                                return item;
                        }
                        throw new Exception($"Not found item in collection with id: {id}");
                    }
                    else
                        throw new Exception($"Not found item in DB with id: {id}");
                }
                catch (Exception ex)
                {
                    await LogManager.Instance.WriteLogAsync($"Error in {nameof(GetItemAsync)}: {ex.Message}");
                    throw;
                }
                finally
                {
                    queryOneSel?.Dispose();
                }
            }
        }

        public virtual async Task<IEnumerable<T>> GetItemsAsync()
        {
            using (var queryAllSel = new Query(CommandTypes.SelectQuery, T.GetTable()))
            {
                try
                {
                    items.Clear();

                    queryAllSel.AddFields(T.GetFieldNames());

                    DataTable data;

                    await using (var con = new Connection(connection))
                    {
                        await con.OpenAsync();
                        data = await con.ExecuteQueryAsync<DataTable>(queryAllSel);
                    }

                    if (data != null)
                    {
                        foreach (DataRow row in data.Rows)
                        {
                            items.Add(await CreateElement(row));
                        }
                    }

                    return await Task.FromResult(items);
                }
                catch (Exception ex)
                {
                    await LogManager.Instance.WriteLogAsync($"Error in {nameof(GetItemsAsync)}: {ex.Message}");
                    throw;
                }
                finally
                {
                    queryAllSel?.Dispose();
                }
            }
        }

        public abstract Task<T> CreateElement(DataRow row);

        public async void FillInsertParams(Query query, T item)
        {
            try
            {
                string SelectorID = T.GetSelectorID();

                if (SelectorID != "")
                {
                    query.AddParameter(SelectorID, FreeID);
                }

                PropertyInfo[] properties = typeof(T).GetProperties();
                foreach (var property in properties)
                {
                    string columnName = property.GetCustomAttribute<DisplayNameAttribute>()?.DisplayName;
                    var columnValue = property.GetValue(item);
                    if (property.PropertyType.BaseType.Name == "Enum")
                        columnValue = GetEnumDescription(property.GetValue(item));

                    if (columnName != null && columnName != SelectorID)
                        query.AddParameter(columnName, columnValue);
                }

                if (query.GetCountParametres() == 0)
                {
                    await LogManager.Instance.WriteLogAsync($"Error, method {nameof(FillInsertParams)} added zero parametres");
                    throw new InvalidOperationException();
                }
            }
            catch (Exception ex)
            {
                await LogManager.Instance.WriteLogAsync($"ERROR In {nameof(FillInsertParams)}: {ex.Message}");
                throw;
            }
        }
    }
}