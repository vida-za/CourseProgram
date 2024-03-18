using CourseProgram.Models;

namespace CourseProgram.ViewModels
{
    public class DriverViewModel : BaseViewModel
    {
        private readonly Driver _driver;

        //public int ID => _driver.ID;
        public string Category => _driver.Category.ToString();
        public string FIO => _driver.FIO;
        public string BirthDay => _driver.BirthDay.ToString("d");
        public string Passport => _driver.Passport;
        public string? Phone => _driver.Phone;
        public string DateStart => _driver.DateStart.ToString("d");
        public string DateEnd => _driver.DateEnd.ToString("d");

        public DriverViewModel(Driver driver)
        {
            _driver = driver;
        }

        public Driver GetDriver()
        {
            return _driver;
        }
    }
}