using CourseProgram.Models;
using System.ComponentModel;

namespace CourseProgram.ViewModels.EntityViewModel
{
    public class CargoViewModel : BaseViewModel
    {
        private readonly Cargo _model;

        public Cargo GetModel() => _model;

        [DisplayName("Номер груза")]
        public int ID => _model.ID;
        [DisplayName("Номер заказа")]
        public string OrderID => _model.OrderID.ToString();
        [DisplayName("Длина")]
        public string Length => _model.Length.ToString();
        [DisplayName("Ширина")]
        public string Width => _model.Width.ToString();
        [DisplayName("Высота")]
        public string Height => _model.Height.ToString();
        [DisplayName("Тип")]
        public string Type => Constants.GetEnumDescription(_model.Type);
        [DisplayName("Категория")]
        public string Category => Constants.GetEnumDescription(_model.Category);
        [DisplayName("Вес")]
        public string Weight => _model.Weight.ToString();
        [DisplayName("Количество")]
        public string Count => _model.Count.ToString();

        public CargoViewModel(Cargo cargo)
        {
            _model = cargo;
        }
    }
}