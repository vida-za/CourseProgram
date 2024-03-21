using System.Collections.Generic;
using System.Threading.Tasks;

namespace CourseProgram.Services.DataServices
{
    public interface IDataService<T>
    {
        int FindMaxID();
        Task<bool> AddItemAsync(T item);
        Task<bool> UpdateItemAsync(T item);
        Task<bool> DeleteItemAsync(int id);
        Task<T> GetItemAsync(int id);
        Task<IEnumerable<T>> GetItemsAsync(bool forceRefresh = false);
    }
}