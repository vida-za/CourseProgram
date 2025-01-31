using CourseProgram.Exceptions;
using CourseProgram.Models;
using CourseProgram.Services;
using CourseProgram.Stores;
using CourseProgram.ViewModels.AddViewModel;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace CourseProgram.Commands.AddCommands
{
    public class AddMachineCommand : BaseAddCommand
    {
        private readonly AddMachineViewModel _viewModel;

        public AddMachineCommand(AddMachineViewModel addMachineViewModel, ServicesStore servicesStore, INavigationService machineViewNavigationService)
        {
            _viewModel = addMachineViewModel;
            _servicesStore = servicesStore;
            _navigationService = machineViewNavigationService;

            _viewModel.PropertyChanged += OnViewModelPropertyChanged;
        }

        protected override void OnViewModelPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_viewModel.TypeMachine) ||
                e.PropertyName == nameof(_viewModel.TypeLoading) ||
                e.PropertyName == nameof(_viewModel.LoadCapacity) ||
                e.PropertyName == nameof(_viewModel.Stamp) ||
                e.PropertyName == nameof(_viewModel.Name) ||
                e.PropertyName == nameof(_viewModel.Volume) ||
                e.PropertyName == nameof(_viewModel.LengthBodywork) ||
                e.PropertyName == nameof(_viewModel.WidthBodywork) ||
                e.PropertyName == nameof(_viewModel.HeightBodywork))
            {
                OnCanExecuteChanged();
            }
        }

        public override bool CanExecute(object? parameter)
        {
            return !string.IsNullOrEmpty(_viewModel.TypeMachine) &&
                   !string.IsNullOrEmpty(_viewModel.TypeLoading) &&
                   !string.IsNullOrEmpty(_viewModel.LoadCapacity) &&
                   !string.IsNullOrEmpty(_viewModel.Stamp) &&
                   !string.IsNullOrEmpty(_viewModel.Name) &&
                   float.TryParse(_viewModel.LoadCapacity, out float temp) &&
                   (float.TryParse(_viewModel.Volume, out temp) || string.IsNullOrEmpty(_viewModel.Volume)) &&
                   (float.TryParse(_viewModel.LengthBodywork, out temp) || string.IsNullOrEmpty(_viewModel.LengthBodywork)) &&
                   (float.TryParse(_viewModel.WidthBodywork, out temp) || string.IsNullOrEmpty(_viewModel.WidthBodywork)) &&
                   (float.TryParse(_viewModel.HeightBodywork, out temp) || string.IsNullOrEmpty(_viewModel.HeightBodywork)) &&
                   base.CanExecute(parameter);
        }

        public override async Task ExecuteAsync(object? parameter)
        {
            Machine machine = new(
                -1,
                _viewModel.TypeMachine,
                _viewModel.TypeBodywork == null ? Constants.GetEnumDescription(Constants.MachineTypeBodyworkValues.Null) : _viewModel.TypeBodywork,
                _viewModel.TypeLoading,
                float.Parse(_viewModel.LoadCapacity),
                _viewModel.Volume == null ? null : float.Parse(_viewModel.Volume),
                _viewModel.HydroBoard,
                _viewModel.LengthBodywork == null ? null : float.Parse(_viewModel.LengthBodywork),
                _viewModel.WidthBodywork == null ? null : float.Parse(_viewModel.WidthBodywork),
                _viewModel.HeightBodywork == null ? null : float.Parse(_viewModel.HeightBodywork),
                _viewModel.Stamp,
                _viewModel.Name,
                _viewModel.StateNumber,
                Constants.GetEnumDescription(Constants.MachineStatusValues.Waiting),
                DateTime.Now,
                null,
                null
                );

            try
            {
                await _servicesStore._machineService.FindMaxEmptyID();
                int result = await _servicesStore._machineService.AddItemAsync(machine);
                if (result > 0)
                    MessageBox.Show("Машина добавлена", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                else
                    MessageBox.Show("Не удалось добавить машину", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);

                _navigationService.Navigate();
            }
            catch (RepeatConflictException<Machine>)
            {
                MessageBox.Show("Такая машина уже имеется", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}