using CourseProgram.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using static CourseProgram.Models.Constants;

namespace CourseProgram.Services.DataServices
{
    public class CargoDataService : BaseService<Cargo>
    {
        public CargoDataService()
        {
            cnn = new DBConnection(Server, Database, User.Username, User.Password, "CargoLoad");
            temp = new Cargo();
        }

        public override async Task<bool> AddItemAsync(Cargo item)
        {
            query = $"Insert Into {temp.GetTable()} ({temp.GetSelectors()}) Values(@1, @2, @3, @4, @5, @6, @7, @8, @9);";

            try
            {
                await cnn.OpenAsync();
                var res = await cnn.ExecParamAsync(query, item.ID, item.OrderID, item.Length, item.Width, item.Height, item.Type, item.Category, item.Weight, item.Count);
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

        public override async Task<IEnumerable<Cargo>> GetFullTableAsync(bool forceRefresh = false)
        {
            query = $"Select {temp.GetSelectors()} From {temp.GetTable()};";

            try
            {
                items.Clear();
                await cnn.OpenAsync();
                foreach (DataRow row in cnn.GetDataTable(query))
                {
                    items.Add(new Cargo(
                        DBConnection.GetIntOrNull(row["КодГруза"], 0),
                        DBConnection.GetIntOrNull(row["КодЗаказа"], 0),
                        DBConnection.GetFloatOrNull(row["Длина"], 0),
                        DBConnection.GetFloatOrNull(row["Ширина"], 0),
                        DBConnection.GetFloatOrNull(row["Высота"], 0),
                        DBConnection.GetStringOrNull(row["Тип"], string.Empty),
                        DBConnection.GetStringOrNull(row["Категория"], string.Empty),
                        DBConnection.GetFloatOrNull(row["Вес"], 0),
                        DBConnection.GetIntOrNull(row["Количество"], 0)
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

        public override async Task<Cargo> GetItemAsync(int id)
        {
            query = $"Select {temp.GetSelectors()} From {temp.GetTable()} Where {temp.GetSelectorID} = @1;";

            try
            {
                await cnn.OpenAsync();
                DataRow row = cnn.GetDataTableParam(query, id);

                foreach (Cargo cargo in items)
                {
                    if (cargo.ID == DBConnection.GetIntOrNull(row["КодГруза"], 0)) return cargo;
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

        public override async Task<IEnumerable<Cargo>> GetItemsAsync(bool forceRefresh = false)
        {
            query = $"Select {temp.GetSelectors()} From {temp.GetTable()};";

            try
            {
                items.Clear();
                await cnn.OpenAsync();
                foreach (DataRow row in cnn.GetDataTable(query))
                {
                    items.Add(new Cargo(
                        DBConnection.GetIntOrNull(row["КодГруза"], 0),
                        DBConnection.GetIntOrNull(row["КодЗаказа"], 0),
                        DBConnection.GetFloatOrNull(row["Длина"], 0),
                        DBConnection.GetFloatOrNull(row["Ширина"], 0),
                        DBConnection.GetFloatOrNull(row["Высота"], 0),
                        DBConnection.GetStringOrNull(row["Тип"], string.Empty),
                        DBConnection.GetStringOrNull(row["Категория"], string.Empty),
                        DBConnection.GetFloatOrNull(row["Вес"], 0),
                        DBConnection.GetIntOrNull(row["Количество"], 0)
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