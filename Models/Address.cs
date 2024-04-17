using System;
using System.ComponentModel;

namespace CourseProgram.Models
{
    public class Address : IModel, IEquatable<Address>
    {
        [DisplayName("Номер адреса")]
        public int ID { get; } //КодАдреса
        [DisplayName("Город")]
        public string City { get; } //Город
        [DisplayName("Улица")]
        public string Street { get; } //Улица
        [DisplayName("Дом")]
        public string House { get; } //Дом
        [DisplayName("Строение")]
        public string Structure { get; } //Строение
        [DisplayName("Корпус")]
        public string Frame { get; } //Корпус

        public Address()
        {
            ID = 0;
            City = string.Empty;
            Street = string.Empty;
            House = string.Empty;
            Structure = string.Empty;
            Frame = string.Empty;
        }

        public Address(
            int id,
            string city,
            string street,
            string house,
            string structure,
            string frame)
        {
            ID = id;
            City = city;
            Street = street;
            House = house;
            Structure = structure;
            Frame = frame;
        }

        public string GetSelectors() => "\"КодАдреса\", \"Город\", \"Улица\", \"Дом\", \"Строение\", \"Корпус\"";
        public string GetTable() => "\"Адрес\"";
        public string GetSelectorID() => "\"КодАдреса\"";
        public string GetProcedureDelete() => "\"DeleteAddress\"";

        public bool Equals(Address? other)
        {
            if (other != null)
                return ID == other.ID;
            else return false;
        }

        public override bool Equals(object obj) => Equals(obj as Address);

        public override int GetHashCode() => HashCode.Combine(ID, City);
    }
}