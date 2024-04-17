using CourseProgram.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using static CourseProgram.Models.Constants;

namespace CourseProgram.Services.DataServices
{
    public class AddressDataService : BaseService<Address>
    {
        public AddressDataService()
        {
            cnn = new DBConnection(Server, Database, User.Username, User.Password, "AddressLoad");
            temp = new Address();
        }

        public override async Task<bool> AddItemAsync(Address item)
        {
            query = $"Insert Into {temp.GetTable()} ({temp.GetSelectors()}) Values(@1, @2, @3, @4, @5, @6);";

            try
            {
                await cnn.OpenAsync();
                var res = await cnn.ExecParamAsync(query, item.ID, item.City, item.Street, item.House, item.Structure, item.Frame);
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

        public override async Task<IEnumerable<Address>> GetFullTableAsync(bool forceRefresh = false)
        {
            query = $"Select {temp.GetSelectors()} From {temp.GetTable()};";

            try
            {
                items.Clear();
                await cnn.OpenAsync();
                foreach (DataRow row in cnn.GetDataTable(query))
                {
                    items.Add(new Address(
                        DBConnection.GetIntOrNull(row["КодАдреса"], 0),
                        DBConnection.GetStringOrNull(row["Город"], string.Empty),
                        DBConnection.GetStringOrNull(row["Улица"], string.Empty),
                        DBConnection.GetStringOrNull(row["Дом"], string.Empty),
                        DBConnection.GetStringOrNull(row["Строение"], string.Empty),
                        DBConnection.GetStringOrNull(row["Корпус"], string.Empty)
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

        public override async Task<Address> GetItemAsync(int id)
        {
            query = $"Select {temp.GetSelectors()} From {temp.GetTable()} Where {temp.GetSelectorID()} = @1;";

            try
            {
                await cnn.OpenAsync();
                DataRow row = cnn.GetDataTableParam(query, id);

                foreach (Address adr in items)
                {
                    if (adr.ID == DBConnection.GetIntOrNull(row["КодАдреса"], 0)) return adr;
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

        public override async Task<IEnumerable<Address>> GetItemsAsync(bool forceRefresh = false)
        {
            query = $"Select {temp.GetSelectors()} From {temp.GetTable()};";

            try
            {
                items.Clear();
                await cnn.OpenAsync();
                foreach (DataRow row in cnn.GetDataTable(query))
                {
                    items.Add(new Address(
                        DBConnection.GetIntOrNull(row["КодАдреса"], 0),
                        DBConnection.GetStringOrNull(row["Город"], string.Empty),
                        DBConnection.GetStringOrNull(row["Улица"], string.Empty),
                        DBConnection.GetStringOrNull(row["Дом"], string.Empty),
                        DBConnection.GetStringOrNull(row["Строение"], string.Empty),
                        DBConnection.GetStringOrNull(row["Корпус"], string.Empty)
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