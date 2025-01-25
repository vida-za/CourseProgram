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
            items.Add(new Worker(GetIntOrNull(row["КодСотрудника"], 0),
                GetStringOrNull(row["ФИО"], string.Empty),
                GetDateOnlyOrNull(row["ДатаРождения"], DateOnly.MinValue),
                GetStringOrNull(row["ПаспортныеДанные"], string.Empty),
                GetStringOrNull(row["Телефон"], string.Empty),
                GetDateOnlyOrNull(row["ДатаНачалаРаботы"], DateOnly.MinValue),
                GetDateOnlyOrNull(row["ДатаОкончанияРаботы"], DateOnly.MinValue)
                ));
        }
    }
}