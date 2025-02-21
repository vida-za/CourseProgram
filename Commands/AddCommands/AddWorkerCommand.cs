using CourseProgram.Models;
using CourseProgram.Services;
using CourseProgram.Stores;
using CourseProgram.ViewModels.AddViewModel;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CourseProgram.Commands.AddCommands
{
    public class AddWorkerCommand : BaseAddCommand
    {
        private readonly AddWorkerViewModel _viewModel;

        public AddWorkerCommand(AddWorkerViewModel viewModel, ServicesStore servicesStore, INavigationService navigationService)
        {
            _viewModel = viewModel;
            _servicesStore = servicesStore;
            _navigationService = navigationService;

            _viewModel.PropertyChanged += OnViewModelPropertyChanged;
        }

        protected override void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
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
            Worker worker = new(
                -1,
                _viewModel.Name,
                _viewModel.BirthDay,
                _viewModel.Passport,
                _viewModel.Phone,
                DateOnly.FromDateTime(DateTime.Now),
                null
                );

            try
            {
                await _servicesStore.GetService<Worker>().FindMaxEmptyID();
                int result = await _servicesStore.GetService<Worker>().AddItemAsync(worker);
                if (result > 0)
                    MessageBox.Show("Сотрудник добавлен", "Успешно", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else
                    MessageBox.Show("Не удалось добавить сотрудника", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);

                _navigationService.Navigate();
            }
            catch(Exception) { }
        }
    }
}