using CourseProgram.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using static CourseProgram.Models.Constants;

namespace CourseProgram.Services.DataServices
{
    public class ClientDataService : BaseService<Client>
    {
        public ClientDataService()
        {
            cnn = new DBConnection(Server, Database, User.Username, User.Password, "ClientLoad");
            temp = new Client();
        }

        public override async Task<bool> AddItemAsync(Client item)
        {
            query = $"Insert Into {temp.GetTable()} ({temp.GetSelectors()}) Values(@1, @2, @3, @4, @5, @6, @7, @8, @9, @10, @11, @12, @13);";

            try
            {
                await cnn.OpenAsync();
                var res = await cnn.ExecParamAsync(query, item.ID, item.Name, item.Type, item.INN, item.KPP, item.OGRN, item.Phone, item.Checking, item.BIK, item.Correspondent, item.Bank, item.PhoneLoad, item.PhoneOnLoad);
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

        public override async Task<IEnumerable<Client>> GetFullTableAsync(bool forceRefresh = false)
        {
            query = $"Select {temp.GetSelectors()} From {temp.GetTable()};";

            try
            {
                items.Clear();
                await cnn.OpenAsync();
                foreach (DataRow row in cnn.GetDataTable(query))
                {
                    items.Add(new Client(
                        DBConnection.GetIntOrNull(row["КодЗаказчика"], 0),
                        DBConnection.GetStringOrNull(row["Название"], string.Empty),
                        DBConnection.GetStringOrNull(row["ТипЗаказчика"], string.Empty),
                        DBConnection.GetStringOrNull(row["ИНН"], string.Empty),
                        DBConnection.GetStringOrNull(row["КПП"], string.Empty),
                        DBConnection.GetStringOrNull(row["ОГРН"], string.Empty),
                        DBConnection.GetStringOrNull(row["Телефон"], string.Empty),
                        DBConnection.GetStringOrNull(row["РасчётныйСчёт"], string.Empty),
                        DBConnection.GetStringOrNull(row["БИК"], string.Empty),
                        DBConnection.GetStringOrNull(row["КорреспондентскийСчёт"], string.Empty),
                        DBConnection.GetStringOrNull(row["Банк"], string.Empty),
                        DBConnection.GetStringOrNull(row["КонтактЗагрузки"], string.Empty),
                        DBConnection.GetStringOrNull(row["КонтактВыгрузки"], string.Empty)
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

        public override async Task<Client> GetItemAsync(int id)
        {
            query = $"Select {temp.GetSelectors()} From {temp.GetTable()} Where \"КодЗаказчика\" = @1;";

            try
            {
                await cnn.OpenAsync();
                DataRow row = cnn.GetDataTableParam(query, id);

                foreach (Client client in items)
                {
                    if (client.ID == DBConnection.GetIntOrNull(row["КодЗаказчика"], 0)) return client;
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

        public override async Task<IEnumerable<Client>> GetItemsAsync(bool forceRefresh = false)
        {
            query = $"Select {temp.GetSelectors()} From {temp.GetTable()};";

            try
            {
                items.Clear();
                await cnn.OpenAsync();
                foreach (DataRow row in cnn.GetDataTable(query))
                {
                    items.Add(new Client(
                        DBConnection.GetIntOrNull(row["КодЗаказчика"], 0),
                        DBConnection.GetStringOrNull(row["Название"], string.Empty),
                        DBConnection.GetStringOrNull(row["ТипЗаказчика"], string.Empty),
                        DBConnection.GetStringOrNull(row["ИНН"], string.Empty),
                        DBConnection.GetStringOrNull(row["КПП"], string.Empty),
                        DBConnection.GetStringOrNull(row["ОГРН"], string.Empty),
                        DBConnection.GetStringOrNull(row["Телефон"], string.Empty),
                        DBConnection.GetStringOrNull(row["РасчётныйСчёт"], string.Empty),
                        DBConnection.GetStringOrNull(row["БИК"], string.Empty),
                        DBConnection.GetStringOrNull(row["КорреспондентскийСчёт"], string.Empty),
                        DBConnection.GetStringOrNull(row["Банк"], string.Empty),
                        DBConnection.GetStringOrNull(row["КонтактЗагрузки"], string.Empty),
                        DBConnection.GetStringOrNull(row["КонтактВыгрузки"], string.Empty)
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