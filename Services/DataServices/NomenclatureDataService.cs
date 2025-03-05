using CourseProgram.Models;
using CourseProgram.Services.DBServices;
using CourseProgram.Stores;
using System;
using System.Data;
using System.Threading.Tasks;
using static CourseProgram.Models.Constants;

namespace CourseProgram.Services.DataServices
{
    public class NomenclatureDataService : BaseService<Nomenclature>
    {
        public NomenclatureDataService(DataStore dataStore) : base(User.Username, User.Password, dataStore) { }

        public async Task<bool> CheckCanDelete(int id)
        {
            try
            {
                DataTable data;

                using (var query = new Query(CommandTypes.SelectQuery, "Груз"))
                {
                    query.AddFields("Count(1)");
                    query.WhereClause.Equals("КодНоменклатуры", id.ToString());

                    await using (var con = new Connection(connection))
                    {
                        await con.OpenAsync();
                        data = await con.ExecuteQueryAsync<DataTable>(query);
                    }
                }

                if (data?.Rows.Count > 0)
                {
                    int count = Convert.ToInt32(data.Rows[0][0]);
                    return count == 0;
                }

                return true;
            }
            catch (Exception ex)
            {
                await LogManager.Instance.WriteLogAsync($"Error in {nameof(CheckCanDelete)}: {ex.Message}");
                throw;
            }
        }

        public override Task<Nomenclature> CreateElement(DataRow row)
        {
            return Task.FromResult(new Nomenclature(GetInt(row["КодНоменклатуры"], 0),
                GetString(row["Наименование"], string.Empty),
                GetString(row["Тип"], string.Empty),
                GetString(row["Категория"], string.Empty),
                GetFloatOrNull(row["Длина"]),
                GetFloatOrNull(row["Ширина"]),
                GetFloatOrNull(row["Высота"]),
                GetFloatOrNull(row["Вес"]),
                GetString(row["ЕдиницаИзмерения"], string.Empty),
                GetStringOrNull(row["Упаковка"]),
                GetStringOrNull(row["ТребованияКТемпературе"]),
                GetStringOrNull(row["Опасность"])));
        }
    }
}