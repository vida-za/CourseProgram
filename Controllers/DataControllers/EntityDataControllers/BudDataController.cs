using CourseProgram.Models;
using CourseProgram.Stores;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CourseProgram.Controllers.DataControllers.EntityDataControllers
{
    public class BudDataController : BaseDataController<Bud>
    {
        public BudDataController(DataStore dataStore) : base(dataStore) { }

        public Task<IEnumerable<Bud>> GetBudsByWorker(int workerId)
        {
            var tempData = _dataStore.GetArrayData<Bud>();
            return Task.FromResult(tempData.Where(b => b.WorkerID == workerId));
        }

        public Task<IEnumerable<Bud>> GetBudsByClient(int clientId)
        {
            var tempData = _dataStore.GetArrayData<Bud>();
            return Task.FromResult(tempData.Where(b => b.ClientID == clientId));
        }
    }
}