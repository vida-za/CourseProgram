using CourseProgram.Models;
using CourseProgram.Stores;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CourseProgram.Controllers.DataControllers.EntityDataControllers
{
    public class CargoDataController : BaseDataController<Cargo>
    {
        public CargoDataController(DataStore dataStore) : base(dataStore) { }

        public Task<IEnumerable<Cargo>> GetCargosByBud(int budId)
        {
            var tempData = _dataStore.GetArrayData<Cargo>();
            return Task.FromResult(tempData.Where(c => c.BudID == budId));
        }
    }
}