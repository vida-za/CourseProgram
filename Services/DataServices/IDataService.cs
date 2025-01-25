using CourseProgram.Models;
using CourseProgram.Services.DBServices;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace CourseProgram.Services.DataServices
{
    public interface IDataService<T> where T : IModel
    {
        Task FindMaxEmptyID();
        Task<bool> AddItemAsync(T item);
        Task<bool> UpdateItemAsync(T item);
        Task<bool> DeleteItemAsync(int id);
        Task<T> GetItemAsync(int id);
        Task<IEnumerable<T>> GetItemsAsync();
        void CreateElement(DataRow row);
        void FillInsertParams(Query query, T item);
    }
}