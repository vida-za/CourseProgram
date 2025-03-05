using CourseProgram.Models;
using CourseProgram.Stores;

namespace CourseProgram.Controllers.DataControllers.EntityDataControllers
{
    public class NomenclatureDataController : BaseDataController<Nomenclature>
    {
        public NomenclatureDataController(DataStore dataStore) : base(dataStore) { }
    }
}