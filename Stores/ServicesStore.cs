using CourseProgram.Services.DataServices;

namespace CourseProgram.Stores
{
    public class ServicesStore
    {
        public readonly DriverDataService _driverService;
        public readonly CategoryDataService _categoryService;
        public readonly DriverCategoriesDataService _driverCategoriesService;
        public ServicesStore() 
        {
            _driverService = new();
            _categoryService = new();
            _driverCategoriesService = new();
        }
    }
}