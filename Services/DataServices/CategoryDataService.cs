using CourseProgram.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static CourseProgram.Models.Constants;

namespace CourseProgram.Services.DataServices
{
    public class CategoryDataService : BaseService<Category>, IDataService<Category>
    {
        public CategoryDataService() 
        {
            cnn = new DBConnection(Server, Database, User.Username, User.Password, "Category load");
        }

        public int FindMaxID()
        {
            Category category = items.OrderByDescending(x => x.ID).FirstOrDefault();
            return category != null ? category.ID : 0;
        }

        public async Task<bool> AddItemAsync(Category item)
        {
            query = "Insert Into " + Category.GetTable() + "(" + Category.GetSelectors() + ") Values(@1);";

            try
            {
                await cnn.OpenAsync();
                var res = await cnn.ExecParamAsync(query, item.Name);
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
            query = "Delete From " + Category.GetTable() + " Where \"КодКатегории\" = @1;";

            try
            {
                await cnn.OpenAsync();
                var res = await cnn.ExecParamAsync(query, id);
                if (res.HasAnswer)
                    foreach (Category ctg in items)
                    {
                        if (ctg.ID == id)
                            items.Remove(ctg);
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

        public async Task<Category> GetItemAsync(int id)
        {
            query = "Select " + Category.GetSelectorID() + ", " + Category.GetSelectors() + " From " + Category.GetTable() + " Where " + Category.GetSelectorID() + " = @1;";

            try
            {
                await cnn.OpenAsync();
                DataRow row = await cnn.ExecParamAsync(query, id);

                foreach (Category ctg in items)
                {
                    if (ctg.ID == DBConnection.GetIntOrNull(row[Category.GetSelectorID()], 0)) return ctg;
                }
            }
            catch (Exception) { }
            finally
            {
                cnn.Close();
                query = string.Empty;
            }
            return new Category();
        }

        public async Task<IEnumerable<Category>> GetItemsAsync(bool forceRefresh = false)
        {
            query = "Select " + Category.GetSelectorID() + ", " + Category.GetSelectors() + " From " + Category.GetTable() + ";";

            try
            {
                items.Clear();
                await cnn.OpenAsync();
                foreach (DataRow row in cnn.GetDataTable(query))
                {
                    items.Add(new Category(
                        DBConnection.GetIntOrNull(row[Category.GetSelectorID()], 0),
                        DBConnection.GetStringOrNull(row["Наименование"], string.Empty)
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

        public async Task<bool> UpdateItemAsync(Category item)
        {
            query = "Update " + Category.GetTable() + " Set";
            var oldItem = items.FirstOrDefault(c => c == item);
            if (oldItem == null) return await Task.FromResult(false);

            var values = new List<object>();
            var propertiesToUpdate = new Dictionary<string, object>();
            int countModify = 0;

            PropertyInfo[] properties = typeof(Category).GetProperties();
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