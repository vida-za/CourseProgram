using System;

namespace CourseProgram.Models
{
    public class Category : IModel
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

        public static string GetSelectors() => "\"Наименование\"";

        public static string GetTable() => "\"Категория\"";

        public static string GetSelectorID() => "\"КодКатегории\"";
    }
}