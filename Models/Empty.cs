namespace CourseProgram.Models
{
    public class Empty : IModel
    {
        public int ID => 0;


        public static string GetSelectorID() => string.Empty;

        public static string[] GetFieldNames() => System.Array.Empty<string>();

        public static string GetTable() => string.Empty;
    }
}