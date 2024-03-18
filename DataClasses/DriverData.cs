using System.Collections.Generic;
using CourseProgram.Models;
using CourseProgram.Exceptions;

namespace CourseProgram.DataClasses
{
    public class DriverData
    {
        private readonly List<Driver> _drivers;

        public DriverData()
        {
            _drivers = new List<Driver>();
        }

        public IEnumerable<Driver> GetDriversAll() => _drivers;

        public void AddDriver(Driver driver) 
        {
            foreach (Driver existDriver in _drivers) 
            {
                if (existDriver == driver)
                {
                    throw new RepeatConflictException<Driver>(existDriver, driver);
                }
            }

            _drivers.Add(driver);
        }
    }
}