using CourseProgram.Models;
using CourseProgram.Stores;
using System.ComponentModel;
using static CourseProgram.Models.Constants;

namespace CourseProgram.ViewModels.EntityViewModel
{
    public class BudViewModel : BaseViewModel
    {
        private readonly Bud _model;
        private readonly ControllersStore _controllersStore;

        private string _clientName;

        public readonly int ID;
        public readonly int ClientID;
        public readonly int WorkerID;
        public readonly int AddressLoadID;
        public readonly int AddressOnLoadID;

        [DisplayName("Заказчик")]
        public string ClientName
        {
            get => _clientName;
            set
            {
                _clientName = value;
                OnPropertyChanged(nameof(ClientName));
            }
        }
        [DisplayName("Время поступления")]
        public string TimeBud => _model.TimeBud.ToString("g");
        [DisplayName("Статус")]
        public string Status => GetEnumDescription(_model.Status);
        [DisplayName("Описание")]
        public string Description => _model.Description ?? "-";
        [DisplayName("Время погрузки")]
        public string DateTimeLoad => _model.DateTimeLoadPlan.ToString("g");
        [DisplayName("Время разгрузки")]
        public string DateTimeOnLoad => _model.DateTimeOnLoadPlan.ToString("g");

        public BudViewModel(Bud bud, ControllersStore controllersStore)
        {
            _model = bud;
            _controllersStore = controllersStore;

            ID = _model.ID;
            ClientID = _model.ClientID;
            WorkerID = _model.WorkerID;
            AddressLoadID = _model.AddressLoadID;
            AddressOnLoadID = _model.AddressOnLoadID;

            UpdateData();
        }
        
        private async void UpdateData()
        {
            Client tempClient = await _controllersStore.GetController<Client>().GetItemByID(ClientID);
            ClientName = tempClient != null ? tempClient.Name : "Ошибка";
        }

        public Bud GetModel() => _model;
    }
}