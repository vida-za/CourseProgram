using CourseProgram.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using static CourseProgram.Models.Constants;

namespace CourseProgram.Services.DataServices
{
    public class MachineDataService : BaseService<Machine>
    {
        public MachineDataService() 
        {
            cnn = new DBConnection(Server, Database, User.Username, User.Password, "MachineLoad");
            temp = new Machine();
        }

        public override async Task<bool> AddItemAsync(Machine item)
        {
            query = $"Insert Into {temp.GetTable()} ({temp.GetSelectors()}) Values(@1, @2, @3, @4, @5, @6, @7, @8, @9, @10, @11, @12, @13, @14, @15, @16);";

            try
            {
                await cnn.OpenAsync();
                var res = await cnn.ExecParamAsync(
                    query,
                    item.ID,
                    item.TypeMachine,
                    item.TypeBodywork,
                    item.TypeLoading,
                    item.LoadCapacity,
                    item.Volume,
                    item.HydroBoard,
                    item.LengthBodywork,
                    item.WidthBodywork,
                    item.HeightBodywork,
                    item.Stamp,
                    item.Name,
                    item.StateNumber,
                    item.Status,
                    item.TimeStart,
                    item.TimeEnd);
                if (res.HasAnswer) items.Add(item);
            }
            catch (Exception) { }
            finally
            {
                cnn.Close();
                query = string.Empty;
            }
            return await Task.FromResult(true);
        }

        public override async Task<bool> DeleteItemAsync(int id)
        {
            var current = await GetItemAsync(id);
            if (current == null) return false;
            if (current.TimeEnd != DateTime.MinValue && current.TimeEnd <= DateTime.Now) return false;

            query = $"Update {temp.GetTable()} Set \"ВремяОкончания\" = current_timestamp Where \"КодМашины\" = @1;";

            try
            {
                await cnn.OpenAsync();
                var res = await cnn.ExecParamAsync(query, id);
            }
            catch (Exception) { }
            finally
            {
                cnn.Close();
                query = string.Empty;
            }
            return await Task.FromResult(true);
        }

        public override async Task<Machine> GetItemAsync(int id)
        {
            query = $"Select {temp.GetSelectors()} From {temp.GetTable()} Where \"КодМашины\" = @1;";

            try
            {
                await cnn.OpenAsync();
                DataRow row = cnn.GetDataTableParam(query, id);

                foreach (Machine mach in items)
                {
                    if (mach.ID == DBConnection.GetIntOrNull(row["КодМашины"], 0)) return mach;
                }
            }
            catch (Exception) { }
            finally
            {
                cnn.Close();
                query = string.Empty;
            }
            return temp;
        }

        public override async Task<IEnumerable<Machine>> GetItemsAsync(bool forceRefresh = false)
        {
            query = $"Select {temp.GetSelectors()}, \"GetTownNowByMachine\"(\"КодМашины\") as \"Город\" From {temp.GetTable()} Where \"ВремяОкончания\" is Null;";

            try
            {
                items.Clear();
                await cnn.OpenAsync();
                foreach (DataRow row in cnn.GetDataTable(query))
                {
                    items.Add(new Machine(
                        DBConnection.GetIntOrNull(row["КодМашины"], 0),
                        DBConnection.GetStringOrNull(row["ТипМашины"], string.Empty),
                        DBConnection.GetStringOrNull(row["ТипКузова"], string.Empty),
                        DBConnection.GetStringOrNull(row["ТипЗагрузки"], string.Empty),
                        DBConnection.GetFloatOrNull(row["Грузоподъёмность"], 0),
                        DBConnection.GetFloatOrNull(row["Объём"], 0),
                        DBConnection.GetStringOrNull(row["Гидроборт"], string.Empty) == "Да",
                        DBConnection.GetFloatOrNull(row["ДлинаКузова"], 0),
                        DBConnection.GetFloatOrNull(row["ШиринаКузова"], 0),
                        DBConnection.GetFloatOrNull(row["ВысотаКузова"], 0),
                        DBConnection.GetStringOrNull(row["Марка"], string.Empty),
                        DBConnection.GetStringOrNull(row["Название"], string.Empty),
                        DBConnection.GetStringOrNull(row["ГосНомер"], string.Empty),
                        DBConnection.GetStringOrNull(row["Состояние"], string.Empty),
                        DBConnection.GetDateTimeOrNull(row["ВремяПоступления"], DateTime.MinValue),
                        DBConnection.GetDateTimeOrNull(row["ВремяОкончания"], DateTime.MinValue),
                        DBConnection.GetStringOrNull(row["Город"], string.Empty)
                    ));
                }
            }
            catch (Exception) { }
            finally
            {
                cnn.Close();
                query = string.Empty;
            }
            return await Task.FromResult(items);
        }

        public async Task<IEnumerable<Machine>> GetDisMachinesAsync(bool forceRefresh = false)
        {
            query = $"Select {temp.GetSelectors()} From {temp.GetTable()} Where \"ВремяОкончания\" is not Null;";

            try
            {
                items.Clear();
                await cnn.OpenAsync();
                foreach (DataRow row in cnn.GetDataTable(query))
                {
                    items.Add(new Machine(
                        DBConnection.GetIntOrNull(row["КодМашины"], 0),
                        DBConnection.GetStringOrNull(row["ТипМашины"], string.Empty),
                        DBConnection.GetStringOrNull(row["ТипКузова"], string.Empty),
                        DBConnection.GetStringOrNull(row["ТипЗагрузки"], string.Empty),
                        DBConnection.GetFloatOrNull(row["Грузоподъёмность"], 0),
                        DBConnection.GetFloatOrNull(row["Объём"], 0),
                        DBConnection.GetStringOrNull(row["Гидроборт"], string.Empty) == "Да",
                        DBConnection.GetFloatOrNull(row["ДлинаКузова"], 0),
                        DBConnection.GetFloatOrNull(row["ШиринаКузова"], 0),
                        DBConnection.GetFloatOrNull(row["ВысотаКузова"], 0),
                        DBConnection.GetStringOrNull(row["Марка"], string.Empty),
                        DBConnection.GetStringOrNull(row["Название"], string.Empty),
                        DBConnection.GetStringOrNull(row["ГосНомер"], string.Empty),
                        DBConnection.GetStringOrNull(row["Состояние"], string.Empty),
                        DBConnection.GetDateTimeOrNull(row["ВремяПоступления"], DateTime.MinValue),
                        DBConnection.GetDateTimeOrNull(row["ВремяОкончания"], DateTime.MinValue),
                        string.Empty
                    ));
                }
            }
            catch (Exception) { }
            finally
            {
                cnn.Close();
                query = string.Empty;
            }
            return await Task.FromResult(items);
        }

        public override async Task<IEnumerable<Machine>> GetFullTableAsync(bool forceRefresh = false)
        {
            query = $"Select {temp.GetSelectors()} From {temp.GetTable()};";

            try
            {
                items.Clear();
                await cnn.OpenAsync();
                foreach (DataRow row in cnn.GetDataTable(query))
                {
                    items.Add(new Machine(
                        DBConnection.GetIntOrNull(row["КодМашины"], 0),
                        DBConnection.GetStringOrNull(row["ТипМашины"], string.Empty),
                        DBConnection.GetStringOrNull(row["ТипКузова"], string.Empty),
                        DBConnection.GetStringOrNull(row["ТипЗагрузки"], string.Empty),
                        DBConnection.GetFloatOrNull(row["Грузоподъёмность"], 0),
                        DBConnection.GetFloatOrNull(row["Объём"], 0),
                        DBConnection.GetIntOrNull(row["Гидроборт"], 0) == 1,
                        DBConnection.GetFloatOrNull(row["ДлинаКузова"], 0),
                        DBConnection.GetFloatOrNull(row["ШиринаКузова"], 0),
                        DBConnection.GetFloatOrNull(row["ВысотаКузова"], 0),
                        DBConnection.GetStringOrNull(row["Марка"], string.Empty),
                        DBConnection.GetStringOrNull(row["Название"], string.Empty),
                        DBConnection.GetStringOrNull(row["ГосНомер"], string.Empty),
                        DBConnection.GetStringOrNull(row["Состояние"], string.Empty),
                        DBConnection.GetDateTimeOrNull(row["ВремяПоступления"], DateTime.MinValue),
                        DBConnection.GetDateTimeOrNull(row["ВремяОкончания"], DateTime.MinValue),
                        string.Empty
                        ));
                }
            }
            catch (Exception) { }
            finally
            {
                cnn.Close();
                query = string.Empty;
            }
            return await Task.FromResult(items);
        }
    }
}