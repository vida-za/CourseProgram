using CourseProgram.Models;
using CourseProgram.Stores;

namespace CourseProgram.Controllers.DataControllers.EntityDataControllers
{
    public class ClientDataController : BaseDataController<Client>
    {
        public ClientDataController(DataStore dataStore) : base(dataStore) { }
    }
}