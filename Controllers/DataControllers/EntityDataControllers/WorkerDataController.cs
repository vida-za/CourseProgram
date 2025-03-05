using CourseProgram.Models;
using CourseProgram.Stores;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CourseProgram.Controllers.DataControllers.EntityDataControllers
{
    public class WorkerDataController : BaseDataController<Worker>
    {
        public WorkerDataController(DataStore dataStore) : base(dataStore) { }

        public Task<IEnumerable<Worker>> GetActiveWorkers(bool active)
        {
            var tempData = _dataStore.GetArrayData<Worker>();
            if (active)
                return Task.FromResult(tempData.Where(w => w.DateEnd == null));
            else
                return Task.FromResult(tempData.Where(w => w.DateEnd != null));
        }
    }
}