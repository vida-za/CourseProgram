using CourseProgram.Models;
using System.ComponentModel;

namespace CourseProgram.ViewModels.EntityViewModel
{
    public class NomenclatureViewModel : BaseViewModel
    {
        private readonly Nomenclature _model;
        public readonly int ID;

        [DisplayName("Наименование")]
        public string Name => _model.Name;
        [DisplayName("Тип")]
        public string Type => Constants.GetEnumDescription(_model.Type);
        [DisplayName("Категория")]
        public string CategoryCargo => Constants.GetEnumDescription(_model.CategoryCargo);
        [DisplayName("Габариты")]
        public string Size => $"{_model.Length}x{_model.Width}x{_model.Height}";
        [DisplayName("Вес")]
        public string Weight => _model.Weight != null ? ((float)_model.Weight).ToString() : "-";
        [DisplayName("Ед. изм.")]
        public string Unit => Constants.GetEnumDescription(_model.Unit);
        [DisplayName("Упаковка")]
        public string Pack => Constants.GetEnumDescription(_model.Pack);
        [DisplayName("Необходимая температура")]
        public string NeedTemperature => _model.NeedTemperature ?? "-";
        [DisplayName("Класс опасности")]
        public string DangerousClass => Constants.GetEnumDescription(_model.DangerousClass);
        [DisplayName("Производитель")]
        public string Manufacturer => _model.Manufacturer ?? "-";
        [DisplayName("Срок годности")]
        public string ExpiryDate => _model.ExpiryDate != null ? _model.ExpiryDate.ToString() : "-";

        public NomenclatureViewModel(Nomenclature nomenclature)
        {
            _model = nomenclature;

            ID = _model.ID;
        }

        public Nomenclature GetModel() => _model;
    }
}