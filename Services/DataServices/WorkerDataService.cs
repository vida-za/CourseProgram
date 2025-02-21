﻿using CourseProgram.Models;
using System;
using System.Data;
using System.Threading.Tasks;
using static CourseProgram.Models.Constants;

namespace CourseProgram.Services.DataServices
{
    public class WorkerDataService : BaseService<Worker>
    {
        public WorkerDataService() : base(User.Username, User.Password) { }

        public override Task<Worker> CreateElement(DataRow row)
        {
            return Task.FromResult(new Worker(GetInt(row["КодСотрудника"], 0),
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