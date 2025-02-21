using CourseProgram.Models;
using System;
using System.ComponentModel;

namespace CourseProgram.ViewModels.EntityViewModel
{
    public class WorkerViewModel : BaseViewModel
    {
        private readonly Worker _model;
        public readonly int ID;

        public Worker GetModel() => _model;

        [DisplayName("ФИО")]
        public string FIO => _model.FIO;
        [DisplayName("Дата рождения")]
        public string BirthDay => _model.BirthDay != null ? ((DateOnly)_model.BirthDay).ToString("d") : "-";
        [DisplayName("Паспортные данные")]
        public string Passport => _model.Passport;
        [DisplayName("Номер телефона")]
        public string Phone => _model.Phone;
        [DisplayName("Принят")]
        public string DateStart => _model.DateStart.ToString("d");
        [DisplayName("Уволен")]
        public string DateEnd => _model.DateEnd != null ? ((DateOnly)_model.DateEnd).ToString("d") : "-";

        public WorkerViewModel(Worker model)
        {
            _model = model;

            ID = model.ID;
        }
    }
}