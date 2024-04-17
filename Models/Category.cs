using System;

namespace CourseProgram.Models
{
    public class Category : IModel, IEquatable<Category>
    {
        public int ID { get; }
        public string Name { get; }
        public bool IsChecked { get; set; }

        public Category() 
        {
            ID = 0;
            Name = String.Empty;
            IsChecked = false;
        }

        public Category(int id, string name)
        {
            ID = id;
            Name = name;
            IsChecked = false;
        }

        public Category(int id, string name, bool check)
        {
            ID = id;
            Name = name;
            IsChecked = check;
        }

        public string GetSelectors() => "\"КодКатегории\", \"Наименование\"";
        public string GetTable() => "\"Категория\"";
        public string GetSelectorID() => "\"КодКатегории\"";
        public string GetProcedureDelete() => "\"DeleteCategory\"";

        public bool Equals(Category? other)
        {
            if (other != null)
                return ID == other.ID;
            else return false;
        }

        public override bool Equals(object obj) => Equals(obj as Category);

        public override int GetHashCode() => HashCode.Combine(ID, Name);
    }
}