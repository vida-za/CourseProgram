using CourseProgram.Services.DataServices;
using CourseProgram.Services.DataServices.ExtDataService;

namespace CourseProgram.Stores
{
    public class ServicesStore
    {
        public readonly AddressDataService _addressService;
        public readonly BudDataService _budService;
        public readonly CargoDataService _cargoService;
        public readonly ClientDataService _clientService;
        public readonly DriverDataService _driverService;
        public readonly DriverCategoriesDataService _driverCategoriesService;
        public readonly HaulDataService _haulService;
        public readonly MachineDataService _machineService;
        public readonly NomenclatureDataService _nomenclatureService;
        public readonly OrderDataService _orderService;
        public readonly RouteDataService _routeService;
        public readonly WorkerDataService _workerService;

        // extService
        public readonly CategoryDataService _categoryService;

        public ServicesStore() 
        {
            _addressService = new AddressDataService();
            _budService = new BudDataService();
            _cargoService = new CargoDataService();
            _clientService = new ClientDataService();
            _driverService = new DriverDataService();
            _driverCategoriesService = new DriverCategoriesDataService();
            _haulService = new HaulDataService();
            _machineService = new MachineDataService();
            _nomenclatureService = new NomenclatureDataService();
            _orderService = new OrderDataService();
            _routeService = new RouteDataService();
            _workerService = new WorkerDataService();

            _categoryService = new CategoryDataService();
        }
    }
}