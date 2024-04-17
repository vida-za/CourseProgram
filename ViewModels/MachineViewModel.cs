using CourseProgram.Models;

namespace CourseProgram.ViewModels
{
    public class MachineViewModel : BaseViewModel
    {
        private readonly Machine _model;

        public Machine GetModel() => _model;

        public int ID => _model.ID;
        public string TypeMachine => _model.TypeMachine;
        public string TypeBodywork => _model.TypeBodywork;
        public string TypeLoading => _model.TypeLoading;
        public float LoadCapacity => _model.LoadCapacity;
        public float Volume => _model.Volume;
        public bool HydroBoard => _model.HydroBoard;
        public float LengthBodywork => _model.LengthBodywork;
        public float WidthBodywork => _model.WidthBodywork;
        public float HeightBodywork => _model.HeightBodywork;
        public string Stamp => _model.Stamp;
        public string Name => _model.Name;
        public string StateNumber => _model.StateNumber;
        public string Status => _model.Status;
        public string TimeStart => _model.TimeStart.ToString("d");
        public string TimeEnd => _model.TimeEnd.ToString("d");
        public string Town => _model.Town;

        public MachineViewModel(Machine machine)
        {
            _model = machine;
        }
    }
}