using CourseProgram.Models;
using CourseProgram.Stores;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CourseProgram.Controllers.DataControllers.EntityDataControllers
{
    public class DriverCategoriesDataController : BaseDataController<DriverCategories>
    {
        public DriverCategoriesDataController(DataStore dataStore) : base(dataStore) { }

        public Task<IEnumerable<DriverCategories>> GetDriverCategories(int driverID)
        {
            var tempData = _dataStore.GetArrayData<DriverCategories>();
            return Task.FromResult(tempData.Where(dc => dc.DriverID == driverID));
        }
    }
}