using CourseProgram.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using static CourseProgram.Models.Constants;

namespace CourseProgram.Services.DataServices
{
    public class DriverCategoriesDataService : BaseService<DriverCategories>
    {
        public DriverCategoriesDataService() 
        {
            cnn = new DBConnection(Server, Database, User.Username, User.Password, "CategoriesDrv load");
            temp = new DriverCategories();
        }

        public override async Task<bool> AddItemAsync(DriverCategories item)
        {
            query = "Insert Into " + item.GetTable() + "(" + item.GetSelectors() + ") Values(@1, @2);";
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

        public override async Task<IEnumerable<DriverCategories>> GetFullTableAsync(bool forceRefresh = false)
        {
            query = "Select " + temp.GetSelectors() + " From " + temp.GetTable() + ";";

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

        public override async Task<DriverCategories> GetItemAsync(int id)
        {
            return await Task.FromResult(temp);
        }

        public override async Task<IEnumerable<DriverCategories>> GetItemsAsync(bool forceRefresh = false)
        {
            query = "Select " + temp.GetSelectors() + " From " + temp.GetTable() + ";"; 

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

        public async Task<IEnumerable<DriverCategories>> GetItemsByDriverAsync(int id)
        {
            query = $"Select {temp.GetSelectors()} From {temp.GetTable()} Where \"КодВодителя\" = @1;";

            try
            {
                items.Clear();
                await cnn.OpenAsync();
                foreach (DataRow row in cnn.GetDataTableParam(query, id))
                {
                    items.Add(new DriverCategories(
                        id,
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
    }
}