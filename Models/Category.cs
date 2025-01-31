using System;
using static CourseProgram.Models.Constants;

namespace CourseProgram.Models
{
    public class Category : IEquatable<Category>
    {
        public Categories EnumCategory { get; }
        public string Name { get; }
        public bool IsChecked { get; set; }

        public Category() 
        {
            EnumCategory = Categories.B;
            Name = String.Empty;
            IsChecked = false;
        }

        public Category(Categories enumCategory)
        {
            EnumCategory = enumCategory;
            Name = GetEnumDescription(enumCategory);
            IsChecked = false;
        }

        public Category(Categories enumCategory, bool check)
        {
            EnumCategory = enumCategory;
            Name = GetEnumDescription(enumCategory);
            IsChecked = check;
        }

        public Category(DriverCategories driverCategories)
        {
            EnumCategory = (Categories)driverCategories.CategoryID;
            Name = GetEnumDescription(EnumCategory);
            IsChecked = true;
        }

        public bool Equals(Category? other)
        {
            if (other != null)
                return Name == other.Name;
            else return false;
        }

        public override bool Equals(object obj) => Equals(obj as Category);

        public override int GetHashCode() => HashCode.Combine(Name);
    }
}