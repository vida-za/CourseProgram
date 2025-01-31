using CourseProgram.Models;
using CourseProgram.Services.DBServices;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using static CourseProgram.Models.Constants;

namespace CourseProgram.Services.DataServices
{
    public class MachineDataService : BaseService<Machine>
    {
        public MachineDataService() : base(User.Username, User.Password) { }

        public async Task<IEnumerable<Machine>> GetDisMachinesAsync(bool forceRefresh = false)
        {
            using (var query = new Query(CommandTypes.SelectQuery, Machine.GetTable()))
            {
                try
                {
                    items.Clear();

                    query.AddFields(Machine.GetFieldNames());
                    query.WhereClause.IsNotNull("ДатаВремяСписания");

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
                    await LogManager.Instance.WriteLogAsync($"Error in {nameof(GetDisMachinesAsync)}: {ex.Message}");
                    throw;
                }
                finally
                {
                    query?.Dispose();
                }
            }
        }

        public async Task<IEnumerable<Machine>> GetRdyMachinesAsync()
        {
            using (var query = new Query(CommandTypes.SelectQuery, Machine.GetTable()))
            {
                try
                {
                    items.Clear();

                    query.AddFields(Machine.GetFieldNames());
                    query.WhereClause.IsNull("ДатаВремяСписания");

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
                    await LogManager.Instance.WriteLogAsync($"Error in {nameof(GetRdyMachinesAsync)}: {ex.Message}");
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
            items.Add(new Machine(GetInt(row["КодМашины"], 0),
                GetString(row["ТипМашины"], string.Empty),
                GetStringOrNull(row["ТипКузова"]),
                GetString(row["ТипЗагрузки"], string.Empty),
                GetFloat(row["Грузоподъёмность"], 0),
                GetFloatOrNull(row["Объём"]),
                GetBool(row["Гидроборт"], false),
                GetFloatOrNull(row["ДлинаКузова"]),
                GetFloatOrNull(row["ШиринаКузова"]),
                GetFloatOrNull(row["ВысотаКузова"]),
                GetString(row["Марка"], string.Empty),
                GetString(row["Название"], string.Empty),
                GetStringOrNull(row["ГосНомер"]),
                GetString(row["Состояние"], string.Empty),
                GetDateTime(row["ДатаВремяПоступления"], DateTime.MinValue),
                GetDateTimeOrNull(row["ДатаВремяСписания"]),
                null //TO DO
                ));
        }
    }
}