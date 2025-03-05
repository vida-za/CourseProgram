using CourseProgram.Models;
using CourseProgram.Services.DataServices;
using CourseProgram.Services.DataServices.ExtDataService;
using System;
using System.Collections.Generic;

namespace CourseProgram.Stores
{
    public class ServicesStore
    {
        private readonly Dictionary<Type, object> _services = new Dictionary<Type, object>();

        public readonly CategoryDataService _categoryService;

        public ServicesStore(DataStore dataStore) 
        {
            _services[typeof(Address)] = new AddressDataService(dataStore);
            _services[typeof(Bud)] = new BudDataService(dataStore);
            _services[typeof(Cargo)] = new CargoDataService(dataStore);
            _services[typeof(Client)] = new ClientDataService(dataStore );
            _services[typeof(Driver)] = new DriverDataService(dataStore);
            _services[typeof(DriverCategories)] = new DriverCategoriesDataService(dataStore);
            _services[typeof(Haul)] = new HaulDataService(dataStore);
            _services[typeof(Machine)] = new MachineDataService(dataStore);
            _services[typeof(Nomenclature)] = new NomenclatureDataService(dataStore);
            _services[typeof(Order)] = new OrderDataService(dataStore);
            _services[typeof(Route)] = new RouteDataService(dataStore);
            _services[typeof(Worker)] = new WorkerDataService(dataStore);

            _categoryService = new CategoryDataService();
        }

        public IDataService<T> GetService<T>() where T : IModel
        {
            if (_services.TryGetValue(typeof(T), out var service))
            {
                return (IDataService<T>)service;
            }

            throw new KeyNotFoundException($"Service for {typeof(T)} not found!");
        }

        public IEnumerable<object> GetAllServices()
        {
            return _services.Values;
        }
    }
}