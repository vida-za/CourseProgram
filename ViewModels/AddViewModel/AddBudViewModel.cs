using CourseProgram.Commands;
using CourseProgram.Commands.AddCommands;
using CourseProgram.Models;
using CourseProgram.Services;
using CourseProgram.Stores;
using CourseProgram.ViewModels.EntityViewModel;
using GalaSoft.MvvmLight.Command;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace CourseProgram.ViewModels.AddViewModel
{
    public class AddBudViewModel : BaseAddViewModel
    {
        private readonly ServicesStore _servicesStore;

        public ObservableCollection<Client> Clients { get; set; }
        public ObservableCollection<Worker> Workers { get; set; }
        public IEnumerable<Nomenclature> Nomenclatures { get; set; }

        public ICommand AddCargo { get; }
        public ICommand RemoveCargo { get; }

        public AddBudViewModel(ServicesStore servicesStore, INavigationService operationalViewNavigationService)
        {
            _servicesStore = servicesStore;

            Clients = new ObservableCollection<Client>();
            Workers = new ObservableCollection<Worker>();
            Cargos = new ObservableCollection<CargoViewModel>();

            SubmitCommand = new AddBudCommand(this, _servicesStore, operationalViewNavigationService);
            CancelCommand = new NavigateCommand(operationalViewNavigationService);

            AddCargo = new RelayCommand(AddCargoFunc);
            RemoveCargo = new RelayCommand(RemoveCargoFunc);

            UpdateData();
        }

        private async void UpdateData()
        {
            Clients.Clear();
            Workers.Clear();

            IEnumerable<Client> tempClients = await _servicesStore._clientService.GetItemsAsync();
            foreach (var temp in tempClients)
                Clients.Add(temp);

            IEnumerable<Worker> tempWorkers = await _servicesStore._workerService.GetItemsAsync();
            foreach (var temp in tempWorkers)
                if (temp.DateEnd == null) Workers.Add(temp);

            Nomenclatures = await _servicesStore._nomenclatureService.GetItemsAsync();

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
                _description = value;
                OnPropertyChanged(nameof(Description));
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
            Cargos.Add(new CargoViewModel(new Cargo(), _servicesStore));
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