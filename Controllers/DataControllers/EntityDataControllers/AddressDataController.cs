using CourseProgram.Models;
using CourseProgram.Stores;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CourseProgram.Controllers.DataControllers.EntityDataControllers
{
    public class AddressDataController : BaseDataController<Address>
    {
        public AddressDataController(DataStore dataStore) : base(dataStore) { }
    }
}