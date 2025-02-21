using CourseProgram.Models;
using CourseProgram.Services.DBServices;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using static CourseProgram.Models.Constants;

namespace CourseProgram.Services.DataServices
{
    public class AddressDataService : BaseService<Address>
    {
        public AddressDataService() : base(User.Username, User.Password) { }

        public async Task<IEnumerable<Address>> GetActAddressesAsync()
        {
            using (var query = new Query(CommandTypes.SelectQuery, Address.GetTable()))
            {
                try
                {
                    var tempItems = new List<Address>();

                    query.AddFields(Address.GetFieldNames());
                    query.WhereClause.Equals("Активен", "true");

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
                    await LogManager.Instance.WriteLogAsync($"Error in {nameof(GetActAddressesAsync)}: {ex.Message}");
                    throw;
                }
                finally
                {
                    query?.Dispose();
                }
            }
        }

        public override Task<Address> CreateElement(DataRow row)
        {
            return Task.FromResult(new Address(GetInt(row["КодАдреса"], 0),
                GetString(row["Город"], string.Empty),
                GetString(row["Улица"], string.Empty),
                GetStringOrNull(row["Дом"]),
                GetStringOrNull(row["Строение"]),
                GetStringOrNull(row["Корпус"]),
                GetBool(row["Активен"], true)
                ));
        }
    }
}