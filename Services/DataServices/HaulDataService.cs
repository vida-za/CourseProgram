using CourseProgram.Models;
using CourseProgram.Stores;
using System;
using System.Data;
using System.Threading.Tasks;
using static CourseProgram.Models.Constants;

namespace CourseProgram.Services.DataServices
{
    public class HaulDataService : BaseService<Haul>
    {
        public HaulDataService(DataStore dataStore) : base(User.Username, User.Password, dataStore) { }

        public override Task<Haul> CreateElement(DataRow row)
        {
            return Task.FromResult(new Haul(GetInt(row["КодРейса"], 0),
                GetDateOnly(row["ДатаНачала"], DateOnly.MinValue),
                GetDateOnlyOrNull(row["ДатаОкончания"]),
                GetFloatOrNull(row["СуммарныйДоход"])
                ));
        }
    }
}