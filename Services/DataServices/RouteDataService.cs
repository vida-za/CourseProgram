using CourseProgram.Models;
using CourseProgram.Services.DBServices;
using CourseProgram.Stores;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using static CourseProgram.Models.Constants;

namespace CourseProgram.Services.DataServices
{
    public class RouteDataService : BaseService<Route>
    {
        public RouteDataService(DataStore dataStore) : base(User.Username, User.Password, dataStore) { }

        public override Task<Route> CreateElement(DataRow row)
        {
            return Task.FromResult(new Route(GetInt(row["КодМаршрута"], 0),
                GetIntOrNull(row["КодМашины"]),
                GetIntOrNull(row["КодВодителя"]),
                GetString(row["Тип"], string.Empty),
                GetString(row["Статус"], string.Empty),
                GetDateTimeOrNull(row["ВремяВыполнения"]),
                GetIntOrNull(row["КодАдресаНачала"]),
                GetIntOrNull(row["КодАдресаОкончания"])
                ));
        }

        public async Task<bool> AddCatchWithOrder(int orderID, int routeID)
        {
            using (var query = new Query(CommandTypes.InsertQuery, "Маршруты_заказа"))
            {
                try
                {
                    query.AddParameter("КодЗаказа", orderID);
                    query.AddParameter("КодМаршрута", routeID);

                    await using (var con = new Connection(connection))
                    {
                        await con.OpenAsync();
                        int result = await con.ExecuteQueryAsync<int>(query);
                        return result > 0;
                    }
                }
                catch (Exception ex)
                {
                    await LogManager.Instance.WriteLogAsync($"Error in {nameof(AddCatchWithOrder)}: {ex.Message}");
                    throw;
                }
            }
        }

        public async Task<bool> StartRouteAsync(int routeID)
        {
            using (var query = new Query(CommandTypes.ScalarFunction, "UpdateRouteStatus"))
            {
                try
                {
                    query.AddParameter("routeID", routeID);

                    int result;
                    await using (var con = new Connection(connection))
                    {
                        await con.OpenAsync();
                        result = await con.ExecuteQueryAsync<int>(query);
                    }

                    return result > 0;
                }
                catch (Exception ex)
                {
                    await LogManager.Instance.WriteLogAsync($"Error in {nameof(StartRouteAsync)}: {ex.Message}");
                    throw;
                }
                finally
                {
                    query?.Dispose();
                }
            }
        }

        public async Task<IEnumerable<Route>> GetRoutesByOrderAsync(int OrderID)
        {
            using (var query = new Query(CommandTypes.TableFunction, "GetRoutesByOrder"))
            {
                try
                {
                    var tempItems = new List<Route>();

                    query.AddFields(Route.GetFieldNames());
                    query.AddParameter("OrderID", OrderID);

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
                            tempItems.Add(await CreateElement(row));
                        }
                    }

                    return await Task.FromResult(tempItems);
                }
                catch (Exception ex)
                {
                    await LogManager.Instance.WriteLogAsync($"Error in {nameof(GetRoutesByOrderAsync)}: {ex.Message}");
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