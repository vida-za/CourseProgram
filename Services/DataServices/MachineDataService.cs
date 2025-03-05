using CourseProgram.Models;
using CourseProgram.Stores;
using System;
using System.Data;
using System.Threading.Tasks;
using static CourseProgram.Models.Constants;

namespace CourseProgram.Services.DataServices
{
    public class MachineDataService : BaseService<Machine>
    {
        public MachineDataService(DataStore dataStore) : base(User.Username, User.Password, dataStore) { }

        public override Task<Machine> CreateElement(DataRow row)
        {
            return Task.FromResult(new Machine(GetInt(row["КодМашины"], 0),
                GetString(row["ТипМашины"], string.Empty),
                GetStringOrNull(row["ТипКузова"]),
                GetString(row["ТипЗагрузки"], string.Empty),
                GetFloat(row["Грузоподъёмность"], 0),
                GetFloatOrNull(row["Объём"]),
                GetBool(row["Гидроборт"], false),
                GetFloatOrNull(row["ДлинаКузова"]),
                GetFloatOrNull(row["ШиринаКузова"]),
                GetFloatOrNull(row["ВысотаКузова"]),
                GetString(row["Марка"], string.Empty),
                GetString(row["Название"], string.Empty),
                GetStringOrNull(row["ГосНомер"]),
                GetString(row["Состояние"], string.Empty),
                GetDateTime(row["ДатаВремяПоступления"], DateTime.MinValue),
                GetDateTimeOrNull(row["ДатаВремяСписания"]),
                GetIntOrNull(row["КодАдреса"]),
                GetInt(row["КодКатегории"], 0)));
        }
    }
}