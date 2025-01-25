using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
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
            _disDrivers = new ObservableCollection<DriverViewModel>();
            _items = new ObservableCollection<DriverViewModel>();

            AddDriverCommand = new NavigateCommand(addDriverNavigationService);
            DeleteDriverCommand = new DeleteDriverCommand(this, _servicesStore);
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

        private async void UpdateTimer_Tick(object? sender, EventArgs e)
        {
            await UpdateDataAsync();
        }

        public override async void UpdateData()
        {
            ObservableCollection<DriverViewModel> _newAllDrivers = new ObservableCollection<DriverViewModel>();
            ObservableCollection<DriverViewModel> _newDisDrivers = new ObservableCollection<DriverViewModel>();

            IEnumerable<Driver> temp = await _servicesStore._driverService.GetItemsAsync();
            foreach (Driver itemTemp in temp)
            {
                DriverViewModel driverViewModel = new(itemTemp);
                _newAllDrivers.Add(driverViewModel);
            }

            temp = await _servicesStore._driverService.GetDisDriversAsync();
            foreach (Driver itemTemp in temp)
            {
                DriverViewModel driverViewModel = new(itemTemp);
                _newDisDrivers.Add(driverViewModel);
            }

            _allDrivers.Clear();
            _disDrivers.Clear();

            foreach (DriverViewModel model in  _newAllDrivers)
                _allDrivers.Add(model);
            foreach (DriverViewModel model in _newDisDrivers)
                _disDrivers.Add(model);
        }

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
                SelectedItem = Items.FirstOrDefault(obj => obj.FIO.ToLower().Contains(TextFilter.ToLower()), SelectedItem);
        }

        private void SelectionChangedExecute(DataGrid dataGrid)
        {
            if (dataGrid.SelectedItem != null)
                dataGrid.ScrollIntoView(dataGrid.SelectedItem);
        }

        public override async Task UpdateDataAsync()
        {
            var currentSelected = SelectedItem;

            ObservableCollection<DriverViewModel> _newAllDrivers = new ObservableCollection<DriverViewModel>();
            ObservableCollection<DriverViewModel> _newDisDrivers = new ObservableCollection<DriverViewModel>();

            IEnumerable<Driver> temp = await _servicesStore._driverService.GetItemsAsync();
            foreach (Driver itemTemp in temp)
            {
                DriverViewModel driverViewModel = new(itemTemp);
                _newAllDrivers.Add(driverViewModel);
            }

            temp = await _servicesStore._driverService.GetDisDriversAsync();
            foreach (Driver itemTemp in temp)
            {
                DriverViewModel driverViewModel = new(itemTemp);
                _newDisDrivers.Add(driverViewModel);
            }

            _allDrivers.Clear();
            _disDrivers.Clear();

            foreach (DriverViewModel model in _newAllDrivers)
                _allDrivers.Add(model);
            foreach (DriverViewModel model in _newDisDrivers)
                _disDrivers.Add(model);

            SelectedItem = Items.FirstOrDefault(d => d.ID == currentSelected?.ID);
        }
    }
}