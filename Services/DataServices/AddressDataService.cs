using CourseProgram.Models;
using CourseProgram.Stores;
using System.Data;
using System.Threading.Tasks;
using static CourseProgram.Models.Constants;

namespace CourseProgram.Services.DataServices
{
    public class AddressDataService : BaseService<Address>
    {
        public AddressDataService(DataStore dataStore) : base(User.Username, User.Password, dataStore) { }

        public override Task<Address> CreateElement(DataRow row)
        {
            return Task.FromResult(new Address(GetInt(row["КодАдреса"], 0),
                GetString(row["Город"], string.Empty),
                GetString(row["Улица"], string.Empty),
                GetStringOrNull(row["Дом"]),
                GetStringOrNull(row["Строение"]),
                GetStringOrNull(row["Корпус"])
                ));
        }
    }
}