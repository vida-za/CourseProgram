using CourseProgram.Models;
using System.Data;
using static CourseProgram.Models.Constants;

namespace CourseProgram.Services.DataServices
{
    public class CargoDataService : BaseService<Cargo>
    {
        public CargoDataService() : base(User.Username, User.Password) { }

        public override void CreateElement(DataRow row)
        {
            items.Add(new Cargo(GetIntOrNull(row["КодГруза"], 0),
                GetIntOrNull(row["КодЗаявки"], 0),
                GetIntOrNull(row["КодНоменклатуры"], 0),
                GetFloatOrNull(row["Объём"], 0),
                GetFloatOrNull(row["Вес"], 0),
                GetIntOrNull(row["Количество"], 0)
                ));
        }
    }
}