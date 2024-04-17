using System;
using System.ComponentModel;

namespace CourseProgram.Models
{
    public class Driver : IModel, IEquatable<Driver> //Водитель
    {
        [DisplayName("Номер водителя")]
        public int ID { get; } //КодВодителя
        [DisplayName("ФИО")]
        public string FIO { get; } //ФИО
        [DisplayName("Дата рождения")]
        public DateOnly BirthDay { get; } //ДатаРождения
        [DisplayName("Паспорт")]
        public string Passport { get; } //ПаспортныеДанные
        [DisplayName("Телефон")]
        public string? Phone { get; } //Телефон
        [DisplayName("Принят")]
        public DateOnly DateStart { get; } //ДатаНачала
        [DisplayName("Уволен")]
        public DateOnly DateEnd { get; } //ДатаОкончания
        [DisplayName("Город")]
        public string Town { get; } //Город

        public Driver()
        {
            ID = 0;
            FIO = string.Empty;
            BirthDay = DateOnly.MinValue;
            Passport = string.Empty;
            Phone = string.Empty;
            DateStart = DateOnly.MinValue;
            DateEnd = DateOnly.MinValue;
            Town = string.Empty;
        }

        public Driver(
            int id,
            string fio,
            DateOnly birthDay,
            string passport,
            string phone,
            DateOnly dateStart,
            DateOnly dateEnd,
            string town)
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

        public string GetSelectors() => "\"КодВодителя\", \"ФИО\", \"ДатаРождения\", \"ПаспортныеДанные\", \"Телефон\", \"ДатаНачала\", \"ДатаОкончания\"";
        public string GetTable() => "\"Водитель\"";
        public string GetSelectorID() => "\"КодВодителя\"";
        public string GetProcedureDelete() => "\"DeleteDriver\"";

        public override string ToString()
        {
            return $"{ID}{FIO}{BirthDay}{Passport}{Phone}{DateStart}{DateEnd}";
        }

        public bool Equals(Driver? other)
        {
            if (other != null)
                return ID == other.ID;
            else return false;
        }

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