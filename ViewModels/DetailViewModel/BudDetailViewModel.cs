using CourseProgram.Commands;
using CourseProgram.Models;
using CourseProgram.Services;
using CourseProgram.Stores;
using CourseProgram.ViewModels.EntityViewModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace CourseProgram.ViewModels.DetailViewModel
{
    public class BudDetailViewModel : BaseViewModel
    {
        private readonly BudViewModel _budViewModel;
        private readonly ServicesStore _servicesStore;

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

            AcceptBud = new UpdateBudCommand(_servicesStore, _budViewModel, true, closeNavigationService);
            CancelBud = new UpdateBudCommand(_servicesStore, _budViewModel, false, closeNavigationService);
            Back = new NavigateCommand(closeNavigationService);

            UpdateData();
        }

        private async void UpdateData()
        {
            _cargos.Clear();

            IEnumerable<Cargo> temp = await _servicesStore._cargoService.GetCargosByBudAsync(ID);
            foreach (var cargo in temp)
            {
                var cargoViewModel = new CargoViewModel(cargo, _servicesStore);
                _cargos.Add(cargoViewModel);
            }
        }

        public string ClientName => _budViewModel.ClientName;
        public string WorkerName => _budViewModel.WorkerName;
        public string TimeBud => _budViewModel.TimeBud;
        public string Status => _budViewModel.Status;
        public string Description => _budViewModel.Description;
    }
}