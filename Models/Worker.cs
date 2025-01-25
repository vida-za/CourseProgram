using System;
using System.ComponentModel;

namespace CourseProgram.Models
{
    public class Worker : IModel, IEquatable<Worker>
    {
        [DisplayName("КодСотрудника")]
        public int ID { get; }
        [DisplayName("ФИО")]
        public string FIO { get; }
        [DisplayName("ДатаРождения")]
        public DateOnly BirthDay { get; }
        [DisplayName("ПаспортныеДанные")]
        public string Passport { get; }
        [DisplayName("Телефон")]
        public string Phone { get; }
        [DisplayName("ДатаНачалаРаботы")]
        public DateOnly DateStart { get; }
        [DisplayName("ДатаОкончанияРаботы")]
        public DateOnly DateEnd { get; }

        public Worker()
        {
            ID = 0;
            FIO = string.Empty;
            BirthDay = DateOnly.MinValue;
            Passport = string.Empty;
            Phone = string.Empty;
            DateStart = DateOnly.MinValue;
            DateEnd = DateOnly.MinValue;
        }

        public Worker(int id, string fio, DateOnly birthDay, string passport, string phone, DateOnly dateStart, DateOnly dateEnd)
        {
            ID = id;
            FIO = fio;
            BirthDay = birthDay;
            Passport = passport;
            Phone = phone;
            DateStart = dateStart;
            DateEnd = dateEnd;
        }

        public static string GetTable() => "Сотрудник";
        public static string GetSelectorID() => "КодСотрудника";
        public static string[] GetFieldNames()
        {
            return new[]
            {
                "КодСотрудника",
                "ФИО",
                "ДатаРождения",
                "ПаспортныеДанные",
                "Телефон",
                "ДатаНачалаРаботы",
                "ДатаОкончанияРаботы"
            };
        }

        public bool Equals(Worker? other) => other != null && ID == other.ID;

        public override bool Equals(object obj) => Equals(obj as Worker);

        public override int GetHashCode() => HashCode.Combine(ID, FIO);
    }
}