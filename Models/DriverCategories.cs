namespace CourseProgram.Models
{
    public class DriverCategories
    {
        public int DriverID { get; }
        public int CategoryID { get; }

        DriverCategories() 
        {
            DriverID = 0;
            CategoryID = 0;
        }

        DriverCategories(int driverID, int categoryID)
        {
            DriverID = driverID;
            CategoryID = categoryID;
        }
    }
}