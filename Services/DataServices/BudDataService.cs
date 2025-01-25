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
            items.Add(new Bud(GetIntOrNull(row["КодЗаявки"], 0),
                GetIntOrNull(row["КодЗаказчика"], 0),
                GetIntOrNull(row["КодСотрудника"], 0),
                GetDateTimeOrNull(row["ДатаВремяЗаявки"], DateTime.MinValue),
                GetStringOrNull(row["Статус"], string.Empty),
                GetStringOrNull(row["Описание"], string.Empty)));
        }
    }
}