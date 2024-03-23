using CourseProgram.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using static CourseProgram.Models.Constants;

namespace CourseProgram.Services.DataServices
{
    public class DriverCategoriesDataService : BaseService<DriverCategories>, IDataService<DriverCategories>
    {
        public DriverCategoriesDataService() 
        {
            cnn = new DBConnection(Server, Database, User.Username, User.Password, "CategoriesDrv load");
        }

        public async Task<bool> AddItemAsync(DriverCategories item)
        {
            query = "Insert Into " + DriverCategories.GetTable() + "(" + DriverCategories.GetSelectors() + ") Values(@1, @2);";
            try
            {
                await cnn.OpenAsync();
                var res = await cnn.ExecParamAsync(query, item.DriverID, item.CategoryID);
                if (res.HasAnswer) items.Add(item);
            }
            catch (Exception)
            {

            }
            finally
            {
                cnn.Close();
                query = string.Empty;
            }
            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(int id)
        {
            query = "Delete From " + DriverCategories.GetTable() + " Where \"КодВодителя\" = @1;";
            try
            {
                await cnn.OpenAsync();
                var res = await cnn.ExecParamAsync(query, id);
                if (res.HasAnswer)
                    for (int i = 0; i < items.Count; i++)
                    {
                        var item = items[i];
                        if (item.DriverID == id)
                        {
                            items.RemoveAt(i);
                        }
                    }
            }
            catch (Exception)
            {

            }
            finally
            {
                cnn.Close();
                query = string.Empty;
            }
            return await Task.FromResult(true);
        }

        [Obsolete("Empty")]
        public async Task<int> FindMaxEmptyID() => await Task.FromResult(0); //

        [Obsolete("Empty")]
        public Task<DriverCategories> GetItemAsync(int id) => Task.FromResult(new DriverCategories());

        public async Task<IEnumerable<DriverCategories>> GetItemsAsync(bool forceRefresh = false)
        {
            query = "Select " + DriverCategories.GetSelectors() + " From " + DriverCategories.GetTable() + ";"; 

            try
            {
                items.Clear();
                await cnn.OpenAsync();
                foreach (DataRow row in cnn.GetDataTable(query))
                {
                    items.Add(new DriverCategories(
                        DBConnection.GetIntOrNull(row["КодВодителя"], 0),
                        DBConnection.GetIntOrNull(row["КодКатегории"], 0)
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

        [Obsolete("Empty")]
        public Task<bool> UpdateItemAsync(DriverCategories item) => Task.FromResult(false);
    }
}