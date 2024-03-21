using CourseProgram.Services.DataServices;

namespace CourseProgram.Stores
{
    public class ServicesStore
    {
        public readonly DriverDataService _driverService;
        public readonly CategoryDataService _categoryService;

        public ServicesStore() 
        {
            _driverService = new();
            _categoryService = new();
        }
    }
}