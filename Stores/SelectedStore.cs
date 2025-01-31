using CourseProgram.ViewModels.EntityViewModel;
using System;

namespace CourseProgram.Stores
{
    public class SelectedStore
    {
        private AddressViewModel _currentAddress;
        public AddressViewModel CurrentAddress
        {
            get => _currentAddress;
            set
            {
                _currentAddress = value;
                OnCurrentModelChanged();
            }
        }

        private BudViewModel _currentBud;
        public BudViewModel CurrentBud
        {
            get => _currentBud;
            set
            {
                _currentBud = value;
                OnCurrentModelChanged();
            }
        }

        private CargoViewModel _currentCargo;
        public CargoViewModel CurrentCargo
        {
            get => _currentCargo;
            set
            {
                _currentCargo = value;
                OnCurrentModelChanged();
            }
        }

        private ClientViewModel _currentClient;
        public ClientViewModel CurrentClient
        {
            get => _currentClient;
            set
            {
                _currentClient = value;
                OnCurrentModelChanged();
            }
        }

        private DriverViewModel _currentDriver;
        public DriverViewModel CurrentDriver
        {
            get => _currentDriver;
            set
            {
                _currentDriver = value;
                OnCurrentModelChanged();
            }
        }

        private HaulViewModel _currentHaul;
        public HaulViewModel CurrentHaul
        {
            get => _currentHaul;
            set
            {
                _currentHaul = value;
                OnCurrentModelChanged();
            }
        }

        private MachineViewModel _currentMachine;
        public MachineViewModel CurrentMachine
        {
            get => _currentMachine;
            set 
            {
                _currentMachine = value;
                OnCurrentModelChanged();
            }
        }

        private NomenclatureViewModel _currentNomenclature;
        public NomenclatureViewModel CurrentNomenclature
        {
            get => _currentNomenclature;
            set
            {
                _currentNomenclature = value;
                OnCurrentModelChanged();
            }
        }

        private OrderViewModel _currentOrder;
        public OrderViewModel CurrentOrder
        {
            get => _currentOrder;
            set
            {
                _currentOrder = value;
                OnCurrentModelChanged();
            }
        }

        private RouteViewModel _currentRoute;
        public RouteViewModel CurrentRoute
        {
            get => _currentRoute;
            set
            {
                _currentRoute = value;
                OnCurrentModelChanged();
            }
        }

        private WorkerViewModel _currentWorker;
        public WorkerViewModel CurrentWorker
        {
            get => _currentWorker;
            set
            {
                _currentWorker = value;
                OnCurrentModelChanged();
            }
        }

        private void OnCurrentModelChanged()
        {
            CurrentModelChanged?.Invoke();
        }

        public event Action CurrentModelChanged;
    }
}