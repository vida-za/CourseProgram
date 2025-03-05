using CourseProgram.Models;
using CourseProgram.Stores;
using System;
using System.ComponentModel;

namespace CourseProgram.ViewModels.EntityViewModel
{
    public class OrderViewModel : BaseViewModel
    {
        private readonly ControllersStore _controllersStore;

        private readonly Order _model;
        private Bud _budModel;
        private Client _clientModel;

        public readonly int ID;
        public readonly int BudID;

        public Order GetModel() => _model;

        private string _clientName;
        [DisplayName("Название клиента")]
        public string ClientName
        {
            get => _clientName;
            set
            {
                _clientName = value;
                OnPropertyChanged(nameof(ClientName));
            }
        }
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

        public OrderViewModel(Order model, ControllersStore controllersStore)
        {
            _model = model;
            _controllersStore = controllersStore;

            ID = _model.ID;
            BudID = _model.BudID;

            UpdateData();
        }

        private async void UpdateData()
        {
            _budModel = await _controllersStore.GetController<Bud>().GetItemByID(BudID);
            _clientModel = await _controllersStore.GetController<Client>().GetItemByID(_budModel.ClientID);

            ClientName = _clientModel.Name;
        }
    }
}