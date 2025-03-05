using CourseProgram.Commands;
using CourseProgram.Models;
using CourseProgram.Services;
using CourseProgram.Stores;
using CourseProgram.ViewModels.EntityViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;

namespace CourseProgram.ViewModels.ListingViewModel
{
    public class RouteListingViewModel : BaseListingViewModel
    {
        public ICommand CompleteRoute { get; }
        public ICommand StartRoute { get; }
        public ICommand CancelRoute { get; }
        public ICommand DetailRoute { get; }

        public RouteListingViewModel(ServicesStore servicesStore, SelectedStore selectedStore, ControllersStore controllersStore, INavigationService detailRouteNavigationService)
        { 
            _selectedStore = selectedStore;
            _controllersStore = controllersStore;

            _progRoutes = new ObservableCollection<RouteViewModel>();
            _waitRoutes = new ObservableCollection<RouteViewModel>();

            CompleteRoute = new CompleteRouteCommand(servicesStore, this);
            StartRoute = new StartRouteCommand(servicesStore, controllersStore, this);
            CancelRoute = new CancelRouteCommand(servicesStore, this);
            DetailRoute = new NavigateDetailCommand(detailRouteNavigationService);

            UpdateData();

            updateTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(10)
            };
            updateTimer.Tick += UpdateTimer_Tick;
            updateTimer.Start();
        }

        private async void UpdateTimer_Tick(object? sender, EventArgs e)
        {
            await UpdateDataAsync();
        }

        private RouteViewModel _selectedProgRoute;
        public RouteViewModel SelectedProgRoute
        {
            get => _selectedProgRoute;
            set
            {
                if (_selectedProgRoute == value || value == null)
                {
                    _selectedProgRoute = null;
                }
                else
                {
                    _selectedProgRoute = value;
                    _selectedStore.CurrentRoute = _selectedProgRoute;
                    SelectedWaitRoute = null;
                }
                OnPropertyChanged(nameof(SelectedProgRoute));
            }
        }

        private RouteViewModel _selectedWaitRoute;
        public RouteViewModel SelectedWaitRoute
        {
            get => _selectedWaitRoute;
            set
            {
                if (_selectedWaitRoute == value || value == null)
                {
                    _selectedWaitRoute = null;
                }
                else
                {
                    _selectedWaitRoute = value;
                    _selectedStore.CurrentRoute = _selectedWaitRoute;
                    SelectedProgRoute = null;
                }
                OnPropertyChanged(nameof(SelectedWaitRoute));
            }
        }

        private ObservableCollection<RouteViewModel> _progRoutes;
        public ObservableCollection<RouteViewModel> ProgRoutes
        {
            get => _progRoutes;
            set
            {
                _progRoutes = value;
                OnPropertyChanged(nameof(ProgRoutes));
            }
        }

        private ObservableCollection<RouteViewModel> _waitRoutes;
        public ObservableCollection<RouteViewModel> WaitRoutes
        {
            get => _waitRoutes;
            set
            {
                _waitRoutes = value;
                OnPropertyChanged(nameof(WaitRoutes));
            }
        }

        protected override void Find() { }

        public override async void UpdateData()
        {
            IEnumerable<Route> temp = await _controllersStore.GetController<Route>().GetItems();
            foreach (var route in temp)
            {
                if (route.Status == Constants.RouteStatusValues.InProgress || route.Status == Constants.RouteStatusValues.Waiting)
                {
                    if (route.Status == Constants.RouteStatusValues.InProgress)
                    {
                        var routeViewModel = new RouteViewModel(route, _controllersStore);
                        _progRoutes.Add(routeViewModel);
                    }
                    else if (route.Status == Constants.RouteStatusValues.Waiting)
                    {
                        var routeViewModel = new RouteViewModel(route, _controllersStore);
                        _waitRoutes.Add(routeViewModel);
                    }
                }
            }
        }

        public override async Task UpdateDataAsync()
        {
            var currentSelectedProg = SelectedProgRoute;
            var currentSelectedWait = SelectedWaitRoute;

            ObservableCollection<RouteViewModel> _newProgRoutes = new ObservableCollection<RouteViewModel>();
            ObservableCollection<RouteViewModel> _newWaitRoutes = new ObservableCollection<RouteViewModel>();

            IEnumerable<Route> temp = await _controllersStore.GetController<Route>().GetItems();
            foreach (var route in temp)
            {
                if (route.Status == Constants.RouteStatusValues.InProgress || route.Status == Constants.RouteStatusValues.Waiting)
                {
                    if (route.Status == Constants.RouteStatusValues.InProgress)
                    {
                        var routeViewModel = new RouteViewModel(route, _controllersStore);
                        _newProgRoutes.Add(routeViewModel);
                    }
                    else if (route.Status == Constants.RouteStatusValues.Waiting)
                    {
                        var routeViewModel = new RouteViewModel(route, _controllersStore);
                        _newWaitRoutes.Add(routeViewModel);
                    }
                }
            }

            _progRoutes.Clear();
            _waitRoutes.Clear();

            foreach (var model in _newProgRoutes)
                _progRoutes.Add(model);
            foreach (var model in _newWaitRoutes)
                _waitRoutes.Add(model);

            SelectedProgRoute = ProgRoutes.FirstOrDefault(r => r.ID == currentSelectedProg?.ID, null);
            SelectedWaitRoute = WaitRoutes.FirstOrDefault(r => r.ID == currentSelectedWait?.ID, null);
        }
    }
}