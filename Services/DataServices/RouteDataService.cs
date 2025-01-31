using CourseProgram.Models;
using CourseProgram.Services.DBServices;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using static CourseProgram.Models.Constants;

namespace CourseProgram.Services.DataServices
{
    public class RouteDataService : BaseService<Route>
    {
        public RouteDataService() : base(User.Username, User.Password) { }

        public override void CreateElement(DataRow row)
        {
            items.Add(new Route(GetInt(row["КодМаршрута"], 0),
                GetIntOrNull(row["КодМашины"]),
                GetIntOrNull(row["КодВодителя"]),
                GetString(row["Тип"], string.Empty),
                GetFloatOrNull(row["Расстояние"]),
                GetString(row["Статус"], string.Empty),
                GetDateTimeOrNull(row["ВремяВыполнения"]),
                GetIntOrNull(row["КодАдресаНачала"]),
                GetIntOrNull(row["КодАдресаОкончания"])
                ));
        }

        public async Task<IEnumerable<Route>> GetItemsByDriverAsync(int id)
        {
            using (var query = new Query(CommandTypes.SelectQuery, Route.GetTable()))
            {
                try
                {
                    items.Clear();

                    query.AddFields(Route.GetFieldNames());
                    query.WhereClause.Equals("КодВодителя", id.ToString());
                    query.WhereClause.NotEquals("Статус", "\'Отменен\'");

                    DataTable data;

                    await using (var con = new Connection(connection))
                    {
                        await con.OpenAsync();
                        data = await con.ExecuteQueryAsync<DataTable>(query);
                    }

                    if (data != null)
                    {
                        foreach (DataRow row in data.Rows)
                        {
                            CreateElement(row);
                        }
                    }
                    return await Task.FromResult(items);
                }
                catch (Exception ex)
                {
                    await LogManager.Instance.WriteLogAsync($"Error in {nameof(GetItemsByDriverAsync)}: {ex.Message}");
                    throw;
                }
                finally
                {
                    query?.Dispose();
                }
            }
        }

        public async Task<IEnumerable<Route>> GetItemsByMachineAsync(int id)
        {
            using (var query = new Query(CommandTypes.SelectQuery, Route.GetTable()))
            {
                try
                {
                    items.Clear();

                    query.AddFields(Route.GetFieldNames());
                    query.WhereClause.Equals("КодМашины", id.ToString());
                    query.WhereClause.NotEquals("Статус", "\'Отменен\'");

                    DataTable data;

                    await using (var con = new Connection(connection))
                    {
                        await con.OpenAsync();
                        data = await con.ExecuteQueryAsync<DataTable>(query);
                    }

                    if (data != null)
                    {
                        foreach (DataRow row in data.Rows)
                        {
                            CreateElement(row);
                        }
                    }
                    return await Task.FromResult(items);
                }
                catch (Exception ex)
                {
                    await LogManager.Instance.WriteLogAsync($"Error in {nameof(GetItemsByDriverAsync)}: {ex.Message}");
                    throw;
                }
                finally
                {
                    query?.Dispose();
                }
            }
        }
    }
}