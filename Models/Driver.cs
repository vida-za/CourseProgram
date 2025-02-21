using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace CourseProgram.Models
{
    public class Driver : IModel, IEquatable<Driver> //Водитель
    {
        [DisplayName("КодВодителя")]
        public int ID { get; }
        [DisplayName("ФИО")]
        public string FIO { get; }
        [DisplayName("ДатаРождения")]
        public DateOnly? BirthDay { get; }
        [DisplayName("ПаспортныеДанные")]
        public string Passport { get; }
        [DisplayName("Телефон")]
        public string? Phone { get; }
        [DisplayName("ДатаНачала")]
        public DateOnly DateStart { get; }
        [DisplayName("ДатаОкончания")]
        public DateOnly? DateEnd { get; }
        public string StringCategories { get; private set; }

        private readonly List<Category> ListCategories = new List<Category>();

        public Driver()
        {
            ID = 0;
            FIO = string.Empty;
            BirthDay = null;
            Passport = string.Empty;
            Phone = null;
            DateStart = DateOnly.MinValue;
            DateEnd = null;
        }

        public Driver(
            int id,
            string fio,
            DateOnly? birthDay,
            string passport,
            string? phone,
            DateOnly dateStart,
            DateOnly? dateEnd)
        {
            ID = id;
            FIO = fio;
            BirthDay = birthDay;
            Passport = passport;
            Phone = phone;
            DateStart = dateStart;
            DateEnd = dateEnd;
        }

        public Driver(
            int id,
            string fio,
            DateOnly? birthDay,
            string passport,
            string? phone,
            DateOnly dateStart,
            DateOnly? dateEnd,
            params Category[] args)
        {
            ID = id;
            FIO = fio;
            BirthDay = birthDay;
            Passport = passport;
            Phone = phone;
            DateStart = dateStart;
            DateEnd = dateEnd;

            SetCategories(args);
        }

        public void SetCategories(params Category[] args)
        {
            foreach (Category arg in args)
            {
                if (arg.IsChecked)
                    ListCategories.Add(arg);
            }

            SetStringCategories();
        }

        public List<Category> GetListCategories()
        {
            return ListCategories;
        }

        public void SetStringCategories()
        {
            var temp = new List<string>();
            foreach (Category cat in ListCategories)
                temp.Add(cat.Name);

            StringCategories = string.Join(", ", temp);
        }

        public static string GetTable() => "Водитель";
        public static string GetSelectorID() => "КодВодителя";
        public static string[] GetFieldNames()
        {
            return new[]
            {
                "КодВодителя",
                "ФИО",
                "ДатаРождения",
                "ПаспортныеДанные",
                "Телефон",
                "ДатаНачала",
                "ДатаОкончания"
            };
        }

        public override string ToString()
        {
            return $"{ID}{FIO}{BirthDay}{Passport}{Phone}{DateStart}{DateEnd}";
        }

        public bool Equals(Driver? other) => other != null && ID == other.ID;

        public override int GetHashCode() => HashCode.Combine(ID, Passport);

        public static bool operator ==(Driver driver1, Driver driver2)
        {
            if (driver1 is null && driver2 is null) return true;

            return driver1 is not null && driver1.Equals(driver2);
        }

        public static bool operator !=(Driver driver1, Driver driver2) => !(driver1 == driver2);

        public override bool Equals(object obj) => Equals(obj as Driver);
    }
}