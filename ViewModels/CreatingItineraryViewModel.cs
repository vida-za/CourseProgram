using CourseProgram.Commands;
using CourseProgram.Controllers.DataControllers.EntityDataControllers;
using CourseProgram.Models;
using CourseProgram.Services;
using CourseProgram.Stores;
using CourseProgram.ViewModels.EntityViewModel;
using GalaSoft.MvvmLight.Command;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace CourseProgram.ViewModels
{
    public class CreatingItineraryViewModel : BaseViewModel
    {
        private readonly ControllersStore _controllersStore;
        private readonly SelectedStore _selectedStore;

        public ICommand DetailRoute { get; }
        public ICommand CreateItinerary { get; }
        public ICommand RouteUp { get; }
        public ICommand RouteDown { get; }

        public CreatingItineraryViewModel(ControllersStore controllersStore, SelectedStore selectedStore, INavigationService detailRouteNavigationService)
        {
            _controllersStore = controllersStore;
            _selectedStore = selectedStore;

            _routes = new ObservableCollection<RouteViewModel>();
            _drivers = new ObservableCollection<DriverViewModel>();

            DetailRoute = new NavigateDetailCommand(detailRouteNavigationService);
            CreateItinerary = new CreateItineraryCommand(this);
            RouteUp = new RelayCommand<RouteViewModel>(MoveUp);
            RouteDown = new RelayCommand<RouteViewModel>(MoveDown);

            UpdateData();
        }

        private async void UpdateData()
        {
            IEnumerable<Driver> temp = await ((DriverDataController)_controllersStore.GetController<Driver>()).GetDriversHasRoutes();
            foreach (var item in temp)
            {
                var driverViewModel = new DriverViewModel(item);
                _drivers.Add(driverViewModel);
            }
        }

        private async void UpdateRoutes()
        {
            ObservableCollection<RouteViewModel> _newRoutes = new ObservableCollection<RouteViewModel>();

            IEnumerable<Route> temp = await ((RouteDataController)_controllersStore.GetController<Route>()).GetRoutesByDriver(SelectedDriver.ID);
            foreach (var item in temp)
            {
                if (item.Status == Constants.RouteStatusValues.Waiting)
                {
                    var routeViewModel = new RouteViewModel(item, _controllersStore);
                    _newRoutes.Add(routeViewModel);
                }
            }

            _routes.Clear();

            foreach (var item in _newRoutes)
                _routes.Add(item);
        }

        private void MoveUp(RouteViewModel route)
        {
            if (route == null)
                return;

            int index = Routes.IndexOf(route);
            if (index > 0)
            {
                Routes.Move(index, index - 1);
            }
        }

        private void MoveDown(RouteViewModel route)
        {
            if (route == null)
                return;

            int index = Routes.IndexOf(route);
            if (index < Routes.Count - 1)
            {
                Routes.Move(index, index + 1);
            }
        }

        private ObservableCollection<RouteViewModel> _routes;
        public ObservableCollection<RouteViewModel> Routes
        {
            get => _routes;
            set
            {
                _routes = value;
                OnPropertyChanged(nameof(Routes));
            }
        }

        private ObservableCollection<DriverViewModel> _drivers;
        public ObservableCollection<DriverViewModel> Drivers
        {
            get => _drivers;
            set
            {
                _drivers = value;
                OnPropertyChanged(nameof(Drivers));
            }
        }

        private RouteViewModel _selectedRoute;
        public RouteViewModel SelectedRoute
        {
            get => _selectedRoute;
            set
            {
                _selectedRoute = value;
                _selectedStore.CurrentRoute = _selectedRoute;
                OnPropertyChanged(nameof(SelectedRoute));
            }
        }

        private DriverViewModel _selectedDriver;
        public DriverViewModel SelectedDriver
        {
            get => _selectedDriver;
            set
            {
                if (_selectedDriver != value && value != null)
                {
                    _selectedDriver = value;
                    UpdateRoutes();
                    OnPropertyChanged(nameof(SelectedDriver));
                }
            }
        }
    }
}