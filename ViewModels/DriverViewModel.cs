using CourseProgram.Models;

namespace CourseProgram.ViewModels
{
    public class DriverViewModel : BaseViewModel
    {
        private readonly Driver _model;

        public int ID => _model.ID;
        public string FIO => _model.FIO;
        public string BirthDay => _model.BirthDay.ToString("d");
        public string Passport => _model.Passport;
        public string Phone => _model.Phone;
        public string DateStart => _model.DateStart.ToString("d");
        public string DateEnd => _model.DateEnd.ToString("d");
        public string Town => _model.Town;

        public DriverViewModel(Driver driver)
        {
            _model = driver;
        }

        public Driver GetModel() => _model;
    }
}