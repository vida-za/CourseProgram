using CourseProgram.Exceptions;
using CourseProgram.Models;
using CourseProgram.Services;
using CourseProgram.Services.DataServices;
using CourseProgram.Stores;
using CourseProgram.ViewModels;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace CourseProgram.Commands
{
    public class AddMachineCommand : CommandBaseAsync
    {
        private readonly AddMachineViewModel _addMachineViewModel;
        private readonly MachineDataService _machineDataService;
        private readonly INavigationService _machineViewNavigationService;

        public AddMachineCommand(AddMachineViewModel addMachineViewModel, ServicesStore servicesStore, INavigationService machineViewNavigationService)
        {
            _addMachineViewModel = addMachineViewModel;
            _machineDataService = servicesStore._machineService;
            _machineViewNavigationService = machineViewNavigationService;

            _addMachineViewModel.PropertyChanged += OnViewModelPropertyChanged;
        }

        private void OnViewModelPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e) 
        { 
            if (e.PropertyName == nameof(_addMachineViewModel.TypeMachine) ||
                e.PropertyName == nameof(_addMachineViewModel.TypeLoading) ||
                e.PropertyName == nameof(_addMachineViewModel.LoadCapacity) ||
                e.PropertyName == nameof(_addMachineViewModel.Stamp) ||
                e.PropertyName == nameof(_addMachineViewModel.Name) ||
                e.PropertyName == nameof(_addMachineViewModel.Volume) ||
                e.PropertyName == nameof(_addMachineViewModel.LengthBodywork) ||
                e.PropertyName == nameof(_addMachineViewModel.WidthBodywork) ||
                e.PropertyName == nameof(_addMachineViewModel.HeightBodywork))
            {
                OnCanExecuteChanged();
            }
        }

        public override bool CanExecute(object? parameter)
        {
            return !string.IsNullOrEmpty(_addMachineViewModel.TypeMachine) &&
                   !string.IsNullOrEmpty(_addMachineViewModel.TypeLoading) &&
                   !string.IsNullOrEmpty(_addMachineViewModel.LoadCapacity) &&
                   !string.IsNullOrEmpty(_addMachineViewModel.Stamp) &&
                   !string.IsNullOrEmpty(_addMachineViewModel.Name) &&
                   float.TryParse(_addMachineViewModel.LoadCapacity, out float temp) &&
                   (float.TryParse(_addMachineViewModel.Volume, out temp) || string.IsNullOrEmpty(_addMachineViewModel.Volume)) &&
                   (float.TryParse(_addMachineViewModel.LengthBodywork, out temp) || string.IsNullOrEmpty(_addMachineViewModel.LengthBodywork)) &&
                   (float.TryParse(_addMachineViewModel.WidthBodywork, out temp) || string.IsNullOrEmpty(_addMachineViewModel.WidthBodywork)) &&
                   (float.TryParse(_addMachineViewModel.HeightBodywork, out temp) || string.IsNullOrEmpty(_addMachineViewModel.HeightBodywork)) &&
                   base.CanExecute(parameter);
        }

        public override async Task ExecuteAsync(object? parameter)
        {
            int newID = await _machineDataService.FindMaxEmptyID();
            Machine machine = new(
                newID,
                _addMachineViewModel.TypeMachine,
                _addMachineViewModel.TypeBodywork,
                _addMachineViewModel.TypeLoading,
                float.Parse(_addMachineViewModel.LoadCapacity),
                float.Parse(_addMachineViewModel.Volume),
                _addMachineViewModel.HydroBoard,
                float.Parse(_addMachineViewModel.LengthBodywork),
                float.Parse(_addMachineViewModel.WidthBodywork),
                float.Parse(_addMachineViewModel.HeightBodywork),
                _addMachineViewModel.Stamp,
                _addMachineViewModel.Name,
                _addMachineViewModel.StateNumber,
                string.Empty,
                DateTime.Now,
                DateTime.MinValue,
                string.Empty
                );

            try
            {
                await _machineDataService.AddItemAsync(machine);

                MessageBox.Show("Машина добавлена", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);

                _machineViewNavigationService.Navigate();
            }
            catch (RepeatConflictException<Machine>) 
            {
                MessageBox.Show("Такая машина уже имеется", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}