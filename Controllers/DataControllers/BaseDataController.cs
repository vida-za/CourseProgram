using CourseProgram.Models;
using CourseProgram.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CourseProgram.Controllers.DataControllers
{
    public abstract class BaseDataController<T> : IDataController<T> where T : IModel, IEquatable<T>
    {
        protected DataStore _dataStore;

        public BaseDataController(DataStore dataStore)
        {
            _dataStore = dataStore;
        }

        public Task<T> GetItemByID(int id)
        {
            var tempData = _dataStore.GetArrayData<T>();
            return Task.FromResult(tempData.FirstOrDefault(d => d.ID == id));
        }

        public Task<IEnumerable<T>> GetItems()
        {
            var tempData = _dataStore.GetArrayData<T>();
            return Task.FromResult<IEnumerable<T>>(tempData);
        }
    }
}