using CourseProgram.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CourseProgram.Controllers.DataControllers
{
    public interface IDataController<T> where T : IModel
    {
        Task<T> GetItemByID(int id);
        Task<IEnumerable<T>> GetItems();
    }
}