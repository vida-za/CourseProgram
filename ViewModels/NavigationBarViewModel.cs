using CourseProgram.Commands;
using CourseProgram.Services;
using System.Windows.Input;

namespace CourseProgram.ViewModels
{
    public class NavigationBarViewModel : BaseViewModel
    {
        #region fields
        public ICommand NavigateAddressesCommand { get; }
        //public ICommand NavigateCargoesCommand { get; }
        public ICommand NavigateClientsCommand { get; }
        public ICommand NavigateDriversCommand { get; }
        //public ICommand NavigateHaulsCommand { get; }
        public ICommand NavigateMachinesCommand { get; }
        public ICommand NavigateNomenclaturesCommand { get; }
        //public ICommand NavigateOrdersCommand { get; }
        //public ICommand NavigateRoutesCommand { get; }
        public ICommand NavigateWorkersCommand { get; }

        public ICommand NavigateHomeCommand { get; }
        public ICommand NavigateOperationalCommand { get; }
        #endregion

        public NavigationBarViewModel(
            INavigationService addressesNavigationService,
            INavigationService clientsNavigationService,
            INavigationService driversNavigationService, 
            INavigationService machinesNavigationService,
            INavigationService nomenclaturesNavigationService,
            INavigationService workersNavigationService,
            INavigationService homeNavigationService,
            INavigationService operationalNavigationService) 
        {
            NavigateAddressesCommand = new NavigateCommand(addressesNavigationService);
            //NavigateCargoesCommand = new NavigateCommand(cargoesNavigationService); 
            NavigateClientsCommand = new NavigateCommand(clientsNavigationService);
            NavigateDriversCommand = new NavigateCommand(driversNavigationService);
            //NavigateHaulsCommand = new NavigateCommand(haulsNavigationService);
            NavigateMachinesCommand = new NavigateCommand(machinesNavigationService);
            NavigateNomenclaturesCommand = new NavigateCommand(nomenclaturesNavigationService);
            //NavigateOrdersCommand = new NavigateCommand(ordersNavigationService);
            //NavigateRoutesCommand = new NavigateCommand(routesNavigationService);
            NavigateWorkersCommand = new NavigateCommand(workersNavigationService);

            NavigateHomeCommand = new NavigateCommand(homeNavigationService);
            NavigateOperationalCommand = new NavigateCommand(operationalNavigationService);
        }
    }
}