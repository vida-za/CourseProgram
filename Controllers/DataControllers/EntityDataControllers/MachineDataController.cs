using CourseProgram.Models;
using CourseProgram.Stores;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CourseProgram.Controllers.DataControllers.EntityDataControllers
{
    public class MachineDataController : BaseDataController<Machine>
    {
        public MachineDataController(DataStore dataStore) : base(dataStore) { }

        public Task<IEnumerable<Machine>> GetActiveMachines(bool active)
        {
            var tempData = _dataStore.GetArrayData<Machine>();
            if (active)
                return Task.FromResult(tempData.Where(m => m.TimeEnd == null));
            else
                return Task.FromResult(tempData.Where(m => m.TimeEnd != null));
        }
    }
}