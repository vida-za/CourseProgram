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
    public class AddressListingViewModel : BaseListingViewModel
    {
        private readonly ServicesStore _servicesStore;
        private readonly SelectedStore _selectedStore;

        private readonly DispatcherTimer updateTimer;

        private readonly ObservableCollection<AddressViewModel> _allAddresses;

        public ICommand AddAddressCommand { get; }
        public ICommand DeleteAddressCommand { get; }
        public ICommand SelectionChangedCommand { get; }

        public AddressListingViewModel(
            ServicesStore servicesStore,
            SelectedStore selectedStore,
            INavigationService addAddressNavigationService)
        {
            _servicesStore = servicesStore;
            _selectedStore = selectedStore;

            _allAddresses = new ObservableCollection<AddressViewModel>();
            _items = new ObservableCollection<AddressViewModel>();

            AddAddressCommand = new NavigateCommand(addAddressNavigationService);
            DeleteAddressCommand = new DeleteAddressCommand(this, _servicesStore._addressService);
            SelectionChangedCommand = new RelayCommand<DataGrid>(SelectionChangedExecute);

            UpdateData();
            _items = _allAddresses;

            updateTimer = new()
            {
                Interval = TimeSpan.FromSeconds(3)
            };
            updateTimer.Tick += UpdateTimer_Tick;
            updateTimer.Start();
        }

        #region methods
        private void UpdateTimer_Tick(object? sender, EventArgs e)
        {
            UpdateData();
        }

        public override async void UpdateData()
        {
            _allAddresses.Clear();

            IEnumerable<Address> temp = await _servicesStore._addressService.GetItemsAsync();
            foreach (Address address in temp)
            {
                AddressViewModel addressViewModel = new(address);
                _allAddresses.Add(addressViewModel);
            }
        }

        protected override void Find()
        {
            if (!string.IsNullOrEmpty(TextFilter))
                SelectedItem = Items.FirstOrDefault(obj => obj.Street.Contains(TextFilter), SelectedItem);
        }

        private void SelectionChangedExecute(DataGrid dataGrid)
        {
            if (dataGrid.SelectedItem != null)
                dataGrid.ScrollIntoView(dataGrid.SelectedItem);
        }
        #endregion

        #region properties
        private ObservableCollection<AddressViewModel> _items;
        public ObservableCollection<AddressViewModel> Items
        {
            get => _items;
            set
            {
                _items = value;
                OnPropertyChanged(nameof(Items));
            }
        }

        private AddressViewModel _selectedItem;
        public AddressViewModel SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                _selectedStore.CurrentAddress = value;
                OnPropertyChanged(nameof(SelectedItem));
            }
        }
        #endregion
    }
}