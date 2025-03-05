using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace CourseProgram.Models
{
    public class Address : IModel, IEquatable<Address>
    {
        [DisplayName("КодАдреса")]
        public int ID { get; }
        [DisplayName("Город")]
        public string City { get; }
        [DisplayName("Улица")]
        public string Street { get; }
        [DisplayName("Дом")]
        public string? House { get; }
        [DisplayName("Строение")]
        public string? Structure { get; }
        [DisplayName("Корпус")]
        public string? Frame { get; }
        public string FullAddress { get; }

        public Address()
        {
            ID = 0;
            City = string.Empty;
            Street = string.Empty;
            House = null;
            Structure = null;
            Frame = null;

            FullAddress = SetFullAddress();
        }

        public Address(
            int id,
            string city,
            string street,
            string? house,
            string? structure,
            string? frame)
        {
            ID = id;
            City = city;
            Street = street;
            House = house;
            Structure = structure;
            Frame = frame;

            FullAddress = SetFullAddress();
        }

        public string SetFullAddress()
        {
            var parts = new List<string>
            {
                $"г.{City}",
                $"ул.{Street}"
            };

            if (!string.IsNullOrWhiteSpace(House))
                parts.Add($"д.{House}");

            if (!string.IsNullOrWhiteSpace(Structure))
                parts.Add($"стр.{Structure}");

            if (!string.IsNullOrWhiteSpace(Frame))
                parts.Add($"корп.{Frame}");

            return string.Join(", ", parts);
        }

        public static string GetTable() => "Адрес";
        public static string GetSelectorID() => "КодАдреса";
        public static string[] GetFieldNames()
        {
            return new[]
            {
                "КодАдреса",
                "Город",
                "Улица",
                "Дом",
                "Строение",
                "Корпус"
            };
        }

        public bool Equals(Address? other) => other != null && ID == other.ID;

        public override bool Equals(object obj) => Equals(obj as Address);

        public override int GetHashCode() => HashCode.Combine(ID, City);
    }
}