using CourseProgram.Models;
using CourseProgram.Stores;
using System;
using System.Data;
using System.Threading.Tasks;
using static CourseProgram.Models.Constants;

namespace CourseProgram.Services.DataServices
{
    public class BudDataService : BaseService<Bud>
    {
        public BudDataService(DataStore dataStore) : base(User.Username, User.Password, dataStore) { }

        public override Task<Bud> CreateElement(DataRow row)
        {
            return Task.FromResult(new Bud(GetInt(row["КодЗаявки"], 0),
                GetInt(row["КодЗаказчика"], 0),
                GetInt(row["КодСотрудника"], 0),
                GetDateTime(row["ДатаВремяЗаявки"], DateTime.MinValue),
                GetString(row["Статус"], string.Empty),
                GetStringOrNull(row["Описание"]),
                GetInt(row["КодАдресаПогрузки"], 0),
                GetInt(row["КодАдресаРазгрузки"], 0),
                GetDateTime(row["ДатаВремяПогрузкиПлан"], DateTime.Now),
                GetDateTime(row["ДатаВремяРазгрузкиПлан"], DateTime.Now)));
        }
    }
}