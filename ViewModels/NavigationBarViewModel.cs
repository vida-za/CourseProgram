using CourseProgram.Commands;
using CourseProgram.Services;
using System.Windows.Input;

namespace CourseProgram.ViewModels
{
    public class NavigationBarViewModel : BaseViewModel
    {
        public ICommand NavigationAddressesCommand { get; }
        public ICommand NavigateDriversCommand { get; }
        public ICommand NavigateMachinesCommand { get; }

        public NavigationBarViewModel(
            INavigationService addressesNavigationService,
            INavigationService driversNavigationService, 
            INavigationService machinesNavigationService) 
        {
            NavigationAddressesCommand = new NavigateCommand(addressesNavigationService);
            NavigateDriversCommand = new NavigateCommand(driversNavigationService);
            NavigateMachinesCommand = new NavigateCommand(machinesNavigationService);
        }
    }
}