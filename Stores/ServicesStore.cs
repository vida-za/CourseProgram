using CourseProgram.Services.DataServices;

namespace CourseProgram.Stores
{
    public class ServicesStore
    {
        public readonly DriverDataService _driverService;
        public readonly CategoryDataService _categoryService;
        public readonly DriverCategoriesDataService _driverCategoriesService;
        public readonly RouteDataService _routeService;
        public ServicesStore() 
        {
            _driverService = new();
            _categoryService = new();
            _driverCategoriesService = new();
            _routeService = new();
        }
    }
}