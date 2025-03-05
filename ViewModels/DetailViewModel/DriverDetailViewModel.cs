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
    public class DriverDetailViewModel : BaseDetailViewModel
    {
        private readonly DriverViewModel _driverViewModel;
        private readonly ControllersStore _controllersStore;

        private ObservableCollection<RouteViewModel> _routes = new ObservableCollection<RouteViewModel>();
        public IEnumerable<RouteViewModel> Routes => _routes;

        public ICommand BackCommand { get; }

        public DriverDetailViewModel(SelectedStore selectedStore, ControllersStore controllersStore, INavigationService closeNavigationService)
        {
            _driverViewModel = selectedStore.CurrentDriver;
            _controllersStore = controllersStore;

            BackCommand = new NavigateCommand(closeNavigationService);

            UpdateData();
        }

        private async void UpdateData()
        {
            _routes.Clear();

            IEnumerable<Route> temp = await ((RouteDataController)_controllersStore.GetController<Route>()).GetRoutesByDriver(ID);

            foreach (Route route in temp)
            {
                var routeViewModel = new RouteViewModel(route, _controllersStore);
                _routes.Add(routeViewModel);
            }
        }

        public int ID => _driverViewModel.ID;
        public string FIO => _driverViewModel.FIO;
        public string BirthDay => _driverViewModel.BirthDay;
        public string Passport => _driverViewModel.Passport;
        public string Phone => _driverViewModel.Phone;
        public string DateStart => _driverViewModel.DateStart;
        public string DateEnd => _driverViewModel.DateEnd;
        public string StringCategories => _driverViewModel.StringCategories;
    }
}