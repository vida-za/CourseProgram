using CourseProgram.Models;
using CourseProgram.Stores;
using System.ComponentModel;

namespace CourseProgram.ViewModels.EntityViewModel
{
    public class OrderViewModel : BaseViewModel
    {
        private readonly ServicesStore _servicesStore;
        private readonly Order _model;

        public Order GetModel() => _model;

        [DisplayName("Номер заказа")]
        public int ID => _model.ID;
        [DisplayName("Номер сотрудника")]
        public int WorkerID => _model.WorkerID;
        [DisplayName("ФИО Сотрудника")]
        public string WorkerName { get; set; }
        [DisplayName("Номер заказчика")]
        public int ClientID => _model.ClientID;
        [DisplayName("Название заказчика")]
        public string ClientName { get; set; }
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
            WorkerName = (await _servicesStore._workerService.GetItemAsync(WorkerID)).FIO;
            ClientName = (await _servicesStore._clientService.GetItemAsync(ClientID)).Name;
        }
    }
}