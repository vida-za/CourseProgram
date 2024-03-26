using CourseProgram.Commands;
using CourseProgram.Models;
using CourseProgram.Services;
using CourseProgram.Stores;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace CourseProgram.ViewModels
{
    public class DriverViewModel : BaseViewModel
    {
        private readonly Driver _driver;
        private readonly ServicesStore _servicesStore;

        private ObservableCollection<RouteViewModel> _routes = new ObservableCollection<RouteViewModel>();
        public IEnumerable<RouteViewModel> Routes => _routes;

        public ICommand BackCommand { get; }

        public int ID => _driver.ID;
        public string FIO => _driver.FIO;
        public string BirthDay => _driver.BirthDay.ToString("d");
        public string Passport => _driver.Passport;
        public string Phone => _driver.Phone;
        public string DateStart => _driver.DateStart.ToString("d");
        public string DateEnd => _driver.DateEnd.ToString("d");
        public string Town => _driver.Town;

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


        public DriverViewModel(Driver driver, NavigationStore navigationStore, DriverListingViewModel driverListingViewModel, ServicesStore servicesStore)
        {
            _driver = driver;
            _servicesStore = servicesStore;

            BackCommand = new NavigateCommand(new NavigationService(navigationStore,() => driverListingViewModel));

            UpdateData();
        }

        public Driver GetDriver() => _driver;

        private async void UpdateData()
        {
            _routes.Clear();

            IEnumerable<Route> temp = await _servicesStore._routeService.GetItemsByDriverAsync(_driver.ID);

            foreach (Route route in temp)
            {
                RouteViewModel routeViewModel = new(route);
                _routes.Add(routeViewModel);
            }

            StringCategories = await _servicesStore._categoryService.GetStringByDriverAsync(ID);
        }
    }
}