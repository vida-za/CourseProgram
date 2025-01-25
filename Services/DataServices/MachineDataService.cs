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

        public override void CreateElement(DataRow row)
        {
            items.Add(new Machine(GetIntOrNull(row["КодМашины"], 0),
                GetStringOrNull(row["ТипМашины"], string.Empty),
                GetStringOrNull(row["ТипКузова"], string.Empty),
                GetStringOrNull(row["ТипЗагрузки"], string.Empty),
                GetFloatOrNull(row["Грузоподъёмность"], 0),
                GetFloatOrNull(row["Объём"], 0),
                GetStringOrNull(row["Гидроборт"], string.Empty) == "Да",
                GetFloatOrNull(row["ДлинаКузова"], 0),
                GetFloatOrNull(row["ШиринаКузова"], 0),
                GetFloatOrNull(row["ВысотаКузова"], 0),
                GetStringOrNull(row["Марка"], string.Empty),
                GetStringOrNull(row["Название"], string.Empty),
                GetStringOrNull(row["ГосНомер"], string.Empty),
                GetStringOrNull(row["Состояние"], string.Empty),
                GetDateTimeOrNull(row["ДатаВремяПоступления"], DateTime.MinValue),
                GetDateTimeOrNull(row["ДатаВремяСписания"], DateTime.MinValue),
                string.Empty //TO DO
                ));
        }
    }
}