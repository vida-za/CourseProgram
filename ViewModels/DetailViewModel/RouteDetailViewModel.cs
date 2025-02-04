using CourseProgram.Commands;
using CourseProgram.Models;
using CourseProgram.Services;
using CourseProgram.Stores;
using CourseProgram.ViewModels.EntityViewModel;
using System.Collections.Generic;
using System.Windows.Input;

namespace CourseProgram.ViewModels.DetailViewModel
{
    public class RouteDetailViewModel : BaseViewModel
    {
        private readonly RouteViewModel _routeViewModel;
        private readonly ServicesStore _servicesStore;

        public IEnumerable<Machine> Machines { get; set; }
        public IEnumerable<Driver> Drivers { get; set; }
        public IEnumerable<Address> Addresses { get; set; }

        public ICommand Back { get; }
        public ICommand Save { get; }

        public RouteDetailViewModel(ServicesStore servicesStore, SelectedStore selectedStore, INavigationService closeNavigationService)
        {
            _routeViewModel = selectedStore.CurrentRoute;
            _servicesStore = servicesStore;

            Back = new NavigateCommand(closeNavigationService);
            //Save = new ..();

            UpdateData();
        }

        private Machine _selectedMachine;
        public Machine SelectedMachine
        {
            get => _selectedMachine;
            set
            {
                _selectedMachine = value;
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
                OnPropertyChanged(nameof(SelectedAddressEnd));
            }
        }

        private async void UpdateData()
        {
            Machines = await _servicesStore._machineService.GetRdyMachinesAsync();
            Drivers = await _servicesStore._driverService.GetActDriversAsync();
            Addresses = await _servicesStore._addressService.GetActAddressesAsync();

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