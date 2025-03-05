using CourseProgram.Controllers.DataControllers;
using CourseProgram.Controllers.DataControllers.EntityDataControllers;
using CourseProgram.Models;
using System;
using System.Collections.Generic;

namespace CourseProgram.Stores
{
    public class ControllersStore
    {
        private readonly Dictionary<Type, object> _controllers = new Dictionary<Type, object>();

        public ControllersStore(DataStore dataStore)
        {
            _controllers[typeof(Address)] = new AddressDataController(dataStore);
            _controllers[typeof(Bud)] = new BudDataController(dataStore);
            _controllers[typeof(Cargo)] = new CargoDataController(dataStore);
            _controllers[typeof(Client)] = new ClientDataController(dataStore);
            _controllers[typeof(DriverCategories)] = new DriverCategoriesDataController(dataStore);
            _controllers[typeof(Driver)] = new DriverDataController(dataStore);
            _controllers[typeof(Haul)] = new HaulDataController(dataStore);
            _controllers[typeof(Machine)] = new MachineDataController(dataStore);
            _controllers[typeof(Nomenclature)] = new NomenclatureDataController(dataStore);
            _controllers[typeof(Order)] = new OrderDataController(dataStore);
            _controllers[typeof(Route)] = new RouteDataController(dataStore);
            _controllers[typeof(Worker)] = new WorkerDataController(dataStore);

        }

        public IDataController<T> GetController<T>() where T : IModel
        {
            if (_controllers.TryGetValue(typeof(T), out var controller))
            {
                return (IDataController<T>)controller;
            }

            throw new KeyNotFoundException($"Controller for {typeof(T)} not found!");
        }

        public IEnumerable<object> GetAllControllers()
        {
            return _controllers.Values;
        }
    }
}