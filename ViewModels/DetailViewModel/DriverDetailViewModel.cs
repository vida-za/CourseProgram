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
    public class DriverDetailViewModel : BaseDetailViewModel
    {
        private readonly DriverViewModel _driverViewModel;
        private readonly ServicesStore _servicesStore;

        private ObservableCollection<RouteViewModel> _routes = new ObservableCollection<RouteViewModel>();
        public IEnumerable<RouteViewModel> Routes => _routes;

        public ICommand BackCommand { get; }

        public DriverDetailViewModel(ServicesStore servicesStore, SelectedStore selectedStore, INavigationService closeNavigationService)
        {
            _driverViewModel = selectedStore.CurrentDriver;
            _servicesStore = servicesStore;

            BackCommand = new NavigateCommand(closeNavigationService);

            UpdateData();
        }

        private async void UpdateData()
        {
            _routes.Clear();

            IEnumerable<Route> temp = await ((RouteDataService)_servicesStore.GetService<Route>()).GetItemsByDriverAsync(ID);

            foreach (Route route in temp)
            {
                Machine? machine = null;
                Address? addressStart = null;
                Address? addressEnd = null;

                if (route.MachineID != null) machine = await _servicesStore.GetService<Machine>().GetItemAsync((int)route.MachineID);
                if (route.AddressStartID != null) addressStart = await _servicesStore.GetService<Address>().GetItemAsync((int)route.AddressStartID);
                if (route.AddressEndID != null) addressEnd = await _servicesStore.GetService<Address>().GetItemAsync((int)route.AddressEndID);

                var routeViewModel = new RouteViewModel(route, machine, _driverViewModel.GetModel(), addressStart, addressEnd);
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