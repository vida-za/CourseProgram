using CourseProgram.Commands;
using CourseProgram.Commands.DeleteCommands;
using CourseProgram.Models;
using CourseProgram.Services;
using CourseProgram.Stores;
using CourseProgram.ViewModels.EntityViewModel;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace CourseProgram.ViewModels.ListingViewModel
{
    public class ClientListingViewModel : BaseListingViewModel
    {
        #region fields
        private readonly ServicesStore _servicesStore;
        private readonly SelectedStore _selectedStore;

        private readonly DispatcherTimer updateTimer;

        private readonly ObservableCollection<ClientViewModel> _allClients;
        private readonly ObservableCollection<ClientViewModel> _disClients;

        public ICommand AddClientCommand { get; }
        public ICommand DeleteClientCommand { get; }
        public ICommand DetailClientCommand { get; }
        public ICommand SelectionChangedCommand { get; }
        public ICommand SwitchHandlerCommand { get; }
        #endregion

        public ClientListingViewModel(
            ServicesStore servicesStore,
            SelectedStore selectedStore,
            INavigationService addClientNavigationService,
            INavigationService detailClientNavigationService) 
        {
            _servicesStore = servicesStore;
            _selectedStore = selectedStore;

            _allClients = new ObservableCollection<ClientViewModel>();
            _disClients = new ObservableCollection<ClientViewModel>();

            AddClientCommand = new NavigateCommand(addClientNavigationService);
            DeleteClientCommand = new DeleteClientCommand(this, servicesStore._clientService);
            DetailClientCommand = new NavigateDetailCommand(detailClientNavigationService);
            SelectionChangedCommand = new RelayCommand<DataGrid>(SelectionChangedExecute);
            SwitchHandlerCommand = new SwitchHandlerCommand(this);

            UpdateData();
            _items = _allClients;

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
            _allClients.Clear();
            _disClients.Clear();

            IEnumerable<Client> temp = await _servicesStore._clientService.GetItemsAsync();
            foreach (Client client in temp)
            {
                ClientViewModel clientViewModel = new(client);
                _allClients.Add(clientViewModel);
            }

            temp = await _servicesStore._clientService.GetItemsAsync();
            foreach (Client client in temp)
            {
                ClientViewModel clientViewModel = new(client);
                _disClients.Add(clientViewModel);
            }
        }

        protected override void Find()
        {
            if (!string.IsNullOrEmpty(TextFilter))
                SelectedItem = Items.FirstOrDefault(obj => obj.Name.ToLower().Contains(TextFilter.ToLower()), SelectedItem);
        }

        private void SelectionChangedExecute(DataGrid dataGrid)
        {
            if (dataGrid.SelectedItem != null)
                dataGrid.ScrollIntoView(dataGrid.SelectedItem);
        }
        #endregion

        #region properties
        private ObservableCollection<ClientViewModel> _items;
        public ObservableCollection<ClientViewModel> Items
        {
            get => _items;
            set
            {
                _items = value;
                OnPropertyChanged(nameof(Items));
            }
        }

        private ClientViewModel _selectedItem;
        public ClientViewModel SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                _selectedStore.CurrentClient = value;
                OnPropertyChanged(nameof(SelectedItem));
            }
        }
        #endregion
    }
}