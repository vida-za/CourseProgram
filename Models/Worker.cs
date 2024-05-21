using System;
using System.ComponentModel;

namespace CourseProgram.Models
{
    public class Worker : IModel, IEquatable<Worker>
    {
        [DisplayName("Номер сотрудника")]
        public int ID { get; } //КодСотрудника
        [DisplayName("ФИО")]
        public string FIO { get; } //ФИО
        [DisplayName("Дата рождения")]
        public DateOnly BirthDay { get; } //ДатаРождения
        [DisplayName("Паспортные данные")]
        public string Passport { get; } //ПаспортныеДанные
        [DisplayName("Номер телефона")]
        public string Phone { get; } //Телефон
        [DisplayName("Принят")]
        public DateOnly DateStart { get; } //ДатаНачала
        [DisplayName("Уволен")]
        public DateOnly DateEnd { get; } //ДатаОкончания

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

        public string GetSelectors() => "\"КодСотрудника\", \"ФИО\", \"ДатаРождения\", \"ПаспортныеДанные\", \"Телефон\", \"ДатаНачала\", \"ДатаОкончания\"";
        public string GetTable() => "\"Сотрудник\"";
        public string GetSelectorID() => "\"КодСотрудника\"";
        public string GetProcedureDelete() => "\"DeleteWorker\"";

        public bool Equals(Worker? other)
        {
            if (other != null)
                return ID == other.ID;
            else return false;
        }

        public override bool Equals(object obj) => Equals(obj as Worker);

        public override int GetHashCode() => HashCode.Combine(ID, FIO);
    }
}