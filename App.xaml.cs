using CourseProgram.Stores;
using CourseProgram.Views;
using CourseProgram.ViewModels;
using System.Windows;
using CourseProgram.DataClasses;
using CourseProgram.Services;

namespace CourseProgram
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly InitData _initData;
        private readonly NavigationStore _naviagtionStore;

        public App()
        {
            _initData = new InitData();
            _naviagtionStore = new NavigationStore();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            _naviagtionStore.CurrentViewModel = CreateDriverListingViewModel();

            MainWindow = new MainWindow()
            {
                DataContext = new MainViewModel(_naviagtionStore)
            };
            MainWindow.Show();

            base.OnStartup(e);
        }

        private AddDriverViewModel CreateAddDriverViewModel()
        {
            return new AddDriverViewModel(_initData.DriverData, new NavigationService(_naviagtionStore, CreateDriverListingViewModel));
        }

        private DriverListingViewModel CreateDriverListingViewModel() 
        {
            return new DriverListingViewModel(_initData.DriverData, new NavigationService(_naviagtionStore, CreateAddDriverViewModel));
        }
    }
}