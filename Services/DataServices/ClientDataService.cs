using CourseProgram.Models;
using CourseProgram.Services.DBServices;
using CourseProgram.Stores;
using System;
using System.Data;
using System.Threading.Tasks;
using static CourseProgram.Models.Constants;

namespace CourseProgram.Services.DataServices
{
    public class ClientDataService : BaseService<Client>
    {
        public ClientDataService(DataStore dataStore) : base(User.Username, User.Password, dataStore) { }

        public async Task<bool> CheckCanDelete(int id)
        {
            try
            {
                DataTable data;

                using (var query = new Query(CommandTypes.SelectQuery, "Заявка"))
                {
                    query.AddFields("Count(1)");
                    query.WhereClause.Equals(Client.GetSelectorID(), id.ToString());

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

        public override Task<Client> CreateElement(DataRow row)
        {
            return Task.FromResult(new Client(
                        GetInt(row["КодЗаказчика"], 0),
                        GetString(row["Название"], string.Empty),
                        GetString(row["ТипЗаказчика"], string.Empty),
                        GetString(row["ИНН"], string.Empty),
                        GetString(row["КПП"], string.Empty),
                        GetString(row["ОГРН"], string.Empty),
                        GetString(row["Телефон"], string.Empty),
                        GetString(row["РасчётныйСчёт"], string.Empty),
                        GetStringOrNull(row["БИК"]),
                        GetStringOrNull(row["КорреспондентскийСчёт"]),
                        GetStringOrNull(row["Банк"])
                        ));
        }
    }
}