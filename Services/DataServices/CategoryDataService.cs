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
    public class CategoryDataService : BaseService<Category>
    {
        public CategoryDataService() 
        {
            cnn = new DBConnection(Server, Database, User.Username, User.Password, "Category load");
            temp = new Category();
        }

        public override async Task<bool> AddItemAsync(Category item)
        {
            query = $"Insert Into {temp.GetTable()} ({temp.GetSelectors()}) Values(@1, @2);";

            try
            {
                await cnn.OpenAsync();
                var res = await cnn.ExecParamAsync(query, item.ID, item.Name);
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

        public override async Task<Category> GetItemAsync(int id)
        {
            query = $"Select {temp.GetSelectors()} From {temp.GetTable()} Where {temp.GetSelectorID()} = @1;";

            try
            {
                await cnn.OpenAsync();
                DataRow row = await cnn.ExecParamAsync(query, id);

                foreach (Category ctg in items)
                {
                    if (ctg.ID == DBConnection.GetIntOrNull(row["КодКатегории"], 0)) return ctg;
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

        public async Task<string> GetStringByDriverAsync(int id)
        {
            query = "Select \"GetStringCategoriesByDriver\"(@1);";

            try
            {
                await cnn.OpenAsync();
                DataRow row = cnn.GetDataTableParam(query, id);
                return DBConnection.GetStringOrNull(row["GetStringCategoriesByDriver"], string.Empty);
            }
            catch (Exception) { }
            finally
            {
                cnn.Close();
                query = string.Empty;
            }
            return string.Empty;
        }

        public override async Task<IEnumerable<Category>> GetItemsAsync(bool forceRefresh = false)
        {
            query = $"Select {temp.GetSelectors()} From {temp.GetTable()};";

            try
            {
                items.Clear();
                await cnn.OpenAsync();
                foreach (DataRow row in cnn.GetDataTable(query))
                {
                    items.Add(new Category(
                        DBConnection.GetIntOrNull(row["КодКатегории"], 0),
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

        public async Task<IEnumerable<Category>> GetItemsByDriverAsync(int id)
        {
            query = $"Select {temp.GetSelectors()} From \"GetCategoriesByDriver\"(@1);";

            try
            {
                items.Clear();
                await cnn.OpenAsync();
                foreach (DataRow row in cnn.GetDataTableParam(query, id))
                {
                    items.Add(new Category(
                        DBConnection.GetIntOrNull(row["КодКатегории"], 0),
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

        public override async Task<IEnumerable<Category>> GetFullTableAsync(bool forceRefresh = false)
        {
            query = $"Select {temp.GetSelectors()} From {temp.GetTable()};";

            try
            {
                items.Clear();
                await cnn.OpenAsync();
                foreach (DataRow row in cnn.GetDataTable(query))
                {
                    items.Add(new Category(
                        DBConnection.GetIntOrNull(row["КодКатегории"], 0),
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
    }
}