namespace CourseProgram.ViewModels
{
    public class LayoutViewModel : BaseViewModel
    {
        public NavigationBarViewModel NavigationBarViewModel { get; }
        public BaseViewModel ContentViewModel { get; }

        public LayoutViewModel(NavigationBarViewModel navigationBarViewModel, BaseViewModel contentViewModel)
        {
            NavigationBarViewModel = navigationBarViewModel;
            ContentViewModel = contentViewModel;
        }

        public override void Dispose()
        {
            NavigationBarViewModel.Dispose();
            ContentViewModel.Dispose();

            base.Dispose();
        }
    }
}