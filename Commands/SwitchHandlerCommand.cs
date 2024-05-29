using CourseProgram.ViewModels.ListingViewModel;

namespace CourseProgram.Commands
{
    public class SwitchHandlerCommand : CommandBase
    {
        private readonly BaseListingViewModel _listingViewModel;

        public SwitchHandlerCommand(BaseListingViewModel listingViewModel)
        {
            _listingViewModel = listingViewModel;
        }

        public override void Execute(object? parameter)
        {
            _listingViewModel.SwitchHandler();
        }
    }
}