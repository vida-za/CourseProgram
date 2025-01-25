using CourseProgram.Models;
using System;
using System.Data;
using static CourseProgram.Models.Constants;

namespace CourseProgram.Services.DataServices
{
    public class OrderDataService : BaseService<Order>
    {
        public OrderDataService() : base(User.Username, User.Password) { }

        public override void CreateElement(DataRow row)
        {
            items.Add(new Order(GetIntOrNull(row["КодЗаказа"], 0),
                GetIntOrNull(row["КодЗаявки"], 0),
                GetDateTimeOrNull(row["ДатаЗаказа"], DateTime.MinValue),
                GetDateTimeOrNull(row["ДатаЗагрузки"], DateTime.MinValue),
                GetDateTimeOrNull(row["ДатаВыгрузки"], DateTime.MinValue),
                GetFloatOrNull(row["Стоимость"], 0),
                GetStringOrNull(row["Статус"], string.Empty),
                GetStringOrNull(row["Договор"], string.Empty)
                ));
        }
    }
}