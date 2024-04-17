using System;
using System.ComponentModel;

namespace CourseProgram.Models
{
    public class Cargo : IModel, IEquatable<Cargo>
    {
        [DisplayName("Номер груза")]
        public int ID { get; } //КодГруза
        [DisplayName("Номер заказа")]
        public int OrderID { get; } //КодЗаказа
        [DisplayName("Длина")]
        public float Length { get; } //Длина
        [DisplayName("Ширина")]
        public float Width { get; } //Ширина
        [DisplayName("Высота")]
        public float Height { get; } //Высота
        [DisplayName("Тип")]
        public string Type { get; } //Тип 
        [DisplayName("Категория")]
        public string Category { get; } //Категория
        [DisplayName("Вес")]
        public float Weight { get; } //Вес
        [DisplayName("Количество")]
        public int Count { get; } //Количество

        public Cargo()
        {
            ID = 0;
            OrderID = 0;
            Length = 0;
            Width = 0;
            Height = 0;
            Type = string.Empty;
            Category = string.Empty;
            Weight = 0;
            Count = 0;
        }

        public Cargo(
            int id,
            int orderID,
            float length,
            float width,
            float height,
            string type,
            string category,
            float weight,
            int count)
        {
            ID = id;
            OrderID = orderID;
            Length = length;
            Width = width;
            Height = height;
            Type = type;
            Category = category;
            Weight = weight;
            Count = count;
        }

        public string GetSelectors() => "\"КодГруза\", \"КодЗаказа\", \"Длина\", \"Ширина\", \"Высота\", \"Тип\", \"Категория\", \"Вес\", \"Количество\"";
        public string GetTable() => "\"Груз\"";
        public string GetSelectorID() => "\"КодГруза\"";
        public string GetProcedureDelete() => "\"DeleteCargo\"";

        public bool Equals(Cargo? other)
        {
            if (other != null)
                return ID == other.ID;
            else return false;
        }

        public override bool Equals(object obj) => Equals(obj as Cargo);

        public override int GetHashCode() => HashCode.Combine(ID, OrderID);
    }
}