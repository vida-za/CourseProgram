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

        public ServicesStore() 
        {
            _services[typeof(Address)] = new AddressDataService();
            _services[typeof(Bud)] = new BudDataService();
            _services[typeof(Cargo)] = new CargoDataService();
            _services[typeof(Client)] = new ClientDataService();
            _services[typeof(Driver)] = new DriverDataService();
            _services[typeof(DriverCategories)] = new DriverCategoriesDataService();
            _services[typeof(Haul)] = new HaulDataService();
            _services[typeof(Machine)] = new MachineDataService();
            _services[typeof(Nomenclature)] = new NomenclatureDataService();
            _services[typeof(Order)] = new OrderDataService();
            _services[typeof(Route)] = new RouteDataService();
            _services[typeof(Worker)] = new WorkerDataService();

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