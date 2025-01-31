using CourseProgram.Models;
using CourseProgram.Stores;
using System;
using System.ComponentModel;

namespace CourseProgram.ViewModels.EntityViewModel
{
    public class OrderViewModel : BaseViewModel
    {
        private readonly ServicesStore _servicesStore;

        private readonly Order _model;
        private Bud _budModel;
        private Client _clientModel;

        public readonly int ID;
        public readonly int BudID;

        public Order GetModel() => _model;

        [DisplayName("Название клиента")]
        public string ClientName => _clientModel.Name;
        [DisplayName("Время заказа")]
        public string TimeOrder => _model.TimeOrder.ToString("g");
        [DisplayName("Время загрузки")]
        public string TimeLoad => _model.TimeLoad != null ? ((DateTime)_model.TimeLoad).ToString("g") : "-";
        [DisplayName("Время разгрузки")]
        public string TimeOnLoad => _model.TimeOnLoad != null ? ((DateTime)_model.TimeOnLoad).ToString("g") : "-";
        [DisplayName("Стоимость")]
        public string Price => _model.Price.ToString();
        [DisplayName("Статус")]
        public string Status => Constants.GetEnumDescription(_model.Status);
        [DisplayName("Договор")]
        public string File => _model.File ?? "-";

        public OrderViewModel(Order model, ServicesStore servicesStore)
        {
            _model = model;
            _servicesStore = servicesStore;

            ID = _model.ID;
            BudID = _model.BudID;

            UpdateData();
        }

        private async void UpdateData()
        {
            _budModel = await _servicesStore._budService.GetItemAsync(BudID);
            _clientModel = await _servicesStore._clientService.GetItemAsync(_budModel.ClientID);
        }
    }
}