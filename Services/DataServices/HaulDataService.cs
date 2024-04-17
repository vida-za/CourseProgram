using CourseProgram.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using static CourseProgram.Models.Constants;

namespace CourseProgram.Services.DataServices
{
    public class HaulDataService : BaseService<Haul>
    {
        public HaulDataService()
        {
            cnn = new DBConnection(Server, Database, User.Username, User.Password, "HaulLoad");
            temp = new Haul();
        }

        public override async Task<bool> AddItemAsync(Haul item)
        {
            query = $"Insert Into {temp.GetTable()} ({temp.GetSelectors()}) Values(@1, @2, @3, @4);";

            try
            {
                await cnn.OpenAsync();
                var res = await cnn.ExecParamAsync(query, item.ID, item.DateStart, item.DateEnd, item.SumIncome);
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

        public override async Task<IEnumerable<Haul>> GetFullTableAsync(bool forceRefresh = false)
        {
            query = $"Select {temp.GetSelectors()} From {temp.GetTable()};";

            try
            {
                items.Clear();
                await cnn.OpenAsync();
                foreach (DataRow row in cnn.GetDataTable(query))
                {
                    items.Add(new Haul(
                        DBConnection.GetIntOrNull(row["КодРейса"], 0),
                        DBConnection.GetDateOnlyOrNull(row["ДатаНачала"], DateOnly.MinValue),
                        DBConnection.GetDateOnlyOrNull(row["ДатаОкончания"], DateOnly.MinValue),
                        DBConnection.GetFloatOrNull(row["СуммарныйДоход"], 0)
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

        public override async Task<Haul> GetItemAsync(int id)
        {
            query = $"Select {temp.GetSelectors()} From {temp.GetTable()} Where \"КодРейса\" = @1;";

            try
            {
                await cnn.OpenAsync();
                DataRow row = cnn.GetDataTableParam(query, id);

                foreach (Haul haul in items)
                {
                    if (haul.ID == DBConnection.GetIntOrNull(row["КодРейса"], 0)) return haul;
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

        public override async Task<IEnumerable<Haul>> GetItemsAsync(bool forceRefresh = false)
        {
            query = $"Select {temp.GetSelectors()} From {temp.GetTable()};";

            try
            {
                items.Clear();
                await cnn.OpenAsync();
                foreach (DataRow row in cnn.GetDataTable(query))
                {
                    items.Add(new Haul(
                        DBConnection.GetIntOrNull(row["КодРейса"], 0),
                        DBConnection.GetDateOnlyOrNull(row["ДатаНачала"], DateOnly.MinValue),
                        DBConnection.GetDateOnlyOrNull(row["ДатаОкончания"], DateOnly.MinValue),
                        DBConnection.GetFloatOrNull(row["СуммарныйДоход"], 0)
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