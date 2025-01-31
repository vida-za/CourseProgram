using CourseProgram.Models;
using System;
using System.Data;
using static CourseProgram.Models.Constants;

namespace CourseProgram.Services.DataServices
{
    public class WorkerDataService : BaseService<Worker>
    {
        public WorkerDataService() : base(User.Username, User.Password) { }

        public override void CreateElement(DataRow row)
        {
            items.Add(new Worker(GetInt(row["КодСотрудника"], 0),
                GetString(row["ФИО"], string.Empty),
                GetDateOnlyOrNull(row["ДатаРождения"]),
                GetString(row["ПаспортныеДанные"], string.Empty),
                GetStringOrNull(row["Телефон"]),
                GetDateOnly(row["ДатаНачалаРаботы"], DateOnly.MinValue),
                GetDateOnlyOrNull(row["ДатаОкончанияРаботы"])
                ));
        }
    }
}