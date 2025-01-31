using CourseProgram.Models;
using System;
using System.Data;
using static CourseProgram.Models.Constants;

namespace CourseProgram.Services.DataServices
{
    public class BudDataService : BaseService<Bud>
    {
        public BudDataService() : base(User.Username, User.Password) { }

        public override void CreateElement(DataRow row)
        {
            items.Add(new Bud(GetInt(row["КодЗаявки"], 0),
                GetInt(row["КодЗаказчика"], 0),
                GetInt(row["КодСотрудника"], 0),
                GetDateTime(row["ДатаВремяЗаявки"], DateTime.MinValue),
                GetString(row["Статус"], string.Empty),
                GetStringOrNull(row["Описание"])));
        }
    }
}