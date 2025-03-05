using CourseProgram.Models;
using CourseProgram.Stores;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CourseProgram.Controllers.DataControllers.EntityDataControllers
{
    public class RouteDataController : BaseDataController<Route>
    {
        public RouteDataController(DataStore dataStore) : base(dataStore) { }

        public Task<IEnumerable<Route>> GetRoutesByDriver(int driverId)
        {
            var tempData = _dataStore.GetArrayData<Route>();
            return Task.FromResult(tempData.Where(r => r.DriverID == driverId && r.Status != Constants.RouteStatusValues.Cancelled));
        }

        public Task<IEnumerable<Route>> GetRoutesByMachine(int machineId)
        {
            var tempData = _dataStore.GetArrayData<Route>();
            return Task.FromResult(tempData.Where(r => r.MachineID == machineId && r.Status != Constants.RouteStatusValues.Cancelled));
        }
    }
}