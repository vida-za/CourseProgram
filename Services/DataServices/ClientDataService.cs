using CourseProgram.Models;
using CourseProgram.Services.DBServices;
using System;
using System.Data;
using System.Threading.Tasks;
using static CourseProgram.Models.Constants;

namespace CourseProgram.Services.DataServices
{
    public class ClientDataService : BaseService<Client>
    {
        public ClientDataService() : base(User.Username, User.Password) { }

        public async Task<bool> CheckCanDelete(int id)
        {
            try
            {
                DataTable data;

                using (var query = new Query(CommandTypes.SelectQuery, ""))
                {
                    query.AddFields("Count(1)");
                    query.AddParameter(Client.GetSelectorID(), id.ToString());

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

        public override void CreateElement(DataRow row)
        {
            items.Add(new Client(
                        GetIntOrNull(row["КодЗаказчика"], 0),
                        GetStringOrNull(row["Название"], string.Empty),
                        GetStringOrNull(row["ТипЗаказчика"], string.Empty),
                        GetStringOrNull(row["ИНН"], string.Empty),
                        GetStringOrNull(row["КПП"], string.Empty),
                        GetStringOrNull(row["ОГРН"], string.Empty),
                        GetStringOrNull(row["Телефон"], string.Empty),
                        GetStringOrNull(row["РасчётныйСчёт"], string.Empty),
                        GetStringOrNull(row["БИК"], string.Empty),
                        GetStringOrNull(row["КорреспондентскийСчёт"], string.Empty),
                        GetStringOrNull(row["Банк"], string.Empty),
                        GetStringOrNull(row["КонтактЗагрузки"], string.Empty),
                        GetStringOrNull(row["КонтактВыгрузки"], string.Empty)
                        ));
        }
    }
}