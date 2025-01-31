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
            items.Add(new Address(GetInt(row["КодАдреса"], 0),
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