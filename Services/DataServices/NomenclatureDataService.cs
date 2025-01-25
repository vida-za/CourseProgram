using CourseProgram.Models;
using System.Data;
using static CourseProgram.Models.Constants;

namespace CourseProgram.Services.DataServices
{
    public class NomenclatureDataService : BaseService<Nomenclature>
    {
        public NomenclatureDataService() : base(User.Username, User.Password) { }

        public override void CreateElement(DataRow row)
        {
            items.Add(new Nomenclature(GetIntOrNull(row["КодНоменклатуры"], 0),
                GetStringOrNull(row["Наименование"], string.Empty),
                GetStringOrNull(row["Тип"], string.Empty),
                GetStringOrNull(row["Категория"], string.Empty),
                GetFloatOrNull(row["Длина"], 0),
                GetFloatOrNull(row["Ширина"], 0),
                GetFloatOrNull(row["Высота"], 0),
                GetFloatOrNull(row["Вес"], 0),
                GetStringOrNull(row["ЕдиницаИзмерения"], string.Empty),
                GetStringOrNull(row["Упаковка"], string.Empty),
                GetStringOrNull(row["ТребованияКТемпературе"], string.Empty),
                GetStringOrNull(row["Опасность"], string.Empty),
                GetStringOrNull(row["Производитель"], string.Empty),
                GetIntOrNull(row["СрокГодности"], 0)));
        }
    }
}