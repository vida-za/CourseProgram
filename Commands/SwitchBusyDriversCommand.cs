using CourseProgram.ViewModels;

namespace CourseProgram.Commands
{
    public class SwitchBusyDriversCommand : CommandBase
    {
        private readonly DriverListingViewModel _driverListingViewModel;

        public SwitchBusyDriversCommand(DriverListingViewModel driverListingViewModel) 
        { 
            _driverListingViewModel = driverListingViewModel;
        }

        public override void Execute(object? parameter)
        {
            _driverListingViewModel.SwitchDrivers();
        }
    }
}