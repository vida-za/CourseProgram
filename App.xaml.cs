using CourseProgram.Stores;
using CourseProgram.Views;
using CourseProgram.ViewModels;
using System.Windows;
using CourseProgram.Services;
using System;
using Microsoft.Extensions.DependencyInjection;
using CourseProgram.ViewModels.ListingViewModel;
using CourseProgram.ViewModels.AddViewModel;
using CourseProgram.ViewModels.DetailViewModel;

namespace CourseProgram
{
    public partial class App : Application
    {
        private readonly IServiceProvider _serviceProvider;

        public App()
        {
            IServiceCollection services = new ServiceCollection();

            //Store
            services.AddSingleton<NavigationStore>();
            services.AddSingleton<ServicesStore>();
            services.AddSingleton<SelectedStore>();
            services.AddSingleton<ModalNavigationStore>();

            services.AddSingleton(s => CreateHomeNavigationService(s));
            services.AddSingleton<CloseModalNavigationService>();

            //Modal
            services.AddTransient(s => new AddDriverViewModel(
                s.GetRequiredService<ServicesStore>(),
                s.GetRequiredService<CloseModalNavigationService>()));
            services.AddTransient(s => new DriverDetailViewModel(
                s.GetRequiredService<ServicesStore>(),
                s.GetRequiredService<SelectedStore>(),
                s.GetRequiredService<CloseModalNavigationService>()));

            services.AddTransient(s => new AddMachineViewModel(
                s.GetRequiredService<ServicesStore>(),
                s.GetRequiredService<CloseModalNavigationService>()));
            services.AddTransient(s => new MachineDetailViewModel(
                s.GetRequiredService<ServicesStore>(),
                s.GetRequiredService<SelectedStore>(),
                s.GetRequiredService<CloseModalNavigationService>()));

            services.AddTransient(s => new AddAddressViewModel(
                s.GetRequiredService<ServicesStore>(),
                s.GetRequiredService<CloseModalNavigationService>()));

            services.AddTransient(s => new AddWorkerViewModel(
                s.GetRequiredService<ServicesStore>(),
                s.GetRequiredService<CloseModalNavigationService>()));
            services.AddTransient(s => new WorkerDetailViewModel(
                s.GetRequiredService<ServicesStore>(),
                s.GetRequiredService<SelectedStore>(),
                s.GetRequiredService<CloseModalNavigationService>()));

            services.AddTransient(s => new AddClientViewModel(
                s.GetRequiredService<ServicesStore>(),
                s.GetRequiredService<CloseModalNavigationService>()));
            services.AddTransient(s => new ClientDetailViewModel(
                s.GetRequiredService<ServicesStore>(),
                s.GetRequiredService<SelectedStore>(),
                s.GetRequiredService<CloseModalNavigationService>()));

            //Layout
            services.AddTransient(s => new DriverListingViewModel(
                s.GetRequiredService<ServicesStore>(),
                s.GetRequiredService<SelectedStore>(),
                CreateAddDriverNavigationService(s),
                CreateDriverDetailNavigationService(s)));
            services.AddTransient(s => new MachineListingViewModel(
                s.GetRequiredService<ServicesStore>(),
                s.GetRequiredService<SelectedStore>(),
                CreateAddMachineNavigationService(s),
                CreateMachineDetailNavigationService(s)));
            services.AddTransient(s => new AddressListingViewModel(
                s.GetRequiredService<ServicesStore>(),
                s.GetRequiredService<SelectedStore>(),
                CreateAddAddressNavigationService(s)));
            services.AddTransient(s => new WorkerListingViewModel(
                s.GetRequiredService<ServicesStore>(),
                s.GetRequiredService<SelectedStore>(),
                CreateAddWorkerNavigationService(s),
                CreateWorkerDetailNavigationService(s)));
            services.AddTransient(s => new ClientListingViewModel(
                s.GetRequiredService<ServicesStore>(),
                s.GetRequiredService<SelectedStore>(),
                CreateAddClientNavigationService(s),
                CreateClientDetailNavigationService(s)));

            services.AddSingleton(s => new HomeViewModel());

            services.AddTransient(CreateNavigationBarViewModel);
            services.AddSingleton<MainViewModel>();

            services.AddSingleton(s => new MainWindow()
            {
                DataContext = s.GetRequiredService<MainViewModel>()
            });

            _serviceProvider = services.BuildServiceProvider();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            ShutdownMode = ShutdownMode.OnExplicitShutdown;

            LoginWindow loginWindow = new();
            loginWindow.ShowDialog();

            if ((bool)loginWindow.DialogResult)
            {
                INavigationService initialNavigationService = _serviceProvider.GetRequiredService<INavigationService>();
                initialNavigationService.Navigate();

                MainWindow = _serviceProvider.GetRequiredService<MainWindow>();
                MainWindow.Show();
            }
            else
                Shutdown();
        }

        #region modal
        private static INavigationService CreateModalNavigationService<T>(IServiceProvider serviceProvider) where T : BaseViewModel
        {
            return new ModalNavigationService<T>(
                serviceProvider.GetRequiredService<ModalNavigationStore>(),
                () => serviceProvider.GetRequiredService<T>());
        }

        //Driver
        private static INavigationService CreateAddDriverNavigationService(IServiceProvider serviceProvider)
        {
            return CreateModalNavigationService<AddDriverViewModel>(serviceProvider);
        }

        private static INavigationService CreateDriverDetailNavigationService(IServiceProvider serviceProvider)
        {
            return CreateModalNavigationService<DriverDetailViewModel>(serviceProvider);
        }

        //Machine
        private static INavigationService CreateAddMachineNavigationService(IServiceProvider serviceProvider)
        {
            return CreateModalNavigationService<AddMachineViewModel>(serviceProvider);
        }

        private static INavigationService CreateMachineDetailNavigationService(IServiceProvider serviceProvider)
        {
            return CreateModalNavigationService<MachineDetailViewModel>(serviceProvider);
        }

        //Address
        private static INavigationService CreateAddAddressNavigationService(IServiceProvider serviceProvider)
        {
            return CreateModalNavigationService<AddAddressViewModel>(serviceProvider);
        }

        //Worker
        private static INavigationService CreateAddWorkerNavigationService(IServiceProvider serviceProvider)
        {
            return CreateModalNavigationService<AddWorkerViewModel>(serviceProvider);
        }

        private static INavigationService CreateWorkerDetailNavigationService(IServiceProvider serviceProvider)
        {
            return CreateModalNavigationService<WorkerDetailViewModel>(serviceProvider);
        }

        //Client
        private static INavigationService CreateAddClientNavigationService(IServiceProvider serviceProvider)
        {
            return CreateModalNavigationService<AddClientViewModel>(serviceProvider);
        }

        private static INavigationService CreateClientDetailNavigationService(IServiceProvider serviceProvider)
        {
            return CreateModalNavigationService<ClientDetailViewModel>(serviceProvider);
        }
        #endregion

        #region layout
        private static INavigationService CreateLayoutNavigationService<T>(IServiceProvider serviceProvider) where T : BaseViewModel
        {
            return new LayoutNavigationService<T>(
                serviceProvider.GetRequiredService<NavigationStore>(),
                () => serviceProvider.GetRequiredService<T>(),
                () => serviceProvider.GetRequiredService<NavigationBarViewModel>());
        }

        private static INavigationService CreateDriverListingNavigationService(IServiceProvider serviceProvider)
        {
            return CreateLayoutNavigationService<DriverListingViewModel>(serviceProvider);
        }

        private static INavigationService CreateMachineListingNavigationService(IServiceProvider serviceProvider)
        {
            return CreateLayoutNavigationService<MachineListingViewModel>(serviceProvider);
        }

        private static INavigationService CreateAddressListingNavigationService(IServiceProvider serviceProvider)
        {
            return CreateLayoutNavigationService<AddressListingViewModel>(serviceProvider);
        }

        private static INavigationService CreateWorkerListingNavigationService(IServiceProvider serviceProvider)
        {
            return CreateLayoutNavigationService<WorkerListingViewModel>(serviceProvider);
        }

        private static INavigationService CreateClientListingNavigationService(IServiceProvider serviceProvider)
        {
            return CreateLayoutNavigationService<ClientListingViewModel>(serviceProvider);
        }

        private static INavigationService CreateHomeNavigationService(IServiceProvider serviceProvider)
        {
            return CreateLayoutNavigationService<HomeViewModel>(serviceProvider);
        }
        #endregion

        private NavigationBarViewModel CreateNavigationBarViewModel(IServiceProvider serviceProvider)
        {
            return new NavigationBarViewModel(
                CreateAddressListingNavigationService(serviceProvider),
                CreateClientListingNavigationService(serviceProvider),
                CreateDriverListingNavigationService(serviceProvider),
                CreateMachineListingNavigationService(serviceProvider),
                CreateWorkerListingNavigationService(serviceProvider));
        }
    }
}