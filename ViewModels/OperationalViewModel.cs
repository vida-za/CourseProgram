using CourseProgram.Commands;
using CourseProgram.Models;
using CourseProgram.Services;
using CourseProgram.Stores;
using CourseProgram.ViewModels.EntityViewModel;
using CourseProgram.ViewModels.ListingViewModel;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace CourseProgram.ViewModels
{
    public class OperationalViewModel : BaseListingViewModel
    {
        #region fields
        public ICommand SelectionChangedCommand { get; }
        public ICommand DetailOrderCommand { get; }
        public ICommand DetailBudCommand { get; }
        public ICommand StartHaulCommand { get; }
        public ICommand CompleteHaulCommand { get; }
        public ICommand AddBudCommand { get; }
        public ICommand AddRouteCommand { get; }
        #endregion

        public OperationalViewModel(
            ServicesStore servicesStore,
            SelectedStore selectedStore,
            INavigationService detailOrderNavigationService,
            INavigationService detailBudNavigationService,
            INavigationService addBudNavigationService,
            INavigationService addRouteNavigationService) 
        {
            _servicesStore = servicesStore;
            _selectedStore = selectedStore;

            _orders = new ObservableCollection<OrderViewModel>();
            _buds = new ObservableCollection<BudViewModel>();

            SelectionChangedCommand = new RelayCommand<DataGrid>(SelectionChangedExecute);
            DetailOrderCommand = new NavigateDetailCommand(detailOrderNavigationService);
            DetailBudCommand = new NavigateDetailCommand(detailBudNavigationService);
            StartHaulCommand = new StartHaulCommand(this, _servicesStore);
            CompleteHaulCommand = new CompleteHaulCommand(this, _servicesStore);
            AddBudCommand = new NavigateCommand(addBudNavigationService);
            AddRouteCommand = new NavigateCommand(addRouteNavigationService);

            UpdateData();

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
            await _servicesStore._haulService.GetItemsAsync();
            Haul? temp = await _servicesStore._haulService.GetCurrentHaul();
            if (temp == null)
                Item = null;
            else
                Item = new HaulViewModel(temp);

            ObservableCollection<BudViewModel> _newBuds = new ObservableCollection<BudViewModel>();
            IEnumerable<Bud> tempBuds = await _servicesStore._budService.GetItemsAsync();
            foreach (Bud bud in tempBuds)
            {
                if (bud.Status == Constants.BudStatusValues.Waiting)
                {
                    var budViewModel = new BudViewModel(bud, _servicesStore);
                    _newBuds.Add(budViewModel);
                }
            }

            _buds.Clear();

            foreach (var model in _newBuds)
                _buds.Add(model);

            if (temp != null)
            {
                var currentSelected = SelectedOrder;

                ObservableCollection<OrderViewModel> _newOrders = new ObservableCollection<OrderViewModel>();

                IEnumerable<Order> tempOrders = await _servicesStore._orderService.GetOrdersByHaulAsync(temp.ID);
                foreach (Order order in tempOrders)
                {
                    var orderViewModel = new OrderViewModel(order, _servicesStore);
                    _newOrders.Add(orderViewModel);
                }

                _orders.Clear();

                foreach (var model in _newOrders)
                    _orders.Add(model);

                SelectedOrder = Orders.FirstOrDefault(o => o.ID == currentSelected?.ID);
            }
            else
            {
                _orders.Clear();
            }
        }

        public override async Task UpdateDataAsync()
        {
            Haul? temp = await _servicesStore._haulService.GetCurrentHaul();
            if (temp == null)
                Item = null;
            else
                Item = new HaulViewModel(temp);

            ObservableCollection<BudViewModel> _newBuds = new ObservableCollection<BudViewModel>();
            IEnumerable<Bud> tempBuds = await _servicesStore._budService.GetItemsAsync();
            foreach (Bud bud in tempBuds)
            {
                if (bud.Status == Constants.BudStatusValues.Waiting)
                {
                    var budViewModel = new BudViewModel(bud, _servicesStore);
                    _newBuds.Add(budViewModel);
                }
            }

            _buds.Clear();

            foreach (var model in _newBuds)
                _buds.Add(model);

            if (temp != null)
            {
                var currentSelected = SelectedOrder;

                ObservableCollection<OrderViewModel> _newOrders = new ObservableCollection<OrderViewModel>();
                IEnumerable<Order> tempOrders = await _servicesStore._orderService.GetOrdersByHaulAsync(temp.ID);
                foreach (Order order in tempOrders)
                {
                    var orderViewModel = new OrderViewModel(order, _servicesStore);
                    _newOrders.Add(orderViewModel);
                }

                _orders.Clear();

                foreach (var model in _newOrders)
                    _orders.Add(model);

                SelectedOrder = Orders.FirstOrDefault(o => o.ID == currentSelected?.ID);
            }
            else
            {
                _orders.Clear();
            }
        }

        protected override void Find() 
        {
            if (!string.IsNullOrEmpty(TextFilter))
                SelectedOrder = Orders.FirstOrDefault(obj => obj.ClientName.ToLower().Contains(TextFilter.ToLower()), SelectedOrder);
        }

        private void SelectionChangedExecute(DataGrid dataGrid)
        {
            if (dataGrid.SelectedItem != null)
                dataGrid.ScrollIntoView(dataGrid.SelectedItem);
        }

        private void ChangeTitle()
        {
            if (_item == null)
            {
                Title = $"Нет текущего рейса, начать?";
                StateButtonHaul = true;
            }
            else
            {
                Title = $"Выполняется рейс, начало - {Item.DateStart}";
                StateButtonHaul = false;
            }
        }
        #endregion

        #region properties
        private string _title;
        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                OnPropertyChanged(nameof(Title));
            }
        }

        private OrderViewModel _selectedOrder;
        public OrderViewModel SelectedOrder
        {
            get => _selectedOrder;
            set
            {
                _selectedOrder = value;
                _selectedStore.CurrentOrder = _selectedOrder;
                OnPropertyChanged(nameof(SelectedOrder));
            }
        }

        private BudViewModel _selectedBud;
        public BudViewModel SelectedBud
        {
            get => _selectedBud;
            set
            {
                _selectedBud = value;
                _selectedStore.CurrentBud = _selectedBud;
                OnPropertyChanged(nameof(SelectedBud));
            }
        }

        private ObservableCollection<OrderViewModel> _orders;
        public ObservableCollection<OrderViewModel> Orders
        {
            get => _orders;
            set
            {
                _orders = value;
                OnPropertyChanged(nameof(Orders));
            }
        }

        private ObservableCollection<BudViewModel> _buds;
        public ObservableCollection<BudViewModel> Buds
        {
            get => _buds;
            set
            {
                _buds = value;
                OnPropertyChanged(nameof(Buds));
            }
        }

        private HaulViewModel? _item;
        public HaulViewModel? Item
        {
            get => _item;
            set
            {
                _item = value;
                ChangeTitle();
                OnPropertyChanged(nameof(Item));
            }
        }

        private bool _stateButtonHaul;
        public bool StateButtonHaul
        {
            get => _stateButtonHaul;
            set
            {
                _stateButtonHaul = value;
                OnPropertyChanged(nameof(StateButtonHaul));
            }
        }
        #endregion
    }
}