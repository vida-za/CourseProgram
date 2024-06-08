using CourseProgram.Commands;
using CourseProgram.Commands.AddCommands;
using CourseProgram.Services;
using CourseProgram.Stores;
using System.Collections.ObjectModel;
using static CourseProgram.Models.Constants;

namespace CourseProgram.ViewModels.AddViewModel
{
    public class AddMachineViewModel : BaseAddViewModel
    {
        public AddMachineViewModel(ServicesStore servicesStore, INavigationService machineViewNavigationService)
        {
            SubmitCommand = new AddMachineCommand(this, servicesStore, machineViewNavigationService);
            CancelCommand = new NavigateCommand(machineViewNavigationService);
        }

        public ObservableCollection<string> TypeMachineArray => GetFullEnumDescription(typeof(TypeMachineValues));
        public ObservableCollection<string> TypeBodyworkArray => GetFullEnumDescription(typeof(TypeBodyworkValues));
        public ObservableCollection<string> TypeLoadingArray => GetFullEnumDescription(typeof(TypeLoadingValues));

        #region don`t must have
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
        #endregion

        #region must have
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
        #endregion
    }
}