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

        public List<Driver> GetDriversAll() => _drivers;

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

        public void RemoveDriver(Driver driver) 
        {
            {
                for (int i = 0; i < _drivers.Count; i++) 
                {
                    if (_drivers[i] == driver) 
                    { 
                        _drivers.RemoveAt(i); 
                    }
                }
            }
        }
    }
}