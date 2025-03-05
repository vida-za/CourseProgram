using CourseProgram.Models;
using CourseProgram.Stores;
using System.Linq;
using System.Threading.Tasks;

namespace CourseProgram.Controllers.DataControllers.EntityDataControllers
{
    public class OrderDataController : BaseDataController<Order>
    {
        public OrderDataController(DataStore dataStore) : base(dataStore) { }

        public Task<Order?> GetOrderByBud(int budId)
        {
            var tempData = _dataStore.GetArrayData<Order>();
            return Task.FromResult(tempData.FirstOrDefault(o => o.BudID == budId));
        }
    }
}