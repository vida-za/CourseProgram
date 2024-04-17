using CourseProgram.Models;
using CourseProgram.ViewModels;
using System;

namespace CourseProgram.Stores
{
    public class SelectedStore
    {
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

        private void OnCurrentModelChanged()
        {
            CurrentModelChanged?.Invoke();
        }

        public event Action CurrentModelChanged;
    }
}