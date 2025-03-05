using CourseProgram.Models;
using CourseProgram.Stores;
using System.Linq;
using System.Threading.Tasks;

namespace CourseProgram.Controllers.DataControllers.EntityDataControllers
{
    public class HaulDataController : BaseDataController<Haul>
    {
        public HaulDataController(DataStore dataStore) : base(dataStore) { }

        public Task<Haul?> GetCurrentHaul()
        {
            var tempData = _dataStore.GetArrayData<Haul>();
            return Task.FromResult(tempData.FirstOrDefault(h => h.DateEnd == null));
        }
    }
}