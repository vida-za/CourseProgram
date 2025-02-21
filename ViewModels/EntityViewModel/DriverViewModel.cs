using CourseProgram.Models;
using System;
using System.ComponentModel;

namespace CourseProgram.ViewModels.EntityViewModel
{
    public class DriverViewModel : BaseViewModel
    {
        private readonly Driver _model;
        public readonly int ID;

        [DisplayName("ФИО")]
        public string FIO => _model.FIO;
        [DisplayName("Список категорий")]
        public string StringCategories => _model.StringCategories != "" ? _model.StringCategories : "Нет категорий";
        [DisplayName("Дата рождения")]
        public string BirthDay => _model.BirthDay != null ? ((DateOnly)_model.BirthDay).ToString("d") : "-";
        [DisplayName("Паспортные данные")]
        public string Passport => _model.Passport;
        [DisplayName("Телефон")]
        public string Phone => _model.Phone;
        [DisplayName("Принят")]
        public string DateStart => _model.DateStart.ToString("d");
        [DisplayName("Уволен")]
        public string DateEnd => _model.DateEnd != null ? ((DateOnly)_model.DateEnd).ToString("d") : "-";

        public DriverViewModel(Driver driver)
        {
            _model = driver;

            ID = _model.ID;
        }

        public Driver GetModel() => _model;
    }
}