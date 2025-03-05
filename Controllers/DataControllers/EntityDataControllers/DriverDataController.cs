using CourseProgram.Models;
using CourseProgram.Stores;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CourseProgram.Controllers.DataControllers.EntityDataControllers
{
    public class DriverDataController : BaseDataController<Driver>
    {
        public DriverDataController(DataStore dataStore) : base(dataStore) { }

        public Task<IEnumerable<Driver>> GetActiveDrivers(bool active)
        {
            var tempData = _dataStore.GetArrayData<Driver>();
            if (active)
                return Task.FromResult(tempData.Where(d => d.DateEnd == null));
            else
                return Task.FromResult(tempData.Where(d => d.DateEnd != null));
        }

        public Task<IEnumerable<Driver>> GetDriversHasRoutes()
        {
            var tempData = _dataStore.GetArrayData<Route>();
            var listDriversID = tempData.Where(r => r.DriverID != null && r.Status == Constants.RouteStatusValues.Waiting).Select(r => r.DriverID).Distinct();
            var listDrivers = _dataStore.GetArrayData<Driver>();
            return Task.FromResult(listDrivers.Where(d => listDriversID.Contains(d.ID)));
        }
    }
}