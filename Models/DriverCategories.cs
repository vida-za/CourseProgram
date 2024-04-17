using System;

namespace CourseProgram.Models
{
    public class DriverCategories : IModel, IEquatable<DriverCategories>
    {
        public int ID { get; }
        public int DriverID { get; }
        public int CategoryID { get; }

        public DriverCategories() 
        {
            ID = 0;
            DriverID = 0;
            CategoryID = 0;
        }

        public DriverCategories(int driverID, int categoryID)
        {
            ID = 0;
            DriverID = driverID;
            CategoryID = categoryID;
        }

        public string GetSelectors() => "\"КодВодителя\", \"КодКатегории\"";
        public string GetTable() => "\"Категории_водителя\"";
        public string GetSelectorID() => "\"\"";
        public string GetProcedureDelete() => "\"DeleteDriverCategories\"";

        public bool Equals(DriverCategories? other)
        {
            if (other != null)
                return DriverID == other.DriverID && CategoryID == other.CategoryID;
            else return false;
        }

        public override bool Equals(object obj) => Equals(obj as DriverCategories);

        public override int GetHashCode() => HashCode.Combine(DriverID, CategoryID);
    }
}