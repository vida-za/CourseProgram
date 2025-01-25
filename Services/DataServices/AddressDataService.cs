using CourseProgram.Models;
using System.Data;
using static CourseProgram.Models.Constants;

namespace CourseProgram.Services.DataServices
{
    public class AddressDataService : BaseService<Address>
    {
        public AddressDataService() : base(User.Username, User.Password) { }

        public override void CreateElement(DataRow row)
        {
            items.Add(new Address(GetIntOrNull(row["КодАдреса"], 0),
                GetStringOrNull(row["Город"], string.Empty),
                GetStringOrNull(row["Улица"], string.Empty),
                GetStringOrNull(row["Дом"], string.Empty),
                GetStringOrNull(row["Строение"], string.Empty),
                GetStringOrNull(row["Корпус"], string.Empty),
                GetBoolOrNull(row["Активен"], true)
                ));
        }
    }
}