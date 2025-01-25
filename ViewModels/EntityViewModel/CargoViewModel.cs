using CourseProgram.Models;
using CourseProgram.Stores;
using System.ComponentModel;

namespace CourseProgram.ViewModels.EntityViewModel
{
    public class CargoViewModel : BaseViewModel
    {
        private readonly ServicesStore _servicesStore;

        private readonly Cargo _model;
        private Order _orderModel;
        private Nomenclature _nomenclatureModel;

        public Cargo GetModel() => _model;

        [DisplayName("Номер груза")]
        public int ID => _model.ID;
        [DisplayName("Номер заявки")]
        public int OrderID => _model.BudID;
        [DisplayName("Номер номенклатуры")]
        public int NomenclatureID => _model.NomenclatureID;
        [DisplayName("Объём")]
        public string Volume => _model.Volume.ToString();
        [DisplayName("Вес")]
        public string Weight => _model.Weight.ToString();
        [DisplayName("Количество")]
        public string Count => _model.Count.ToString();

        public CargoViewModel(Cargo cargo, ServicesStore servicesStore)
        {
            _model = cargo;
            _servicesStore = servicesStore;

            UpdateData();
        }

        private async void UpdateData()
        {
            _orderModel = await _servicesStore._orderService.GetItemAsync(OrderID);
            _nomenclatureModel = await _servicesStore._nomenclatureService.GetItemAsync(NomenclatureID);
        }
    }
}