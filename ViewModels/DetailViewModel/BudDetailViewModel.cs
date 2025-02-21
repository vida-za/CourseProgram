using CourseProgram.Commands;
using CourseProgram.Models;
using CourseProgram.Services;
using CourseProgram.Services.DataServices;
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
        private readonly ServicesStore _servicesStore;
        private Client _clientModel;
        private Worker _workerModel;

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

        public BudDetailViewModel(ServicesStore servicesStore, SelectedStore selectedStore, INavigationService closeNavigationService)
        {
            _budViewModel = selectedStore.CurrentBud;
            _servicesStore = servicesStore;

            _cargos = new ObservableCollection<CargoViewModel>();

            AcceptBud = new UpdateBudCommand(_servicesStore, _budViewModel, true, closeNavigationService);
            CancelBud = new UpdateBudCommand(_servicesStore, _budViewModel, false, closeNavigationService);
            Back = new NavigateCommand(closeNavigationService);

            UpdateData();
        }

        private async void UpdateData()
        {
            _clientModel = await _servicesStore.GetService<Client>().GetItemAsync(_budViewModel.ClientID);
            _workerModel = await _servicesStore.GetService<Worker>().GetItemAsync(_budViewModel.WorkerID);

            ClientName = _clientModel != null ? _clientModel.Name : "Не указан";
            WorkerName = _workerModel != null ? _workerModel.FIO : "Не указан";

            _cargos.Clear();

            IEnumerable<Cargo> temp = await ((CargoDataService)_servicesStore.GetService<Cargo>()).GetCargosByBudAsync(ID);
            foreach (var cargo in temp)
            {
                var cargoViewModel = new CargoViewModel(cargo, _servicesStore);
                _cargos.Add(cargoViewModel);
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
        public string TimeBud => _budViewModel.TimeBud;
        public string Status => _budViewModel.Status;
        public string Description => _budViewModel.Description;
    }
}