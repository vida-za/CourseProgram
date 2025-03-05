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
using CourseProgram.ViewModels.HistoryViewModel;

namespace CourseProgram
{
    public partial class App : Application
    {
        private readonly IServiceProvider _serviceProvider;

        public App()
        {
            IServiceCollection services = new ServiceCollection();

            services.AddSingleton<LogManager>();

            //Store
            services.AddSingleton<DataStore>();
            services.AddSingleton<NavigationStore>();
            services.AddSingleton(s => new ControllersStore(s.GetRequiredService<DataStore>()));
            services.AddSingleton(s => new ServicesStore(s.GetRequiredService<DataStore>()));
            services.AddSingleton<SelectedStore>();
            services.AddSingleton<ModalNavigationStore>();

            services.AddSingleton(CreateHomeNavigationService);
            services.AddSingleton<CloseModalNavigationService>();

            //Modal
            services.AddTransient(s => new AddDriverViewModel(
                s.GetRequiredService<ServicesStore>(),
                s.GetRequiredService<CloseModalNavigationService>()));
            services.AddTransient(s => new DriverDetailViewModel(
                s.GetRequiredService<SelectedStore>(),
                s.GetRequiredService<ControllersStore>(),
                s.GetRequiredService<CloseModalNavigationService>()));

            services.AddTransient(s => new AddMachineViewModel(
                s.GetRequiredService<ServicesStore>(),
                s.GetRequiredService<CloseModalNavigationService>()));
            services.AddTransient(s => new MachineDetailViewModel(
                s.GetRequiredService<SelectedStore>(),
                s.GetRequiredService<ControllersStore>(),
                s.GetRequiredService<CloseModalNavigationService>()));

            services.AddTransient(s => new AddAddressViewModel(
                s.GetRequiredService<ServicesStore>(),
                s.GetRequiredService<CloseModalNavigationService>()));

            services.AddTransient(s => new AddWorkerViewModel(
                s.GetRequiredService<ServicesStore>(),
                s.GetRequiredService<CloseModalNavigationService>()));
            services.AddTransient(s => new WorkerDetailViewModel(
                s.GetRequiredService<SelectedStore>(),
                s.GetRequiredService<ControllersStore>(),
                s.GetRequiredService<CloseModalNavigationService>()));

            services.AddTransient(s => new AddClientViewModel(
                s.GetRequiredService<ServicesStore>(),
                s.GetRequiredService<CloseModalNavigationService>()));
            services.AddTransient(s => new ClientDetailViewModel(
                s.GetRequiredService<SelectedStore>(),
                s.GetRequiredService<ControllersStore>(),
                s.GetRequiredService<CloseModalNavigationService>()));

            services.AddTransient(s => new AddNomenclatureViewModel(
                s.GetRequiredService<ServicesStore>(),
                s.GetRequiredService<CloseModalNavigationService>()));

            services.AddTransient(s => new OrderDetailViewModel(
                s.GetRequiredService<ServicesStore>(),
                s.GetRequiredService<SelectedStore>(),
                s.GetRequiredService<ControllersStore>(),
                s.GetRequiredService<CloseModalNavigationService>()));
            services.AddTransient(s => new BudDetailViewModel(
                s.GetRequiredService<ServicesStore>(),
                s.GetRequiredService<SelectedStore>(),
                s.GetRequiredService<ControllersStore>(),
                s.GetRequiredService<CloseModalNavigationService>()));
            services.AddTransient(s => new AddBudViewModel(
                s.GetRequiredService<ServicesStore>(),
                s.GetRequiredService<ControllersStore>(),
                s.GetRequiredService<CloseModalNavigationService>()));

            services.AddTransient(s => new RouteDetailViewModel(
                s.GetRequiredService<ServicesStore>(),
                s.GetRequiredService<SelectedStore>(),
                s.GetRequiredService<ControllersStore>(),
                s.GetRequiredService<CloseModalNavigationService>()));
            services.AddTransient(s => new AddRouteViewModel(
                s.GetRequiredService<ServicesStore>(),
                s.GetRequiredService<ControllersStore>(),
                s.GetRequiredService<CloseModalNavigationService>()));

            //Layout
            services.AddTransient(s => new DriverListingViewModel(
                s.GetRequiredService<ServicesStore>(),
                s.GetRequiredService<SelectedStore>(),
                s.GetRequiredService<ControllersStore>(),
                CreateAddDriverNavigationService(s),
                CreateDriverDetailNavigationService(s)));
            services.AddTransient(s => new MachineListingViewModel(
                s.GetRequiredService<ServicesStore>(),
                s.GetRequiredService<SelectedStore>(),
                s.GetRequiredService<ControllersStore>(),
                CreateAddMachineNavigationService(s),
                CreateMachineDetailNavigationService(s)));
            services.AddTransient(s => new AddressListingViewModel(
                s.GetRequiredService<SelectedStore>(),
                s.GetRequiredService<ControllersStore>(),
                CreateAddAddressNavigationService(s)));
            services.AddTransient(s => new WorkerListingViewModel(
                s.GetRequiredService<ServicesStore>(),
                s.GetRequiredService<SelectedStore>(),
                s.GetRequiredService<ControllersStore>(),
                CreateAddWorkerNavigationService(s),
                CreateWorkerDetailNavigationService(s)));
            services.AddTransient(s => new ClientListingViewModel(
                s.GetRequiredService<ServicesStore>(),
                s.GetRequiredService<SelectedStore>(),
                s.GetRequiredService<ControllersStore>(),
                CreateAddClientNavigationService(s),
                CreateClientDetailNavigationService(s)));
            services.AddTransient(s => new OperationalViewModel(
                s.GetRequiredService<ServicesStore>(),
                s.GetRequiredService<SelectedStore>(),
                s.GetRequiredService<ControllersStore>(),
                CreateOrderDetailNavigationService(s),
                CreateBudDetailNavigationService(s),
                CreateAddBudNavigationService(s),
                CreateAddRouteNavigationService(s)));
            services.AddTransient(s => new NomenclatureListingViewModel(
                s.GetRequiredService<ServicesStore>(), 
                s.GetRequiredService<SelectedStore>(),
                s.GetRequiredService<ControllersStore>(),
                CreateAddNomenclatureNavigationService(s)));
            services.AddTransient(s => new RouteListingViewModel(
                s.GetRequiredService<ServicesStore>(),
                s.GetRequiredService<SelectedStore>(),
                s.GetRequiredService<ControllersStore>(),
                CreateRouteDetailNavigationService(s)));
            services.AddTransient(s => new BudHistoryViewModel(
                s.GetRequiredService<SelectedStore>(),
                s.GetRequiredService<ControllersStore>(),
                CreateBudDetailNavigationService(s)));
            services.AddTransient(s => new HaulHistoryViewModel(
                s.GetRequiredService<ServicesStore>(),
                s.GetRequiredService<ControllersStore>()));
            services.AddTransient(s => new CreatingItineraryViewModel(
                s.GetRequiredService<ControllersStore>(),
                s.GetRequiredService<SelectedStore>(),
                CreateRouteDetailNavigationService(s)));

            services.AddSingleton(s => new HomeViewModel(s.GetRequiredService<ServicesStore>()));

            services.AddTransient(CreateNavigationBarViewModel);
            services.AddSingleton<MainViewModel>();

            services.AddSingleton(s => new MainWindow()
            {
                DataContext = s.GetRequiredService<MainViewModel>()
            });

            _serviceProvider = services.BuildServiceProvider();

            LogManager.Initialize(_serviceProvider.GetRequiredService<LogManager>());
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            await LogManager.Instance.WriteLogAsync("App start");

            ShutdownMode = ShutdownMode.OnExplicitShutdown;

            LoginWindow loginWindow = new();
            loginWindow.ShowDialog();

            if ((bool)loginWindow.DialogResult)
            {
                await LogManager.Instance.WriteLogAsync("User login");

                INavigationService initialNavigationService = _serviceProvider.GetRequiredService<INavigationService>();
                initialNavigationService.Navigate();

                MainWindow = _serviceProvider.GetRequiredService<MainWindow>();
                MainWindow.Show();
            }
            else 
            {
                await LogManager.Instance.WriteLogAsync("User not login");
                Shutdown();
            }
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

        //Nomenclature
        private static INavigationService CreateAddNomenclatureNavigationService(IServiceProvider serviceProvider)
        {
            return CreateModalNavigationService<AddNomenclatureViewModel>(serviceProvider);
        }

        //Operational
        private static INavigationService CreateOrderDetailNavigationService(IServiceProvider serviceProvider)
        {
            return CreateModalNavigationService<OrderDetailViewModel>(serviceProvider);
        }

        private static INavigationService CreateBudDetailNavigationService(IServiceProvider serviceProvider)
        {
            return CreateModalNavigationService<BudDetailViewModel>(serviceProvider);
        }

        private static INavigationService CreateAddBudNavigationService(IServiceProvider serviceProvider)
        {
            return CreateModalNavigationService<AddBudViewModel>(serviceProvider);
        }

        private static INavigationService CreateAddRouteNavigationService(IServiceProvider serviceProvider)
        {
            return CreateModalNavigationService<AddRouteViewModel>(serviceProvider);
        }

        //Route
        private static INavigationService CreateRouteDetailNavigationService(IServiceProvider serviceProvider)
        {
            return CreateModalNavigationService<RouteDetailViewModel>(serviceProvider);
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

        private static INavigationService CreateOperationalNavigationService(IServiceProvider serviceProvider)
        {
            return CreateLayoutNavigationService<OperationalViewModel>(serviceProvider);
        }

        private static INavigationService CreateNomenclatureListingNavigationService(IServiceProvider serviceProvider)
        {
            return CreateLayoutNavigationService<NomenclatureListingViewModel>(serviceProvider);
        }

        private static INavigationService CreateRouteListingNavigationService(IServiceProvider serviceProvider)
        {
            return CreateLayoutNavigationService<RouteListingViewModel>(serviceProvider);
        }

        private static INavigationService CreateBudHistoryNavigationService(IServiceProvider serviceProvider)
        {
            return CreateLayoutNavigationService<BudHistoryViewModel>(serviceProvider);
        }

        private static INavigationService CreateHaulHistoryNavigationService(IServiceProvider serviceProvider)
        {
            return CreateLayoutNavigationService<HaulHistoryViewModel>(serviceProvider);
        }

        private static INavigationService CreateItineraryNavigationService(IServiceProvider serviceProvider)
        {
            return CreateLayoutNavigationService<CreatingItineraryViewModel>(serviceProvider);
        }
        #endregion

        private NavigationBarViewModel CreateNavigationBarViewModel(IServiceProvider serviceProvider)
        {
            return new NavigationBarViewModel(
                CreateAddressListingNavigationService(serviceProvider),
                CreateClientListingNavigationService(serviceProvider),
                CreateDriverListingNavigationService(serviceProvider),
                CreateMachineListingNavigationService(serviceProvider),
                CreateNomenclatureListingNavigationService(serviceProvider),
                CreateWorkerListingNavigationService(serviceProvider),
                CreateHomeNavigationService(serviceProvider),
                CreateOperationalNavigationService(serviceProvider),
                CreateRouteListingNavigationService(serviceProvider),
                CreateBudHistoryNavigationService(serviceProvider),
                CreateHaulHistoryNavigationService(serviceProvider),
                CreateItineraryNavigationService(serviceProvider));
        }
    }
}