using CourseProgram.Models;
using CourseProgram.Services.DBServices;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using static CourseProgram.Models.Constants;

namespace CourseProgram.Services.DataServices
{
    public class DriverCategoriesDataService : BaseService<DriverCategories>
    {
        public DriverCategoriesDataService() : base(User.Username, User.Password) { }

        public override async Task<int> AddItemAsync(DriverCategories item)
        {
            using (var query = new Query(CommandTypes.InsertQuery, DriverCategories.GetTable()))
            {
                FillInsertParams(query, item);

                try
                {
                    await using (var con = new Connection(connection))
                    {
                        await con.OpenAsync();
                        int res = await con.ExecuteQueryAsync<int>(query);
                        return res;
                    }
                }
                catch (Exception ex)
                {
                    await LogManager.Instance.WriteLogAsync($"Error in {nameof(AddItemAsync)}: {ex.Message}");
                    throw;
                }
                finally
                {
                    query?.Dispose();
                }
            }
        }

        public async Task<IEnumerable<DriverCategories>> GetListForDriverAsync(int id)
        {
            using (var query = new Query(CommandTypes.SelectQuery, DriverCategories.GetTable()))
            {
                try
                {
                    items.Clear();

                    query.AddFields(DriverCategories.GetFieldNames());
                    query.WhereClause.Equals("КодВодителя", id.ToString());

                    DataTable data;

                    await using (var con = new Connection(connection))
                    {
                        await con.OpenAsync();
                        data = await con.ExecuteQueryAsync<DataTable>(query);
                    }

                    if (data != null)
                    {
                        foreach (DataRow row in data.Rows)
                            CreateElement(row);
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
                    query?.Dispose();
                }
            }
        }

        public override void CreateElement(DataRow row)
        {
            items.Add(new DriverCategories(
                GetInt(row["КодВодителя"], 0),
                GetInt(row["КодКатегории"], 0)));
        }
    }
}