using System;
using System.ComponentModel;

namespace CourseProgram.Models
{
    public class DriverCategories : IModel, IEquatable<DriverCategories>
    {
        public int ID { get; }
        [DisplayName("КодВодителя")]
        public int DriverID { get; }
        [DisplayName("КодКатегории")]
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

        public static string GetTable() => "Категории_водителя";
        public static string GetSelectorID() => "";
        public static string[] GetFieldNames()
        {
            return new[]
            {
                "КодВодителя",
                "КодКатегории"
            };
        }

        public bool Equals(DriverCategories? other) => other != null && ID == other.ID;

        public override bool Equals(object obj) => Equals(obj as DriverCategories);

        public override int GetHashCode() => HashCode.Combine(DriverID, CategoryID);
    }
}