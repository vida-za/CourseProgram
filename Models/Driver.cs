using System;
using static CourseProgram.Models.Constants;

namespace CourseProgram.Models
{
    public class Driver : IModel //Водитель
    {
        public int ID { get; }
        public string FIO { get; } //ФИО
        public DateTime BirthDay { get; } //ДатаРождения
        public string Passport { get; } //ПаспортныеДанные
        public string? Phone { get; } //Телефон
        public DateTime DateStart { get; } //ДатаНачала
        public DateTime DateEnd { get; } //ДатаОкончания 
        public string Town { get; } //Город

        public Driver()
        {
            ID = -1;
            FIO = string.Empty;
            BirthDay = new DateTime();
            Passport = string.Empty;
            Phone = string.Empty;
            DateStart = new DateTime();
            DateEnd = new DateTime();
            Town = string.Empty;
        }

        public Driver(int id, string fio, DateTime birthDay, string passport, string phone, DateTime dateStart, DateTime dateEnd, string town)
        {
            ID = id;
            FIO = fio;
            BirthDay = birthDay;
            Passport = passport;
            Phone = phone;
            DateStart = dateStart;
            DateEnd = dateEnd;
            Town = town;
        }

        public static string GetSelectors() => "\"ФИО\", \"ДатаРождения\", \"ПаспортныеДанные\", \"Телефон\", \"ДатаНачала\", \"ДатаОкончания\"";

        public static string GetTable() => "\"Водитель\"";

        public static string GetSelectorID() => "\"КодВодителя\"";

        public override string ToString()
        {
            return $"{ID}{FIO}{BirthDay}{Passport}{Phone}{DateStart}{DateEnd}";
        }

        public override bool Equals(object obj) => obj is Driver driver && ID == driver.ID;

        public override int GetHashCode() => HashCode.Combine(ID, Passport);

        public static bool operator ==(Driver driver1, Driver driver2)
        {
            if (driver1 is null && driver2 is null) return true;

            return driver1 is not null && driver1.Equals(driver2);
        }

        public static bool operator !=(Driver driver1, Driver driver2) => !(driver1 == driver2);
    }
}