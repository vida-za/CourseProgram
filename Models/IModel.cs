namespace CourseProgram.Models
{
    public interface IModel
    {
        public int ID { get; }

        public static abstract string GetTable();
        public static abstract string GetSelectorID();
        public static abstract string[] GetFieldNames();
    }
}