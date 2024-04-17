using CourseProgram.Commands;
using CourseProgram.Models;
using CourseProgram.Services;
using CourseProgram.Stores;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace CourseProgram.ViewModels
{
    public class MachineDetailViewModel : BaseViewModel
    {
        private readonly MachineViewModel _machinevViewModel;
        private readonly ServicesStore _servicesStore;

        private readonly ObservableCollection<RouteViewModel> _routes = new ObservableCollection<RouteViewModel>();
        public IEnumerable<RouteViewModel> Routes => _routes;

        public ICommand BackCommand { get; }

        public MachineDetailViewModel(ServicesStore servicesStore, SelectedStore selectedStore, INavigationService closeNavigationService)
        {
            _machinevViewModel = selectedStore.CurrentMachine;
            _servicesStore = servicesStore;

            BackCommand = new NavigateCommand(closeNavigationService);

            UpdateData();
        }

        private async void UpdateData()
        {
            _routes.Clear();

            IEnumerable<Route> temp = await _servicesStore._routeService.GetItemsByMachineAsync(ID);

            foreach (Route route in temp)
            {
                RouteViewModel routeViewModel = new(route, _servicesStore);
                _routes.Add(routeViewModel);
            }
        }

        public int ID => _machinevViewModel.ID;
        public string TypeMachine => _machinevViewModel.TypeMachine;
        public string TypeBodywork => _machinevViewModel.TypeBodywork;
        public string TypeLoading => _machinevViewModel.TypeLoading;
        public string LoadCapacity => _machinevViewModel.LoadCapacity.ToString();
        public string Volume => _machinevViewModel.Volume.ToString();
        public bool HydroBoard => _machinevViewModel.HydroBoard;
        public string LengthBodywork => _machinevViewModel.LengthBodywork.ToString();
        public string WidthBodywork => _machinevViewModel.WidthBodywork.ToString();
        public string HeightBodywork => _machinevViewModel.HeightBodywork.ToString();
        public string Stamp => _machinevViewModel.Stamp;
        public string Name => _machinevViewModel.Name;
        public string StateNumber => _machinevViewModel.StateNumber;
        public string Status => _machinevViewModel.Status;
        public string TimeStart => _machinevViewModel.TimeStart.ToString();
        public string TimeEnd => _machinevViewModel.TimeEnd.ToString();
        public string Town => _machinevViewModel.Town;

    }
}