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
    public class AddressListingViewModel : BaseListingViewModel
    {
        #region fields
        private readonly ObservableCollection<AddressViewModel> _allAddresses;

        public ICommand AddAddressCommand { get; }
        public ICommand DeleteAddressCommand { get; }
        public ICommand SelectionChangedCommand { get; }
        #endregion

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
            DeleteAddressCommand = new DeleteAddressCommand(this, _servicesStore);
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
        private async void UpdateTimer_Tick(object? sender, EventArgs e)
        {
            await UpdateDataAsync();
        }

        public override async void UpdateData()
        {
            ObservableCollection<AddressViewModel> _newAllAddresses = new ObservableCollection<AddressViewModel>();

            IEnumerable<Address> temp = await _servicesStore._addressService.GetItemsAsync();
            foreach (Address address in temp)
            {
                AddressViewModel addressViewModel = new(address);
                _newAllAddresses.Add(addressViewModel);
            }

            _allAddresses.Clear();

            foreach (AddressViewModel model in _newAllAddresses)
                _allAddresses.Add(model);
        }

        protected override void Find()
        {
            if (!string.IsNullOrEmpty(TextFilter))
                SelectedItem = Items
                    .Where(obj => obj.Street.ToLower().Contains(TextFilter.ToLower()))
                    .FirstOrDefault();
        }

        private void SelectionChangedExecute(DataGrid dataGrid)
        {
            if (dataGrid.SelectedItem != null)
                dataGrid.ScrollIntoView(dataGrid.SelectedItem);
        }

        public override async Task UpdateDataAsync()
        {
            var currentSelected = SelectedItem;

            ObservableCollection<AddressViewModel> _newAllAddresses = new ObservableCollection<AddressViewModel>();

            IEnumerable<Address> temp = await _servicesStore._addressService.GetItemsAsync();
            foreach (Address address in temp)
            {
                AddressViewModel addressViewModel = new(address);
                _newAllAddresses.Add(addressViewModel);
            }

            _allAddresses.Clear();

            foreach (AddressViewModel model in _newAllAddresses)
                _allAddresses.Add(model);

            SelectedItem = Items.FirstOrDefault(a => a.ID == currentSelected?.ID);
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