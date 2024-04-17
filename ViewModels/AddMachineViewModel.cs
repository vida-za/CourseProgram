using CourseProgram.Commands;
using CourseProgram.Services;
using CourseProgram.Stores;
using System.Windows.Input;

namespace CourseProgram.ViewModels
{
    public class AddMachineViewModel : BaseViewModel
    {
        public AddMachineViewModel(ServicesStore servicesStore, INavigationService machineViewNavigationService)
        {
            SubmitCommand = new AddMachineCommand(this, servicesStore, machineViewNavigationService);
            CancelCommand = new NavigateCommand(machineViewNavigationService);
        }

        public ICommand SubmitCommand { get; }
        public ICommand CancelCommand { get; }

        private string _typeMachine;
        public string TypeMachine
        {
            get => _typeMachine;
            set
            {
                _typeMachine = value;
                OnPropertyChanged(nameof(TypeMachine));
            }
        }

        private string _typeBodywork;
        public string TypeBodywork
        {
            get => _typeBodywork;
            set
            {
                _typeBodywork = value;
                OnPropertyChanged(nameof(TypeBodywork));
            }
        }

        private string _typeLoading;
        public string TypeLoading
        {
            get => _typeLoading;
            set
            {
                _typeLoading = value;
                OnPropertyChanged(nameof(TypeLoading));
            }
        }

        private string _loadCapacity;
        public string LoadCapacity
        {
            get => _loadCapacity;
            set
            {
                _loadCapacity = value;
                OnPropertyChanged(nameof(LoadCapacity));
            }
        }

        private string _volume;
        public string Volume
        {
            get => _volume;
            set
            {
                _volume = value;
                OnPropertyChanged(nameof(Volume));
            }
        }

        private bool _hydroBoard;
        public bool HydroBoard
        {
            get => _hydroBoard;
            set
            {
                _hydroBoard = value;
                OnPropertyChanged(nameof(HydroBoard));
            }
        }

        private string _lengthBodywork;
        public string LengthBodywork
        {
            get => _lengthBodywork;
            set
            {
                _lengthBodywork = value;
                OnPropertyChanged(nameof(LengthBodywork));
            }
        }

        private string _widthBodywork;
        public string WidthBodywork
        {
            get => _widthBodywork;
            set
            {
                _widthBodywork = value;
                OnPropertyChanged(nameof(WidthBodywork));
            }
        }

        private string _heightBodywork;
        public string HeightBodywork
        {
            get => _heightBodywork;
            set
            {
                _heightBodywork = value;
                OnPropertyChanged(nameof(HeightBodywork));
            }
        }

        private string _stamp;
        public string Stamp
        {
            get => _stamp;
            set
            {
                _stamp = value;
                OnPropertyChanged(nameof(Stamp));
            }
        }

        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        private string _stateNumber;
        public string StateNumber
        {
            get => _stateNumber;
            set
            {
                _stateNumber = value;
                OnPropertyChanged(nameof(StateNumber));
            }
        }

        private string _status;
        public string Status
        {
            get => _status;
            set
            {
                _status = value;
                OnPropertyChanged(nameof(Status));
            }
        }
    }
}