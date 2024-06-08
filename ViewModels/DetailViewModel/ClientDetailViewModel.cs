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
    public class ClientDetailViewModel : BaseViewModel
    {
        #region fields
        private readonly ClientViewModel _clientViewModel;
        private readonly ServicesStore _servicesStore;
        private readonly ObservableCollection<OrderViewModel> _orders;
        
        public ICommand BackCommand { get; }
        #endregion

        public ClientDetailViewModel(
            ServicesStore servicesStore, 
            SelectedStore selectedStore,
            INavigationService closeNavigationService)
        {
            _orders = new ObservableCollection<OrderViewModel>();

            _clientViewModel = selectedStore.CurrentClient;
            _servicesStore = servicesStore;

            BackCommand = new NavigateCommand(closeNavigationService);

            UpdateData();
        }

        #region methods
        private async void UpdateData()
        {
            _orders.Clear();

            IEnumerable<Order> temp = await _servicesStore._orderService.GetItemsAsync();

            foreach (Order order in temp)
            {
                OrderViewModel orderViewModel = new(order, _servicesStore);
                _orders.Add(orderViewModel);
            }
        }
        #endregion

        #region properties
        public IEnumerable<OrderViewModel> Orders => _orders;

        public int ID => _clientViewModel.ID;
        public string Name => _clientViewModel.Name;
        public string StringType => _clientViewModel.Type;
        public string INN => _clientViewModel.INN;
        public string KPP => _clientViewModel.KPP;
        public string OGRN => _clientViewModel.OGRN;
        public string Phone => _clientViewModel.Phone;
        public string Checking => _clientViewModel.Checking;
        public string BIK => _clientViewModel.BIK;
        public string Correspondent => _clientViewModel.Correspondent;
        public string Bank => _clientViewModel.Bank;
        public string PhoneLoad => _clientViewModel.PhoneLoad;
        public string PhoneOnLoad => _clientViewModel.PhoneOnLoad;
        #endregion
    }
}