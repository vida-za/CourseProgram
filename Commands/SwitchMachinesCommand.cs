using CourseProgram.ViewModels.ListingViewModel;

namespace CourseProgram.Commands
{
    public class SwitchMachinesCommand : CommandBase
    {
        private readonly MachineListingViewModel _machineListingViewModel;

        public SwitchMachinesCommand(MachineListingViewModel machineListingViewModel)
        {
            _machineListingViewModel = machineListingViewModel;
        }

        public override void Execute(object? parameter)
        {
            _machineListingViewModel.SwitchMachines();
        }
    }
}