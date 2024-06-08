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
    public class WorkerDetailViewModel : BaseViewModel
    {
        private readonly WorkerViewModel _workerViewModel;
        private readonly ServicesStore _servicesStore;

        private ObservableCollection<OrderViewModel> _orders = new ObservableCollection<OrderViewModel>();
        public IEnumerable<OrderViewModel> Orders => _orders;

        public ICommand BackCommand { get; }

        public WorkerDetailViewModel(
            ServicesStore servicesStore,
            SelectedStore selectedStore,
            INavigationService closeNavigationService) 
        {
            _workerViewModel = selectedStore.CurrentWorker;
            _servicesStore = servicesStore;

            BackCommand = new NavigateCommand(closeNavigationService);

            UpdateData();
        }

        private async void UpdateData()
        {
            _orders.Clear();

            IEnumerable<Order> temp = await _servicesStore._orderService.GetItemsAsync(); //TO DO

            foreach (Order order in temp)
            {
                OrderViewModel orderViewModel = new(order, _servicesStore);
                _orders.Add(orderViewModel);
            }
        }

        public int ID => _workerViewModel.ID;
        public string FIO => _workerViewModel.FIO;
        public string BirthDay => _workerViewModel.BirthDay;
        public string Passport => _workerViewModel.Passport;
        public string Phone => _workerViewModel.Phone;
        public string DateStart => _workerViewModel.DateStart;
        public string DateEnd => _workerViewModel.DateEnd;
    }
}