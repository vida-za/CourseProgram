using CourseProgram.Models;
using CourseProgram.Stores;
using System;
using System.Data;
using System.Threading.Tasks;
using static CourseProgram.Models.Constants;

namespace CourseProgram.Services.DataServices
{
    public class DriverDataService : BaseService<Driver>
    {
        public DriverDataService(DataStore dataStore) : base(User.Username, User.Password, dataStore) { }

        public override Task<Driver> CreateElement(DataRow row)
        {
            return Task.FromResult(new Driver(
                GetInt(row["КодВодителя"], 0),
                GetString(row["ФИО"], string.Empty),
                GetDateOnlyOrNull(row["ДатаРождения"]),
                GetString(row["ПаспортныеДанные"], string.Empty),
                GetStringOrNull(row["Телефон"]),
                GetDateOnly(row["ДатаНачала"], DateOnly.MinValue),
                GetDateOnlyOrNull(row["ДатаОкончания"])));
        }
    }
}