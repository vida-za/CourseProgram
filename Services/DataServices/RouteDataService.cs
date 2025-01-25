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
            items.Add(new Route(GetIntOrNull(row["КодМаршрута"], 0),
                GetIntOrNull(row["КодМашины"], 0),
                GetIntOrNull(row["КодВодителя"], 0),
                GetStringOrNull(row["Тип"], string.Empty),
                GetFloatOrNull(row["Расстояние"], 0),
                GetStringOrNull(row["Статус"], string.Empty),
                GetDateTimeOrNull(row["ВремяВыполнения"], DateTime.MinValue),
                GetIntOrNull(row["КодАдресаНачала"], 0),
                GetIntOrNull(row["КодАдресаОкончания"], 0)
                ));
        }

        public async Task<IEnumerable<Route>> GetItemsByDriverAsync(int id)
        {
            using (var query = new Query(CommandTypes.SelectQuery, Route.GetTable()))
            {
                try
                {
                    items.Clear();

                    query.AddFields(Worker.GetFieldNames());
                    query.WhereClause.Equals("КодВодителя", id.ToString());
                    query.WhereClause.NotEquals("Статус", "Отменен");

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

                    query.AddFields(Worker.GetFieldNames());
                    query.WhereClause.Equals("КодМашины", id.ToString());
                    query.WhereClause.NotEquals("Статус", "Отменен");

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