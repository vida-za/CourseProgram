using CourseProgram.Models;
using CourseProgram.Stores;
using System.ComponentModel;

namespace CourseProgram.ViewModels.EntityViewModel
{
    public class OrderViewModel : BaseViewModel
    {
        private readonly ServicesStore _servicesStore;

        private readonly Order _model;
        private Bud _budModel;

        public Order GetModel() => _model;

        [DisplayName("Номер заказа")]
        public int ID => _model.ID;
        [DisplayName("Номер заявки")]
        public int BudID => _model.BudID;
        [DisplayName("Время заказа")]
        public string TimeOrder => _model.TimeOrder.ToString();
        [DisplayName("Время загрузки")]
        public string TimeLoad => _model.TimeLoad.ToString();
        [DisplayName("Время разгрузки")]
        public string TimeOnLoad => _model.TimeOnLoad.ToString();
        [DisplayName("Стоимость")]
        public string Price => _model.Price.ToString();
        [DisplayName("Статус")]
        public string Status => Constants.GetEnumDescription(_model.Status);
        [DisplayName("Договор")]
        public string File => _model.File;

        public OrderViewModel(Order model, ServicesStore servicesStore)
        {
            _model = model;
            _servicesStore = servicesStore;

            UpdateData();
        }

        private async void UpdateData()
        {
            _budModel = await _servicesStore._budService.GetItemAsync(BudID);
        }
    }
}