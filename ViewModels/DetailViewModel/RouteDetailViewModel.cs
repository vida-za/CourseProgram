using CourseProgram.Commands;
using CourseProgram.Models;
using CourseProgram.Services;
using CourseProgram.Services.DataServices;
using CourseProgram.Stores;
using CourseProgram.ViewModels.EntityViewModel;
using System.Collections.Generic;
using System.Windows.Input;

namespace CourseProgram.ViewModels.DetailViewModel
{
    public class RouteDetailViewModel : BaseDetailViewModel
    {
        private readonly RouteViewModel _routeViewModel;
        private readonly ServicesStore _servicesStore;
        private readonly Route _base;

        public IEnumerable<Machine> Machines { get; set; }
        public IEnumerable<Driver> Drivers { get; set; }
        public IEnumerable<Address> Addresses { get; set; }

        public ICommand Back { get; }
        public ICommand Save { get; }

        public RouteDetailViewModel(ServicesStore servicesStore, SelectedStore selectedStore, INavigationService closeNavigationService)
        {
            _routeViewModel = selectedStore.CurrentRoute;
            _base = _routeViewModel.GetModel();
            _servicesStore = servicesStore;

            SelectedMachine = _routeViewModel.GetMachine();
            SelectedDriver = _routeViewModel.GetDriver();
            SelectedAddressStart = _routeViewModel.GetAddressStart();
            SelectedAddressEnd = _routeViewModel.GetAddressEnd();
            IsDirty = false;

            Back = new NavigateCommand(closeNavigationService);
            Save = new UpdateEntityCommand<Route>(this, _servicesStore.GetService<Route>(), GetUpdatedRoute, closeNavigationService, () => IsDirty, () => IsDirty = false);

            UpdateData();
        }

        private Route GetUpdatedRoute() => new Route(_base.ID, SelectedMachine.ID, SelectedDriver.ID, _base.Type, _base.Status, _base.CompleteTime, SelectedAddressStart.ID, SelectedAddressEnd.ID);

        private Machine _selectedMachine;
        public Machine SelectedMachine
        {
            get => _selectedMachine;
            set
            {
                _selectedMachine = value;
                IsDirty = true;
                OnPropertyChanged(nameof(SelectedMachine));
            }
        }

        private Driver _selectedDriver;
        public Driver SelectedDriver
        {
            get => _selectedDriver;
            set
            {
                _selectedDriver = value;
                IsDirty = true;
                OnPropertyChanged(nameof(SelectedDriver));
            }
        }

        private Address _selectedAddressStart;
        public Address SelectedAddressStart
        {
            get => _selectedAddressStart;
            set
            {
                _selectedAddressStart = value;
                IsDirty = true;
                OnPropertyChanged(nameof(SelectedAddressStart));
            }
        }

        private Address _selectedAddressEnd;
        public Address SelectedAddressEnd
        {
            get => _selectedAddressEnd;
            set
            {
                _selectedAddressEnd = value;
                IsDirty = true;
                OnPropertyChanged(nameof(SelectedAddressEnd));
            }
        }

        private async void UpdateData()
        {
            Machines = await ((MachineDataService)_servicesStore.GetService<Machine>()).GetRdyMachinesAsync();
            Drivers = await ((DriverDataService)_servicesStore.GetService<Driver>()).GetActDriversAsync();
            Addresses = await ((AddressDataService)_servicesStore.GetService<Address>()).GetActAddressesAsync();

            OnPropertyChanged(nameof(Machines));
            OnPropertyChanged(nameof(Drivers));
            OnPropertyChanged(nameof(Addresses));
        }

        public string MachineName => _routeViewModel.MachineName;
        public string DriverName => _routeViewModel.DriverName;
        public string AddressStart => _routeViewModel.AddressStart;
        public string AddressEnd => _routeViewModel.AddressEnd;
        public string Type => _routeViewModel.Type;
        public string Status => _routeViewModel.Status;
        public string CompleteTime => _routeViewModel.CompleteTime;
    }
}