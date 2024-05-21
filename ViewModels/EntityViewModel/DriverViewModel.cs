using CourseProgram.Models;
using System.ComponentModel;

namespace CourseProgram.ViewModels.EntityViewModel
{
    public class DriverViewModel : BaseViewModel
    {
        private readonly Driver _model;

        [DisplayName("Номер водителя")]
        public int ID => _model.ID;
        [DisplayName("ФИО")]
        public string FIO => _model.FIO;
        [DisplayName("Дата рождения")]
        public string BirthDay => _model.BirthDay.ToString("d");
        [DisplayName("Паспортные данные")]
        public string Passport => _model.Passport;
        [DisplayName("Телефон")]
        public string Phone => _model.Phone;
        [DisplayName("Принят")]
        public string DateStart => _model.DateStart.ToString("d");
        [DisplayName("Уволен")]
        public string DateEnd => _model.DateEnd.ToString("d");
        [DisplayName("Текущая дислокация")]
        public string Town => _model.Town;

        public DriverViewModel(Driver driver)
        {
            _model = driver;
        }

        public Driver GetModel() => _model;
    }
}