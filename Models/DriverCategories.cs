namespace CourseProgram.Models
{
    public class DriverCategories
    {
        public int DriverID { get; }
        public int CategoryID { get; }

        public DriverCategories() 
        {
            DriverID = 0;
            CategoryID = 0;
        }

        public DriverCategories(int driverID, int categoryID)
        {
            DriverID = driverID;
            CategoryID = categoryID;
        }

        public static string GetSelectors() => "\"КодВодителя\", \"КодКатегории\"";

        public static string GetTable() => "\"Категории_водителя\"";
    }
}