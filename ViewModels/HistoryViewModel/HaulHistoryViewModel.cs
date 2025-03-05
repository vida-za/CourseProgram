using CourseProgram.Models;
using CourseProgram.Services.DataServices;
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

namespace CourseProgram.ViewModels.HistoryViewModel
{
    public class HaulHistoryViewModel : BaseListingViewModel
    {
        public ICommand NextHaul { get; }
        public ICommand PrevHaul { get; }

        public HaulHistoryViewModel(ServicesStore servicesStore, ControllersStore controllersStore)
        {
            _servicesStore = servicesStore;
            _controllersStore = controllersStore;

            _hauls = new ObservableCollection<HaulViewModel>();
            _orders = new ObservableCollection<OrderViewModel>();

            SelectionChangedCommand = new RelayCommand<DataGrid>(SelectionChangedExecute);
            NextHaul = new RelayCommand(SetNextHaul);
            PrevHaul = new RelayCommand(SetPrevHaul);

            UpdateData();
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
            IEnumerable<Haul> temp = await _controllersStore.GetController<Haul>().GetItems();
            foreach (var item in temp)
            {
                if (item.DateEnd != null)
                {
                    var haulViewModel = new HaulViewModel(item);
                    _hauls.Add(haulViewModel);
                }
            }

            Hauls.OrderBy(static h => h.DateEnd);
            SelectedHaul = Hauls.Last();
        }

        public override async Task UpdateDataAsync()
        {
            var currentSelected = SelectedHaul;

            ObservableCollection<HaulViewModel> _newHauls = new ObservableCollection<HaulViewModel>();

            IEnumerable<Haul> temp = await _controllersStore.GetController<Haul>().GetItems();
            foreach (var item in temp)
            {
                if (item.DateEnd != null)
                {
                    var haulViewModel = new HaulViewModel(item);
                    _newHauls.Add(haulViewModel);
                }
            }

            _hauls.Clear();

            foreach (var item in _newHauls)
                _hauls.Add(item);

            Hauls.OrderBy(static h => h.DateEnd);
            SelectedHaul = Hauls.FirstOrDefault(h => h.ID == currentSelected?.ID, Hauls.Last());
        }

        public async void UpdateOrdersAsync()
        {
            var currentSelected = SelectedOrder;

            ObservableCollection<OrderViewModel> _newOrders = new ObservableCollection<OrderViewModel>();

            IEnumerable<Order> tempOrders = await ((OrderDataService)_servicesStore.GetService<Order>()).GetOrdersByHaulAsync(SelectedHaul.ID);
            foreach (var item in tempOrders)
            {
                var orderViewModel = new OrderViewModel(item, _controllersStore);
                _newOrders.Add(orderViewModel);
            }

            _orders.Clear();

            foreach (var item in _newOrders)
                _orders.Add(item);

            SelectedOrder = Orders.FirstOrDefault(o => o.ID == currentSelected?.ID);
        }

        protected override void Find()
        {
            if (!string.IsNullOrEmpty(TextFilter))
                SelectedOrder = Orders.FirstOrDefault(obj => obj.ClientName.ToLower().Contains(TextFilter.ToLower()), SelectedOrder);
        }

        private void SetNextHaul()
        {
            if (Hauls.Count < 2 || SelectedHaul == null)
                return;

            int currentIndex = Hauls.IndexOf(SelectedHaul);
            if (currentIndex < Hauls.Count - 1)
            {
                SelectedHaul = Hauls[currentIndex + 1];
            }
        }

        private void SetPrevHaul()
        {
            if (Hauls.Count < 2 || SelectedHaul == null)
                return;

            int currentIndex = Hauls.IndexOf(SelectedHaul);
            if (currentIndex > 0)
            {
                SelectedHaul = Hauls[currentIndex - 1];
            }
        }

        private ObservableCollection<HaulViewModel> _hauls;
        public ObservableCollection<HaulViewModel> Hauls
        {
            get => _hauls;
            set
            {
                _hauls = value;
                OnPropertyChanged(nameof(Hauls));
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

        private HaulViewModel _selectedHaul;
        public HaulViewModel SelectedHaul
        {
            get => _selectedHaul;
            set
            {
                if (_selectedHaul != value && value != null)
                {
                    _selectedHaul = value;
                    UpdateOrdersAsync();
                    OnPropertyChanged(nameof(SelectedHaul));
                }
            }
        }

        private OrderViewModel _selectedOrder;
        public OrderViewModel SelectedOrder
        {
            get => _selectedOrder;
            set
            {
                _selectedOrder = value;
                OnPropertyChanged(nameof(SelectedOrder));
            }
        }
    }
}