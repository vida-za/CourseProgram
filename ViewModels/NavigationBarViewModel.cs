using CourseProgram.Commands;
using CourseProgram.Services;
using System.Windows.Input;

namespace CourseProgram.ViewModels
{
    public class NavigationBarViewModel : BaseViewModel
    {
        public ICommand NavigateDriversCommand { get; }
        public ICommand NavigateMachinesCommand { get; }

        public NavigationBarViewModel(INavigationService driversNavigationService, INavigationService machinesNavigationService) 
        {
            NavigateDriversCommand = new NavigateCommand(driversNavigationService);
            NavigateMachinesCommand = new NavigateCommand(machinesNavigationService);
        }
    }
}