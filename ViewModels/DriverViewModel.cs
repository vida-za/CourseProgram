using CourseProgram.Commands;
using CourseProgram.Models;
using CourseProgram.Services;
using CourseProgram.Stores;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace CourseProgram.ViewModels
{
    public class DriverViewModel : BaseViewModel
    {
        private readonly Driver _driver;

        public int ID => _driver.ID;
        public string FIO => _driver.FIO;
        public string BirthDay => _driver.BirthDay.ToString("d");
        public string Passport => _driver.Passport;
        public string Phone => _driver.Phone;
        public string DateStart => _driver.DateStart.ToString("d");
        public string DateEnd => _driver.DateEnd.ToString("d");
        public string Town => _driver.Town;

        public DriverViewModel(Driver driver)
        {
            _driver = driver;
        }

        public Driver GetDriver() => _driver;
    }
}