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
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace CourseProgram.ViewModels.ListingViewModel
{
    public class ClientListingViewModel : BaseListingViewModel
    {
        #region fields
        private readonly ObservableCollection<ClientViewModel> _allClients;

        public ICommand AddClientCommand { get; }
        public ICommand DeleteClientCommand { get; }
        public ICommand DetailClientCommand { get; }
        public ICommand SwitchHandlerCommand { get; }
        #endregion

        public ClientListingViewModel(
            ServicesStore servicesStore,
            SelectedStore selectedStore,
            ControllersStore controllersStore,
            INavigationService addClientNavigationService,
            INavigationService detailClientNavigationService) 
        {
            _selectedStore = selectedStore;
            _controllersStore = controllersStore;

            _allClients = new ObservableCollection<ClientViewModel>();

            AddClientCommand = new NavigateCommand(addClientNavigationService);
            DeleteClientCommand = new DeleteClientCommand(this, servicesStore);
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
        private async void UpdateTimer_Tick(object? sender, EventArgs e)
        {
            await UpdateDataAsync();
        }

        public override async void UpdateData()
        {
            ObservableCollection<ClientViewModel> _newAllClients = new ObservableCollection<ClientViewModel>();

            IEnumerable<Client> temp = await _controllersStore.GetController<Client>().GetItems();
            foreach (Client itemTemp in temp)
            {
                ClientViewModel clientViewModel = new(itemTemp);
                _newAllClients.Add(clientViewModel);
            }

            _allClients.Clear();

            foreach (ClientViewModel model in  _newAllClients)
                _allClients.Add(model);
        }

        protected override void Find()
        {
            if (!string.IsNullOrEmpty(TextFilter))
                SelectedItem = Items.FirstOrDefault(obj => obj.Name.ToLower().Contains(TextFilter.ToLower()), SelectedItem);
        }

        public override async Task UpdateDataAsync()
        {
            var currentSelected = SelectedItem;

            ObservableCollection<ClientViewModel> _newAllClients = new ObservableCollection<ClientViewModel>();

            IEnumerable<Client> temp = await _controllersStore.GetController<Client>().GetItems();
            foreach (Client itemTemp in temp)
            {
                ClientViewModel clientViewModel = new(itemTemp);
                _newAllClients.Add(clientViewModel);
            }

            _allClients.Clear();

            foreach (ClientViewModel model in _newAllClients)
                _allClients.Add(model);

            SelectedItem = Items.FirstOrDefault(i => i.ID == currentSelected?.ID);
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