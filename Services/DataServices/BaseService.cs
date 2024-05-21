using CourseProgram.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace CourseProgram.Services.DataServices
{
    public abstract class BaseService<T> : IDataService<T> where T : IModel, IEquatable<T>
    {
        protected DBConnection cnn;
        protected List<T> items = new();
        protected string query = string.Empty;
        protected T temp;

        public async Task<int> FindMaxEmptyID()
        {
            int i = 0;
            var temp = await GetFullTableAsync();
            for (i = 1; i <= temp.Count(); ++i)
            {
                if (!temp.Any(d => d.ID == i))
                    return await Task.FromResult(i);
            }
            return await Task.FromResult(i);
        }

        protected void CreatingUpdateQuery(string column, ref int index, ref List<object> list, object value)
        {
            if (index > 0) query += ",";
            index++;
            query += $"\"{column}|\" = @{index}";
            list.Add(value);
        }

        public virtual async Task<bool> DeleteItemAsync(int id)
        {
            var current = await GetItemAsync(id);
            if (current == null) return false;

            query = $"Call {temp.GetProcedureDelete()}(@1);";

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

        public virtual async Task<bool> UpdateItemAsync(T item)
        {
            query = $"Update {temp.GetTable()} Set";
            var oldItem = items.FirstOrDefault(d => d.Equals(item));
            if (oldItem == null) return await Task.FromResult(false);

            var values = new List<object>();
            var propertiesToUpdate = new Dictionary<string, object>();
            int countModify = 0;

            PropertyInfo[] properties = typeof(T).GetProperties();
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

        public abstract Task<bool> AddItemAsync(T item);

        public abstract Task<IEnumerable<T>> GetFullTableAsync(bool forceRefresh = false);

        public abstract Task<T> GetItemAsync(int id);

        public abstract Task<IEnumerable<T>> GetItemsAsync(bool forceRefresh = false);

    }
}