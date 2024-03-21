using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using CourseProgram.Commands;
using CourseProgram.Models;
using CourseProgram.Services;
using CourseProgram.DataClasses;
using CourseProgram.Stores;
using CourseProgram.Services.DataServices;

namespace CourseProgram.ViewModels
{
    public class DriverListingViewModel : BaseViewModel
    {
        public DriverListingViewModel(DriverDataService driverDataService, NavigationService addDriverNavigationService, NavigationStore navigationStore)
        {
            _driverDataService = driverDataService;
            _drivers = new ObservableCollection<DriverViewModel>();

            AddDriverCommand = new NavigateCommand(addDriverNavigationService);
            DeleteDriverCommand = new DeleteDriverCommand(this, driverDataService);
            DetailDriverCommand = new NavigateCommand(new NavigationService(navigationStore, () => SelectedDriver));

            UpdateDrivers();
        }

        public async void UpdateDrivers()
        {
            _drivers.Clear();

            IEnumerable<Driver> temp = await _driverDataService.GetItemsAsync();

            foreach (Driver driver in temp)
            {
                DriverViewModel driverViewModel = new(driver);
                _drivers.Add(driverViewModel);
            }
        }

        private readonly ObservableCollection<DriverViewModel> _drivers;
        private readonly DriverDataService _driverDataService;

        public IEnumerable<DriverViewModel> Drivers => _drivers;

        public ICommand AddDriverCommand { get; }
        public ICommand DeleteDriverCommand { get; }
        public ICommand DetailDriverCommand { get; }

        private DriverViewModel _selectedDriver;
        public DriverViewModel SelectedDriver
        {
            get => _selectedDriver;
            set
            {
                _selectedDriver = value;
                OnPropertyChanged(nameof(SelectedDriver));
            }
        }
    }
}