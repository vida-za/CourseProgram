using CourseProgram.Models;
using System;
using System.Data;
using static CourseProgram.Models.Constants;

namespace CourseProgram.Services.DataServices
{
    public class HaulDataService : BaseService<Haul>
    {
        public HaulDataService() : base(User.Username, User.Password) { }

        public override void CreateElement(DataRow row)
        {
            items.Add(new Haul(GetIntOrNull(row["КодРейса"], 0),
                GetDateOnlyOrNull(row["ДатаНачала"], DateOnly.MinValue),
                GetDateOnlyOrNull(row["ДатаОкончания"], DateOnly.MinValue),
                GetFloatOrNull(row["СуммарныйДоход"], 0)
                ));
        }
    }
}