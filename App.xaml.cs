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
        private readonly NavigationStore _naviagtionStore;
        private ServicesStore _servicesStore;

        public App()
        {
            _naviagtionStore = new NavigationStore();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            ShutdownMode = ShutdownMode.OnExplicitShutdown;

            LoginWindow loginWindow = new();
            loginWindow.ShowDialog();

            if ((bool)loginWindow.DialogResult)
            {
                _servicesStore = new ServicesStore();

                _naviagtionStore.CurrentViewModel = CreateDriverListingViewModel();

                MainWindow = new MainWindow()
                {
                    DataContext = new MainViewModel(_naviagtionStore)
                };
                MainWindow.Show();
            }
            else
                Shutdown();
        }

        private AddDriverViewModel CreateAddDriverViewModel() => new AddDriverViewModel(_servicesStore._driverService, new NavigationService(_naviagtionStore, CreateDriverListingViewModel));

        private DriverListingViewModel CreateDriverListingViewModel() => new DriverListingViewModel(_servicesStore._driverService, new NavigationService(_naviagtionStore, CreateAddDriverViewModel), _naviagtionStore);
    }
}