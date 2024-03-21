using System;

namespace CourseProgram.Models
{
    public class Category
    {
        public int ID { get; }
        public string Name { get; }

        public Category() 
        {
            ID = 0;
            Name = String.Empty;
        }

        public Category(int id, string name)
        {
            ID = id;
            Name = name;
        }

        public static string GetSelectors() => "\"Наименование\"";

        public static string GetTable() => "\"Категория\"";

        public static string GetSelectorID() => "\"КодКатегории\"";
    }
}