using CourseProgram.Commands;
using CourseProgram.Models;
using CourseProgram.Services;
using CourseProgram.Stores;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CourseProgram.ViewModels
{
    public class DriverDetailViewModel : BaseViewModel
    {
        private readonly DriverViewModel _driverViewModel;
        private readonly ServicesStore _servicesStore;

        private ObservableCollection<RouteViewModel> _routes = new ObservableCollection<RouteViewModel>();
        public IEnumerable<RouteViewModel> Routes => _routes;

        public ICommand BackCommand { get; }

        public DriverDetailViewModel(ServicesStore servicesStore, SelectedStore selectedStore, INavigationService closeNavigationService)
        {
            _driverViewModel = selectedStore.CurrentDriver;
            _servicesStore = servicesStore;

            BackCommand = new NavigateCommand(closeNavigationService);

            UpdateData();
        }

        private async void UpdateData()
        {
            _routes.Clear();

            IEnumerable<Route> temp = await _servicesStore._routeService.GetItemsByDriverAsync(ID);

            foreach (Route route in temp)
            {
                RouteViewModel routeViewModel = new(route);
                _routes.Add(routeViewModel);
            }

            StringCategories = await _servicesStore._categoryService.GetStringByDriverAsync(ID);
        }

        public int ID => _driverViewModel.ID;
        public string FIO => _driverViewModel.FIO;
        public string BirthDay => _driverViewModel.BirthDay;
        public string Passport => _driverViewModel.Passport;
        public string Phone => _driverViewModel.Phone;
        public string DateStart => _driverViewModel.DateStart;
        public string DateEnd => _driverViewModel.DateEnd;
        public string Town => _driverViewModel.Town;

        private string _stringCategories;
        public string StringCategories
        {
            get => _stringCategories;
            set
            {
                _stringCategories = value;
                OnPropertyChanged(nameof(StringCategories));
            }
        }

    }
}