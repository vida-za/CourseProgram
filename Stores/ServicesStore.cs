using CourseProgram.Models;
using CourseProgram.Services.DataServices;

namespace CourseProgram.Stores
{
    public class ServicesStore
    {
        public readonly AddressDataService _addressService;
        public readonly CargoDataService _cargoService;
        public readonly CategoryDataService _categoryService;
        public readonly ClientDataService _clientService;
        public readonly DriverCategoriesDataService _driverCategoriesService;
        public readonly DriverDataService _driverService;
        public readonly HaulDataService _haulService;
        public readonly MachineDataService _machineService;
        public readonly OrderDataService _orderService;
        public readonly RouteDataService _routeService;
        public readonly WorkerDataService _workerService;

        public ServicesStore() 
        {
            _addressService = new AddressDataService();
            _cargoService = new CargoDataService();
            _categoryService = new CategoryDataService();
            _clientService = new ClientDataService();
            _driverCategoriesService = new DriverCategoriesDataService();
            _driverService = new DriverDataService();
            _haulService = new HaulDataService();
            _machineService = new MachineDataService();
            _orderService = new OrderDataService();
            _routeService = new RouteDataService();
            _workerService = new WorkerDataService();
        }
    }
}