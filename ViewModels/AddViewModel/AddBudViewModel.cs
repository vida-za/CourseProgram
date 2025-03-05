using CourseProgram.Commands;
using CourseProgram.Commands.AddCommands;
using CourseProgram.Controllers.DataControllers.EntityDataControllers;
using CourseProgram.Models;
using CourseProgram.Services;
using CourseProgram.Stores;
using CourseProgram.ViewModels.EntityViewModel;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace CourseProgram.ViewModels.AddViewModel
{
    public class AddBudViewModel : BaseAddViewModel
    {
        private readonly ControllersStore _controllersStore;

        public ObservableCollection<Client> Clients { get; set; }
        public ObservableCollection<Worker> Workers { get; set; }
        public IEnumerable<Nomenclature> Nomenclatures { get; set; }
        public IEnumerable<Address> Addresses { get; set; }

        public ICommand AddCargo { get; }
        public ICommand RemoveCargo { get; }

        public AddBudViewModel(ServicesStore servicesStore, ControllersStore controllersStore, INavigationService operationalViewNavigationService)
        {
            _controllersStore = controllersStore;

            Clients = new ObservableCollection<Client>();
            Workers = new ObservableCollection<Worker>();
            Cargos = new ObservableCollection<CargoViewModel>();

            SubmitCommand = new AddBudCommand(this, servicesStore, controllersStore, operationalViewNavigationService);
            CancelCommand = new NavigateCommand(operationalViewNavigationService);

            AddCargo = new RelayCommand(AddCargoFunc);
            RemoveCargo = new RelayCommand(RemoveCargoFunc);

            UpdateData();
        }

        private async void UpdateData()
        {
            Clients.Clear();
            Workers.Clear();

            IEnumerable<Client> tempClients = await _controllersStore.GetController<Client>().GetItems();
            foreach (var temp in tempClients)
                Clients.Add(temp);

            IEnumerable<Worker> tempWorkers = await ((WorkerDataController)_controllersStore.GetController<Worker>()).GetActiveWorkers(true);
            foreach (var temp in tempWorkers)
                if (temp.DateEnd == null) Workers.Add(temp);

            Nomenclatures = await _controllersStore.GetController<Nomenclature>().GetItems();
            Addresses = await _controllersStore.GetController<Address>().GetItems();

            OnPropertyChanged(nameof(Clients));
            OnPropertyChanged(nameof(Workers));
            OnPropertyChanged(nameof(Nomenclatures));
        }

        private Client _selectedClient;
        public Client SelectedClient
        {
            get => _selectedClient;
            set
            {
                _selectedClient = value;
                OnPropertyChanged(nameof(SelectedClient));
            }
        }

        private Worker _selectedWorker;
        public Worker SelectedWorker
        {
            get => _selectedWorker;
            set
            {
                _selectedWorker = value;
                OnPropertyChanged(nameof(SelectedWorker));
            }
        }

        private Address _selectedAddressLoad;
        public Address SelectedAddressLoad
        {
            get => _selectedAddressLoad;
            set
            {
                _selectedAddressLoad = value;
                OnPropertyChanged(nameof(SelectedAddressLoad));
            }
        }

        private Address _selectedAddressOnLoad;
        public Address SelectedAddressOnLoad
        {
            get => _selectedAddressOnLoad;
            set
            {
                _selectedAddressOnLoad = value;
                OnPropertyChanged(nameof(SelectedAddressOnLoad));
            }
        }

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

        private string _description;
        public string Description
        {
            get => _description;
            set
            {
                if (LettersAndDigitsRegex.IsMatch(value))
                {
                    _description = value;
                    OnPropertyChanged(nameof(Description));
                }
            }
        }


        private DateTime _dateTimeLoadPlan = DateTime.Now;
        public DateTime DateTimeLoadPlan
        {
            get => _dateTimeLoadPlan;
            set
            {
                _dateTimeLoadPlan = value;
                OnPropertyChanged(nameof(DateTimeLoadPlan));
            }
        }

        private DateTime _dateTimeOnLoadPlan = DateTime.Now;
        public DateTime DateTimeOnLoadPlan
        {
            get => _dateTimeOnLoadPlan;
            set
            {
                _dateTimeOnLoadPlan = value;
                OnPropertyChanged(nameof(DateTimeOnLoadPlan));
            }
        }
        #region cargosreg
        private CargoViewModel _selectedCargo;
        public CargoViewModel SelectedCargo
        {
            get => _selectedCargo;
            set
            {
                _selectedCargo = value;
                OnPropertyChanged(nameof(SelectedCargo));
            }
        }

        private void AddCargoFunc()
        {
            Cargos.Add(new CargoViewModel(new Cargo(), _controllersStore));
        }

        private void RemoveCargoFunc()
        {
            if (SelectedCargo != null)
            {
                Cargos.Remove(SelectedCargo);
                SelectedCargo = null;
                OnPropertyChanged(nameof(Cargos));
                OnPropertyChanged(nameof(SelectedCargo));
            }
        }
        #endregion
    }
}