using CourseProgram.Models;
using CourseProgram.Services.DBServices;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using static CourseProgram.Models.Constants;

namespace CourseProgram.Services.DataServices
{
    public class CargoDataService : BaseService<Cargo>
    {
        public CargoDataService() : base(User.Username, User.Password) { }

        public async Task<IEnumerable<Cargo>> GetCargosByBudAsync(int BudID)
        {
            using (var query = new Query(CommandTypes.SelectQuery, Cargo.GetTable()))
            {
                try
                {
                    items.Clear();

                    query.AddFields(Cargo.GetFieldNames());
                    query.WhereClause.Equals("КодЗаявки", BudID.ToString());

                    DataTable data;

                    await using (var con = new Connection(connection))
                    {
                        await con.OpenAsync();
                        data = await con.ExecuteQueryAsync<DataTable>(query);
                    }

                    if (data != null)
                        foreach (DataRow row in data.Rows)
                        {
                            CreateElement(row);
                        }

                    return await Task.FromResult(items);
                }
                catch (Exception ex)
                {
                    await LogManager.Instance.WriteLogAsync($"Error in {nameof(GetCargosByBudAsync)}: {ex.Message}");
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
            items.Add(new Cargo(GetInt(row["КодГруза"], 0),
                GetInt(row["КодЗаявки"], 0),
                GetInt(row["КодНоменклатуры"], 0),
                GetFloatOrNull(row["Объём"]),
                GetFloat(row["Вес"], 0),
                GetInt(row["Количество"], 0)
                ));
        }
    }
}