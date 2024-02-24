using System;
using System.ComponentModel;

namespace CourseProgram.Models
{
    public class Driver : IModel //Водитель
    {
        [DisplayName("Номер водителя")]
        public int? ID { get; set; } //КодВодителя
        [DisplayName("Категория")]
        public string? Category { get; set; } //Категория
        [DisplayName("ФИО")]
        public string? FIO { get; set; } //ФИО
        [DisplayName("Дата рождения")]
        public DateOnly? BirthDay { get; set; } //ДатаРождения
        [DisplayName("Паспортные данные")]
        public string? Passport { get; set; } //ПаспортныеДанные
        [DisplayName("Телефон")]
        public string? Phone { get; set; } //Телефон
        [DisplayName("Дата начала")]
        public DateOnly? DateStart { get; set; } //ДатаНачала
        [DisplayName("Дата окончания")]
        public DateOnly? DateEnd { get; set; } //ДатаОкончания 

        public string GetSelectors() => " \"КодВодителя\", \"Категория\", \"ФИО\", \"ДатаРождения\", \"ПаспортныеДанные\", \"Телефон\", \"ДатаНачала\", \"ДатаОкончания\"";

        public string GetTable() => " From \"Водитель\";";
    }
}