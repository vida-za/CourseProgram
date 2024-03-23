using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using CourseProgram.Commands;
using CourseProgram.Models;
using CourseProgram.Services;
using CourseProgram.DataClasses;
using CourseProgram.Stores;
using CourseProgram.Services.DataServices;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Linq;

namespace CourseProgram.ViewModels
{
    public class DriverListingViewModel : BaseViewModel
    {
        public DriverListingViewModel(DriverDataService driverDataService, NavigationService addDriverNavigationService, NavigationStore navigationStore)
        {
            _driverDataService = driverDataService;
            _allDrivers = new ObservableCollection<DriverViewModel>();
            _freeDrivers = new ObservableCollection<DriverViewModel>();
            _disDrivers = new ObservableCollection<DriverViewModel>();
            _drivers = new ObservableCollection<DriverViewModel>();

            AddDriverCommand = new NavigateCommand(addDriverNavigationService);
            DeleteDriverCommand = new DeleteDriverCommand(this, driverDataService);
            DetailDriverCommand = new NavigateCommand(new NavigationService(navigationStore, () => SelectedDriver));
            SwitchBusyDrivers = new SwitchBusyDrivers(this);

            UpdateData();
            _drivers = _allDrivers;
        }

        public async void UpdateData()
        {
            _allDrivers.Clear();
            _freeDrivers.Clear();
            _disDrivers.Clear();

            IEnumerable<Driver> temp = await _driverDataService.GetItemsAsync();

            foreach (Driver driver in temp)
            {
                DriverViewModel driverViewModel = new(driver);
                _allDrivers.Add(driverViewModel);
            }

            temp = await _driverDataService.GetFreeDriversAsync();

            foreach (Driver driver in temp)
            {
                DriverViewModel driverViewModel = new(driver);
                _freeDrivers.Add(driverViewModel);
            }

            temp = await _driverDataService.GetDisDriversAsync();

            foreach (Driver driver in temp)
            {
                DriverViewModel driverViewModel = new(driver);
                _disDrivers.Add(driverViewModel);
            }
        }

        private readonly DriverDataService _driverDataService;

        private readonly ObservableCollection<DriverViewModel> _freeDrivers;
        public IEnumerable<DriverViewModel> FreeDrivers => _freeDrivers;


        private readonly ObservableCollection<DriverViewModel> _allDrivers;
        public IEnumerable<DriverViewModel> AllDrivers => _allDrivers;

        private readonly ObservableCollection<DriverViewModel> _disDrivers;
        public IEnumerable<DriverViewModel> DisDrivers => _disDrivers;

        private ObservableCollection<DriverViewModel> _drivers;
        public IEnumerable<DriverViewModel> Drivers
        {
            get => _drivers;
            set
            {
                _drivers = (ObservableCollection<DriverViewModel>)value;
                OnPropertyChanged(nameof(Drivers));
            }
        }

        public ICommand AddDriverCommand { get; }
        public ICommand DeleteDriverCommand { get; }
        public ICommand DetailDriverCommand { get; }
        public ICommand SwitchBusyDrivers { get; }

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

        private bool _stateCheckedBusy;
        public bool StateCheckedBusy
        {
            get => _stateCheckedBusy;
            set
            {
                _stateCheckedBusy = value;
                OnPropertyChanged(nameof(StateCheckedBusy));
            }
        }

        private bool _stateCheckedWork;
        public bool StateCheckedWork
        {
            get => _stateCheckedWork;
            set 
            { 
                _stateCheckedWork = value;
                OnPropertyChanged(nameof(StateCheckedWork));
            }
        }

        public void SwitchDrivers()
        {
            if (StateCheckedBusy)
                Drivers = new ObservableCollection<DriverViewModel>(_freeDrivers);
            else
                Drivers = new ObservableCollection<DriverViewModel>(_allDrivers);

            if (StateCheckedWork)
            {
                foreach (DriverViewModel dvm in _disDrivers)
                    _drivers.Add(dvm);
                OnPropertyChanged(nameof(Drivers));
            }
        }
    }
}