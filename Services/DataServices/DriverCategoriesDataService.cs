using CourseProgram.Models;
using CourseProgram.Services.DBServices;
using CourseProgram.Stores;
using System;
using System.Data;
using System.Threading.Tasks;
using static CourseProgram.Models.Constants;

namespace CourseProgram.Services.DataServices
{
    public class DriverCategoriesDataService : BaseService<DriverCategories>
    {
        public DriverCategoriesDataService(DataStore dataStore) : base(User.Username, User.Password, dataStore) { }

        public override async Task<int> AddItemAsync(DriverCategories item)
        {
            using (var query = new Query(CommandTypes.InsertQuery, DriverCategories.GetTable()))
            {
                await FillInsertParams(query, item);

                try
                {
                    await using (var con = new Connection(connection))
                    {
                        await con.OpenAsync();
                        int res = await con.ExecuteQueryAsync<int>(query);
                        _dataStore.AddData(item);
                        if (res > 0)
                            return 1;
                        else
                            return 0;
                    }
                }
                catch (Exception ex)
                {
                    await LogManager.Instance.WriteLogAsync($"Error in {nameof(AddItemAsync)}: {ex.Message}");
                    throw;
                }
            }
        }

        public override Task<DriverCategories> CreateElement(DataRow row)
        {
            return Task.FromResult(new DriverCategories(
                GetInt(row["КодВодителя"], 0),
                GetInt(row["КодКатегории"], 0)));
        }
    }
}