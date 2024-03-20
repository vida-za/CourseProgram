using CourseProgram.Services.DataServices;

namespace CourseProgram.Stores
{
    public class ServicesStore
    {
        private readonly DriverDataService _driverService;

        public ServicesStore() 
        {
            _driverService = new();
        }
    }
}