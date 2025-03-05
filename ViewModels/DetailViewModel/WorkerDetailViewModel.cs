using CourseProgram.Commands;
using CourseProgram.Controllers.DataControllers.EntityDataControllers;
using CourseProgram.Models;
using CourseProgram.Services;
using CourseProgram.Stores;
using CourseProgram.ViewModels.EntityViewModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace CourseProgram.ViewModels.DetailViewModel
{
    public class WorkerDetailViewModel : BaseDetailViewModel
    {
        private readonly WorkerViewModel _workerViewModel;
        private readonly ControllersStore _controllersStore;

        private ObservableCollection<OrderViewModel> _orders = new ObservableCollection<OrderViewModel>();
        public IEnumerable<OrderViewModel> Orders => _orders;

        public ICommand BackCommand { get; }

        public WorkerDetailViewModel(
            SelectedStore selectedStore,
            ControllersStore controllersStore,
            INavigationService closeNavigationService) 
        {
            _workerViewModel = selectedStore.CurrentWorker;
            _controllersStore = controllersStore;

            BackCommand = new NavigateCommand(closeNavigationService);

            UpdateData();
        }

        private async void UpdateData()
        {
            _orders.Clear();

            IEnumerable<Bud> temp = await ((BudDataController)_controllersStore.GetController<Bud>()).GetBudsByWorker(ID);

            foreach (var bud in temp)
            {
                Order? order = await ((OrderDataController)_controllersStore.GetController<Order>()).GetOrderByBud(bud.ID);
                if (order != null)
                {
                    var orderViewModel = new OrderViewModel(order, _controllersStore);
                    _orders.Add(orderViewModel);
                }
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