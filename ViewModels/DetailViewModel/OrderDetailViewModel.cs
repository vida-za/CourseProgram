using CourseProgram.Commands;
using CourseProgram.Models;
using CourseProgram.Services;
using CourseProgram.Services.DataServices;
using CourseProgram.Stores;
using CourseProgram.ViewModels.EntityViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace CourseProgram.ViewModels.DetailViewModel
{
    public class OrderDetailViewModel : BaseDetailViewModel
    {
        private readonly ServicesStore _servicesStore;
        private readonly ControllersStore _controllersStore;
        private readonly OrderViewModel _orderViewModel;
        private readonly Order _base;
        private float? fPrice;

        public EditableTextFieldViewModel PriceField { get; }

        public ICommand Back { get; }
        public ICommand SaveChanges { get; }
        public ICommand CancelOrder { get; }
        public ICommand CompleteOrder { get; }
        public ICommand SetTimeLoad { get; }
        public ICommand SetTimeOnLoad { get; }

        public OrderDetailViewModel(ServicesStore servicesStore, SelectedStore selectedStore, ControllersStore controllersStore, INavigationService closeNavigationService)
        {
            _servicesStore = servicesStore;
            _orderViewModel = selectedStore.CurrentOrder;
            _controllersStore = controllersStore;

            Price = _orderViewModel.Price;
            _base = _orderViewModel.GetModel();

            PriceField = new EditableTextFieldViewModel { Value = Price };
            PriceField.PropertyChanged += PriceField_PropertyChanged;

            _routes = new ObservableCollection<RouteViewModel>();
            _cargos = new ObservableCollection<CargoViewModel>();

            Back = new NavigateCommand(closeNavigationService);
            SaveChanges = new UpdateEntityCommand<Order>(this, _servicesStore.GetService<Order>(), GetUpdatedOrder, closeNavigationService, () => IsDirty, () => IsDirty = false);
            CancelOrder = new UpdateEntityCommand<Order>(this, _servicesStore.GetService<Order>(), GetCancelledOrder, closeNavigationService);
            CompleteOrder = new UpdateEntityCommand<Order>(this, _servicesStore.GetService<Order>(), GetCompletedOrder, closeNavigationService, 
                () => Routes.All(r => (r.Status == Constants.GetEnumDescription(Constants.RouteStatusValues.Cancelled) || r.Status == Constants.GetEnumDescription(Constants.RouteStatusValues.Completed))) && Routes.Any(r => r.Status == Constants.GetEnumDescription(Constants.RouteStatusValues.Completed)));
            SetTimeLoad = new UpdateEntityCommand<Order>(this, _servicesStore.GetService<Order>(), GetNewLoadOrder, closeNavigationService, () => TimeLoad == "-");
            SetTimeOnLoad = new UpdateEntityCommand<Order>(this, _servicesStore.GetService<Order>(), GetNewOnLoadOrder, closeNavigationService, () => TimeOnLoad == "-" && TimeLoad != "-");

            UpdateData();
        }

        private void PriceField_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Value")
            {
                Price = PriceField.Value;
            }
        }

        private async void UpdateData()
        {
            _routes.Clear();
            _cargos.Clear();

            IEnumerable<Route> tempRoutes = await ((RouteDataService)_servicesStore.GetService<Route>()).GetRoutesByOrderAsync(ID);
            foreach (var route in tempRoutes)
            {
                var routeViewModel = new RouteViewModel(route, _controllersStore);
                _routes.Add(routeViewModel);
            }

            IEnumerable<Cargo> tempCargos = await ((CargoDataService)_servicesStore.GetService<Cargo>()).GetCargosByOrderAsync(ID);
            foreach (var temp in tempCargos)
            {
                var cargoViewModel = new CargoViewModel(temp, _controllersStore);
                _cargos.Add(cargoViewModel);
            }
        }

        private Order GetUpdatedOrder() => new Order(_base.ID, _base.BudID, _base.TimeOrder, _base.TimeLoad, _base.TimeOnLoad, fPrice, _base.Status, _base.PhoneLoad, _base.PhoneOnLoad);
        private Order GetCancelledOrder() => new Order(_base.ID, _base.BudID, _base.TimeOrder, _base.TimeLoad, _base.TimeOnLoad, _base.Price, Constants.OrderStatusValues.Cancelled, _base.PhoneLoad, _base.PhoneOnLoad);
        private Order GetCompletedOrder() => new Order(_base.ID, _base.BudID, _base.TimeOrder, _base.TimeLoad, _base.TimeOnLoad, _base.Price, Constants.OrderStatusValues.Completed, _base.PhoneLoad, _base.PhoneOnLoad);
        private Order GetNewLoadOrder() => new Order(_base.ID, _base.BudID, _base.TimeOrder, DateTime.Now, _base.TimeOnLoad, _base.Price, _base.Status, _base.PhoneLoad, _base.PhoneOnLoad);
        private Order GetNewOnLoadOrder() => new Order(_base.ID, _base.BudID, _base.TimeOrder, _base.TimeLoad, DateTime.Now, _base.Price, _base.Status, _base.PhoneLoad, _base.PhoneOnLoad);



        private string _price;
        public string Price
        {
            get => _price;
            set
            {
                if (float.TryParse(value, out float fValue))
                {
                    _price = value;
                    fPrice = fValue;
                    IsDirty = true;
                    OnPropertyChanged(nameof(Price));

                    if (PriceField?.Value != value && PriceField != null)
                    {
                        PriceField.Value = value;
                    }
                }
            }
        }

        public int ID => _orderViewModel.ID;
        public string ClientName => _orderViewModel.ClientName;
        public string TimeOrder => _orderViewModel.TimeOrder;
        public string TimeLoad => _orderViewModel.TimeLoad;
        public string TimeOnLoad => _orderViewModel.TimeOnLoad;
        public string Status => _orderViewModel.Status;


        private ObservableCollection<RouteViewModel> _routes;
        public ObservableCollection<RouteViewModel> Routes
        {
            get => _routes;
            set
            {
                _routes = value;
                OnPropertyChanged(nameof(Routes));
            }
        }

        private ObservableCollection<CargoViewModel> _cargos;
        public ObservableCollection<CargoViewModel> Cargos
        {
            get => _cargos;
            set
            {
                _cargos = value;
                OnPropertyChanged(nameof(Cargos));
            }
        }
    }
}