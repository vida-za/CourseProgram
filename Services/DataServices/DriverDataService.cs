using CourseProgram.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CourseProgram.Services.DataServices
{
    public class DriverDataService : IDataService<Driver>
    {
        public DriverDataService() 
        {
            
        }

        public Task<bool> AddItemAsync(Driver item)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteItemAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Driver> GetItemAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Driver>> GetItemsAsync(bool forceRefresh = false)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateItemAsync(Driver item)
        {
            throw new NotImplementedException();
        }
    }
}