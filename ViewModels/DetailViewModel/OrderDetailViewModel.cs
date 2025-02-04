using CourseProgram.Commands;
using CourseProgram.Models;
using CourseProgram.Services;
using CourseProgram.Stores;
using CourseProgram.ViewModels.EntityViewModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace CourseProgram.ViewModels.DetailViewModel
{
    public class OrderDetailViewModel : BaseViewModel
    {
        private readonly ServicesStore _servicesStore;
        private readonly OrderViewModel _orderViewModel;

        public ICommand Back { get; }

        public OrderDetailViewModel(ServicesStore servicesStore, SelectedStore selectedStore, INavigationService closeNavigationService)
        {
            _servicesStore = servicesStore;
            _orderViewModel = selectedStore.CurrentOrder;

            _routes = new ObservableCollection<RouteViewModel>();
            _cargos = new ObservableCollection<CargoViewModel>();

            Back = new NavigateCommand(closeNavigationService);

            UpdateData();
        }

        private async void UpdateData()
        {
            _routes.Clear();
            _cargos.Clear();

            IEnumerable<Route> tempRoutes = await _servicesStore._routeService.GetRoutesByOrderAsync(ID);
            foreach (var temp in tempRoutes)
            {
                var routeViewModel = new RouteViewModel(temp, _servicesStore);
                _routes.Add(routeViewModel);
            }

            IEnumerable<Cargo> tempCargos = await _servicesStore._cargoService.GetCargosByOrderAsync(ID);
            foreach (var temp in tempCargos)
            {
                var cargoViewModel = new CargoViewModel(temp, _servicesStore);
                _cargos.Add(cargoViewModel);
            }
        }

        public int ID => _orderViewModel.ID;
        public string ClientName => _orderViewModel.ClientName;
        public string TimeOrder => _orderViewModel.TimeOrder;
        public string TimeLoad => _orderViewModel.TimeLoad;
        public string TimeOnLoad => _orderViewModel.TimeOnLoad;
        public string Price => _orderViewModel.Price;
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