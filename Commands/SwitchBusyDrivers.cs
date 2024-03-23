using CourseProgram.ViewModels;

namespace CourseProgram.Commands
{
    public class SwitchBusyDrivers : CommandBase
    {
        private readonly DriverListingViewModel _driverListingViewModel;
        public SwitchBusyDrivers(DriverListingViewModel driverListingViewModel) 
        { 
            _driverListingViewModel = driverListingViewModel;
        }

        public override void Execute(object? parameter)
        {
            _driverListingViewModel.SwitchDrivers();
        }
    }
}