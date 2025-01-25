using CourseProgram.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static CourseProgram.Models.Constants;

namespace CourseProgram.Services.DataServices.ExtDataService
{
    public class CategoryDataService
    {
        public List<Category> items;

        public CategoryDataService()
        {
            items = new List<Category>();
        }

        public async Task<IEnumerable<Category>> GetTemplateList()
        {
            try
            {
                items.Clear();
                foreach (Categories cat in Enum.GetValues(typeof(Categories))) 
                {
                    items.Add(new Category(cat));
                }
            }
            catch (Exception) { }
            return await Task.FromResult(items);
        }
    }
}