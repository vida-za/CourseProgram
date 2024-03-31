using CourseProgram.Services;

namespace CourseProgram.Commands
{
    public class NavigateDetailCommand : CommandBase
    {
        private readonly INavigationService _navigationService;

        public NavigateDetailCommand(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public override void Execute(object? parameter)
        {
            if (parameter is not null) _navigationService.Navigate();
        }
    }
}