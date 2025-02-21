namespace CourseProgram.Models
{
    public interface IModel
    {
        int ID { get; }

        static abstract string GetTable();
        static abstract string GetSelectorID();
        static abstract string[] GetFieldNames();
    }
}