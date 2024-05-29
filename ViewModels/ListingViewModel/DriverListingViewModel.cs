using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using CourseProgram.Commands;
using CourseProgram.Commands.DeleteCommands;
using CourseProgram.Models;
using CourseProgram.Services;
using CourseProgram.Stores;
using CourseProgram.ViewModels.EntityViewModel;
using GalaSoft.MvvmLight.Command;

namespace CourseProgram.ViewModels.ListingViewModel
{
    public class DriverListingViewModel : BaseListingViewModel
    {
        public DriverListingViewModel(
            ServicesStore servicesStore,
            SelectedStore selectedStore,
            INavigationService addDriverNavigationService,
            INavigationService detailDriverNavigationService)
        {
            _servicesStore = servicesStore;
            _selectedStore = selectedStore;
            _allDrivers = new ObservableCollection<DriverViewModel>();
            _freeDrivers = new ObservableCollection<DriverViewModel>();
            _disDrivers = new ObservableCollection<DriverViewModel>();
            _items = new ObservableCollection<DriverViewModel>();

            AddDriverCommand = new NavigateCommand(addDriverNavigationService);
            DeleteDriverCommand = new DeleteDriverCommand(this, _servicesStore._driverService);
            DetailDriverCommand = new NavigateDetailCommand(detailDriverNavigationService);
            SwitchBusyDrivers = new SwitchHandlerCommand(this);
            SelectionChangedCommand = new RelayCommand<DataGrid>(SelectionChangedExecute);

            UpdateData();
            _items = _allDrivers;

            updateTimer = new()
            {
                Interval = TimeSpan.FromSeconds(3)
            };
            updateTimer.Tick += UpdateTimer_Tick;
            updateTimer.Start();
        }

        private void UpdateTimer_Tick(object? sender, EventArgs e)
        {
            UpdateData();
        }

        public override async void UpdateData()
        {
            _allDrivers.Clear();
            _freeDrivers.Clear();
            _disDrivers.Clear();

            IEnumerable<Driver> temp = await _servicesStore._driverService.GetItemsAsync();

            foreach (Driver driver in temp)
            {
                DriverViewModel driverViewModel = new(driver);
                _allDrivers.Add(driverViewModel);
            }

            temp = await _servicesStore._driverService.GetFreeDriversAsync();

            foreach (Driver driver in temp)
            {
                DriverViewModel driverViewModel = new(driver);
                _freeDrivers.Add(driverViewModel);
            }

            temp = await _servicesStore._driverService.GetDisDriversAsync();

            foreach (Driver driver in temp)
            {
                DriverViewModel driverViewModel = new(driver);
                _disDrivers.Add(driverViewModel);
            }
        }

        private readonly ServicesStore _servicesStore;
        private readonly SelectedStore _selectedStore;

        private readonly DispatcherTimer updateTimer;

        private readonly ObservableCollection<DriverViewModel> _freeDrivers;
        private readonly ObservableCollection<DriverViewModel> _allDrivers;
        private readonly ObservableCollection<DriverViewModel> _disDrivers;

        private ObservableCollection<DriverViewModel> _items;
        public ObservableCollection<DriverViewModel> Items
        {
            get => _items;
            set
            {
                _items = value;
                OnPropertyChanged(nameof(Items));
            }
        }

        public ICommand AddDriverCommand { get; }
        public ICommand DeleteDriverCommand { get; }
        public ICommand DetailDriverCommand { get; }
        public ICommand SwitchBusyDrivers { get; }
        public ICommand SelectionChangedCommand { get; }

        private DriverViewModel _selectedItem;
        public DriverViewModel SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                _selectedStore.CurrentDriver = _selectedItem;
                OnPropertyChanged(nameof(SelectedItem));
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

        public override void SwitchHandler()
        {
            if (StateCheckedBusy)
                Items = new ObservableCollection<DriverViewModel>(_freeDrivers);
            else
                Items = new ObservableCollection<DriverViewModel>(_allDrivers);

            if (StateCheckedWork)
            {
                foreach (DriverViewModel dvm in _disDrivers)
                    Items.Add(dvm);
                OnPropertyChanged(nameof(Items));
            }
        }

        protected override void Find()
        {
            if (!string.IsNullOrEmpty(TextFilter))
                SelectedItem = Items.FirstOrDefault(obj => obj.FIO.Contains(TextFilter), SelectedItem);
        }

        private void SelectionChangedExecute(DataGrid dataGrid)
        {
            if (dataGrid.SelectedItem != null)
                dataGrid.ScrollIntoView(dataGrid.SelectedItem);
        }
    }
}