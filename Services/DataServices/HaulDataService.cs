using CourseProgram.Models;
using CourseProgram.Services.DBServices;
using System;
using System.Data;
using System.Threading.Tasks;
using static CourseProgram.Models.Constants;

namespace CourseProgram.Services.DataServices
{
    public class HaulDataService : BaseService<Haul>
    {
        public HaulDataService() : base(User.Username, User.Password) { }

        public async Task<Haul?> GetCurrentHaul()
        {
            using (var query = new Query(CommandTypes.SelectQuery, Haul.GetTable()))
            {
                try
                {
                    query.AddFields(Haul.GetFieldNames());
                    query.WhereClause.IsNull("ДатаОкончания");

                    DataTable data;
                    DataRow dataRow;

                    await using (var con = new Connection(connection))
                    {
                        await con.OpenAsync();
                        data = await con.ExecuteQueryAsync<DataTable>(query);
                    }

                    if (data?.Rows.Count == 1)
                    {
                        dataRow = data.Rows[0];
                        int targetID = GetInt(dataRow[Haul.GetSelectorID()], 0);

                        foreach (Haul item in items)
                        {
                            if (item.ID == targetID)
                                return item;
                        }

                        if (targetID > 0)
                            items.Add(await CreateElement(dataRow));

                        foreach (Haul item in items)
                        {
                            if (item.ID == targetID)
                                return item;
                        }

                        throw new Exception($"It`s impossible..");
                    }
                    else if (data?.Rows.Count == 0)
                        return null;
                    else
                        throw new Exception($"Error on query or db");
                }
                catch (Exception ex)
                {
                    await LogManager.Instance.WriteLogAsync($"Error in {nameof(GetCurrentHaul)}: {ex.Message}");
                    throw;
                }
                finally
                {
                    query?.Dispose();
                }
            }
        }

        public override Task<Haul> CreateElement(DataRow row)
        {
            return Task.FromResult(new Haul(GetInt(row["КодРейса"], 0),
                GetDateOnly(row["ДатаНачала"], DateOnly.MinValue),
                GetDateOnlyOrNull(row["ДатаОкончания"]),
                GetFloatOrNull(row["СуммарныйДоход"])
                ));
        }
    }
}