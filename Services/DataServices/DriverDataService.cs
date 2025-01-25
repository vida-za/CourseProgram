using CourseProgram.Models;
using CourseProgram.Services.DBServices;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using static CourseProgram.Models.Constants;

namespace CourseProgram.Services.DataServices
{
    public class DriverDataService : BaseService<Driver>
    {
        public DriverDataService() : base(User.Username, User.Password) { }

        public override async Task<bool> AddItemAsync(Driver item)
        {
            using (var query = new Query(CommandTypes.InsertQuery, Driver.GetTable()))
            {
                FillInsertParams(query, item);

                try
                {
                    int res;
                    await using (var con = new Connection(connection))
                    {
                        await con.OpenAsync();
                        res = await con.ExecuteQueryAsync<int>(query);
                    }
                    if (res == 0)
                    {
                        await LogManager.Instance.WriteLogAsync($"ERROR Insert for {query.GetObjectName()}");
                        return await Task.FromResult(false);
                    }
                    else
                    {
                        foreach (Category c in item.GetListCategories())
                        {
                            using (var queryExt = new Query(CommandTypes.InsertQuery, "Категории_водителя"))
                            {
                                queryExt.AddParameter(Driver.GetSelectorID(), FreeID);
                                queryExt.AddParameter("КодКатегории", (int)c.EnumCategory);

                                await using (var con = new Connection(connection))
                                {
                                    await con.OpenAsync();
                                    res = await con.ExecuteQueryAsync<int>(queryExt);
                                }
                                if (res == 0)
                                {
                                    await LogManager.Instance.WriteLogAsync($"ERROR Insert for {queryExt.GetObjectName()}");
                                    return await Task.FromResult(false);
                                }
                            }
                        }
                        return await Task.FromResult(true);
                    }
                }
                catch (Exception) { }
                finally
                {
                    query?.Dispose();
                }
                return await Task.FromResult(true);
            }
        }

        public async Task<IEnumerable<Driver>> GetDisDriversAsync(bool forceRefresh = false)
        {
            using (var query = new Query(CommandTypes.SelectQuery, Driver.GetTable()))
            {
                try
                {
                    items.Clear();

                    query.AddFields(Driver.GetFieldNames());
                    query.WhereClause.IsNotNull("ДатаОкончания");

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
                    await LogManager.Instance.WriteLogAsync($"Error in {nameof(GetItemsAsync)}: {ex.Message}");
                    throw;
                }
                finally
                {
                    query?.Dispose();
                }
            }
        }

        public override async void CreateElement(DataRow row)
        {
            using (var query = new Query(CommandTypes.SelectQuery, "Категории_водителя"))
            {
                try
                {
                    Driver newItem = new Driver(
                        GetIntOrNull(row["КодВодителя"], 0),
                        GetStringOrNull(row["ФИО"], string.Empty),
                        GetDateOnlyOrNull(row["ДатаРождения"], DateOnly.MinValue),
                        GetStringOrNull(row["ПаспортныеДанные"], string.Empty),
                        GetStringOrNull(row["Телефон"], string.Empty),
                        GetDateOnlyOrNull(row["ДатаНачала"], DateOnly.MinValue),
                        GetDateOnlyOrNull(row["ДатаОкончания"], DateOnly.MinValue),
                        string.Empty);

                    query.AddFields(Driver.GetSelectorID(), "КодКатегории");
                    query.WhereClause.Equals(Driver.GetSelectorID(), GetIntOrNull(row["КодВодителя"], 0).ToString());

                    DataTable data;
                    await using (var con = new Connection(connection))
                    {
                        await con.OpenAsync();
                        data = await con.ExecuteQueryAsync<DataTable>(query);
                    }

                    if (data != null)
                    {

                        var listCat = new List<Category>();
                        foreach (DataRow temp in data.Rows)
                        {
                            listCat.Add(new Category((Categories)GetIntOrNull(temp["КодКатегории"], 0), true));
                        }

                        newItem.SetCategories(listCat.ToArray());
                    }

                    items.Add(newItem);
                }
                catch (Exception ex)
                {
                    await LogManager.Instance.WriteLogAsync($"Ошибка при созданиии объекта - {ex.Message}");
                }
                finally
                {
                    query?.Dispose();
                }
            }
        }
    }
}