using CourseProgram.Models;
using CourseProgram.Services;
using CourseProgram.Services.DataServices;
using CourseProgram.Stores;
using CourseProgram.ViewModels.AddViewModel;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CourseProgram.Commands.AddCommands
{
    public class AddWorkerCommand : CommandBaseAsync
    {
        private readonly AddWorkerViewModel _viewModel;
        private readonly WorkerDataService _dataService;
        private readonly INavigationService _navigationService;

        public AddWorkerCommand(AddWorkerViewModel viewModel, ServicesStore servicesStore, INavigationService navigationService)
        {
            _viewModel = viewModel;
            _dataService = servicesStore._workerService;
            _navigationService = navigationService;

            _viewModel.PropertyChanged += OnViewModelPropertyChanged;
        }

        private void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_viewModel.Name) ||
                e.PropertyName == nameof(_viewModel.Passport))
                OnCanExecuteChanged();
        }

        public override bool CanExecute(object? parameter)
        {
            return !string.IsNullOrEmpty(_viewModel.Name) && 
                   !string.IsNullOrEmpty(_viewModel.Passport) && 
                   base.CanExecute(parameter);
        }

        public override async Task ExecuteAsync(object? parameter)
        {
            int newID = await _dataService.FindMaxEmptyID();
            Worker worker = new(
                newID,
                _viewModel.Name,
                _viewModel.BirthDay,
                _viewModel.Passport,
                _viewModel.Phone == null ? string.Empty : _viewModel.Phone,
                DateOnly.FromDateTime(DateTime.Now),
                DateOnly.MinValue
                );

            try
            {
                await _dataService.AddItemAsync(worker);

                MessageBox.Show("Сотрудник добавлен", "Успешно", MessageBoxButtons.OK, MessageBoxIcon.Information);

                _navigationService.Navigate();
            }
            catch(Exception ex) { }
        }
    }
}