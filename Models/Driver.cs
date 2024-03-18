using System;
using static CourseProgram.Models.Constants;

namespace CourseProgram.Models
{
    public class Driver //Водитель
    {
        //public int ID { get; } //КодВодителя
        public DriverCategory Category { get; } //Категория
        public string FIO { get; } //ФИО
        public DateTime BirthDay { get; } //ДатаРождения
        public string Passport { get; } //ПаспортныеДанные
        public string? Phone { get; } //Телефон
        public DateTime DateStart { get; } //ДатаНачала
        public DateTime DateEnd { get; } //ДатаОкончания 

        public Driver()
        {
            Category = new DriverCategory();
            FIO = string.Empty;
            BirthDay = new DateTime();
            Passport = string.Empty;
            Phone = string.Empty;
            DateStart = new DateTime();
            DateEnd = new DateTime();
        }

        public Driver(/*int id,*/ DriverCategory category, string fio, DateTime birthDay, string passport, string phone, DateTime dateStart, DateTime dateEnd)
        {
            //ID = id;
            Category = category;
            FIO = fio;
            BirthDay = birthDay;
            Passport = passport;
            Phone = phone;
            DateStart = dateStart;
            DateEnd = dateEnd;
        }

        public string GetSelectors() => " \"КодВодителя\", \"Категория\", \"ФИО\", \"ДатаРождения\", \"ПаспортныеДанные\", \"Телефон\", \"ДатаНачала\", \"ДатаОкончания\"";

        public string GetTable() => " From \"Водитель\";";

        public override string ToString()
        {
            return $"{Category}{FIO}{BirthDay}{Passport}{Phone}{DateStart}{DateEnd}";
        }

        public override bool Equals(object obj) => obj is Driver driver && Passport == driver.Passport;

        public override int GetHashCode()
        {
            return HashCode.Combine(Passport);
        }

        public static bool operator ==(Driver driver1, Driver driver2)
        {
            if (driver1 is null && driver2 is null) return true;

            return driver1 is not null && driver1.Equals(driver2);
        }

        public static bool operator !=(Driver driver1, Driver driver2)
        {
            return !(driver1 == driver2);
        }
    }
}