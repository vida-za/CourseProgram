using CourseProgram.Commands;
using CourseProgram.Controllers.DataControllers.EntityDataControllers;
using CourseProgram.Models;
using CourseProgram.Services;
using CourseProgram.Stores;
using CourseProgram.ViewModels.EntityViewModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace CourseProgram.ViewModels.DetailViewModel
{
    public class BudDetailViewModel : BaseDetailViewModel
    {
        private readonly BudViewModel _budViewModel;
        private readonly ControllersStore _controllersStore;
        private Client _clientModel;
        private Worker _workerModel;
        private Address _addressLoad;
        private Address _addressOnLoad;

        public ICommand AcceptBud { get; }
        public ICommand CancelBud { get; }
        public ICommand Back { get; }

        private ObservableCollection<CargoViewModel> _cargos;
        public ObservableCollection<CargoViewModel> Cargos
        {
            get => _cargos;
            set
            {
                _cargos = value;
                OnPropertyChanged(nameof(Cargos));
            }
        }

        public int ID => _budViewModel.ID;

        public BudDetailViewModel(ServicesStore servicesStore, SelectedStore selectedStore, ControllersStore controllersStore, INavigationService closeNavigationService)
        {
            _budViewModel = selectedStore.CurrentBud;
            _controllersStore = controllersStore;
            _isHistory = _budViewModel.GetModel().Status != Constants.BudStatusValues.Waiting;

            _cargos = new ObservableCollection<CargoViewModel>();

            AcceptBud = new UpdateBudCommand(servicesStore, _budViewModel, true, closeNavigationService);
            CancelBud = new UpdateBudCommand(servicesStore, _budViewModel, false, closeNavigationService);
            Back = new NavigateCommand(closeNavigationService);

            UpdateData();
        }

        private async void UpdateData()
        {
            _clientModel = await _controllersStore.GetController<Client>().GetItemByID(_budViewModel.ClientID);
            _workerModel = await _controllersStore.GetController<Worker>().GetItemByID(_budViewModel.WorkerID);
            _addressLoad = await _controllersStore.GetController<Address>().GetItemByID(_budViewModel.AddressLoadID);
            _addressOnLoad = await _controllersStore.GetController<Address>().GetItemByID(_budViewModel.AddressOnLoadID);

            ClientName = _clientModel != null ? _clientModel.Name : "Ошибка";
            WorkerName = _workerModel != null ? _workerModel.FIO : "Ошибка";
            AddressLoadName = _addressLoad != null ? _addressLoad.FullAddress : "Ошибка";
            AddressOnLoadName = _addressOnLoad != null ? _addressOnLoad.FullAddress : "Ошибка";

            _cargos.Clear();

            IEnumerable<Cargo> temp = await ((CargoDataController)_controllersStore.GetController<Cargo>()).GetCargosByBud(ID);
            foreach (var cargo in temp)
            {
                var cargoViewModel = new CargoViewModel(cargo, _controllersStore);
                _cargos.Add(cargoViewModel);
            }
        }

        private bool _isHistory;
        public bool IsHistory
        {
            get => _isHistory;
            set
            {
                _isHistory = value;
                OnPropertyChanged(nameof(IsHistory));
            }
        }

        private string _clientName;
        public string ClientName
        {
            get => _clientName;
            set
            {
                _clientName = value;
                OnPropertyChanged(nameof(ClientName));
            }
        }
        private string _workerName;
        public string WorkerName
        {
            get => _workerName;
            set
            {
                _workerName = value;
                OnPropertyChanged(nameof(WorkerName));
            }
        }
        private string _addressLoadName;
        public string AddressLoadName
        {
            get => _addressLoadName;
            set
            {
                _addressLoadName = value;
                OnPropertyChanged(nameof(AddressLoadName));
            }
        }
        private string _addressOnLoadName;
        public string AddressOnLoadName
        {
            get => _addressOnLoadName;
            set
            {
                _addressOnLoadName = value;
                OnPropertyChanged(nameof(AddressOnLoadName));
            }
        }
        public string TimeBud => _budViewModel.TimeBud;
        public string Status => _budViewModel.Status;
        public string Description => _budViewModel.Description;
        public string DateTimeLoad => _budViewModel.DateTimeLoad;
        public string DateTimeOnLoad => _budViewModel.DateTimeOnLoad;
    }
}