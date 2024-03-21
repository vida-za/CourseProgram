using System.Collections.Generic;

namespace CourseProgram.Services.DataServices
{
    public abstract class BaseService<T>
    {
        protected DBConnection cnn;
        protected List<T> items = new();
        protected string query = string.Empty;
    }
}