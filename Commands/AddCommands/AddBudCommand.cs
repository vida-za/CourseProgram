using CourseProgram.Models;
using CourseProgram.Services;
using CourseProgram.Stores;
using CourseProgram.ViewModels.AddViewModel;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using static CourseProgram.Models.Constants;

namespace CourseProgram.Commands.AddCommands
{
    public class AddBudCommand : BaseAddCommand
    {
        private readonly AddBudViewModel _viewModel;

        public AddBudCommand(AddBudViewModel viewModel, ServicesStore servicesStore, INavigationService operationalViewNavigationService)
        {
            _viewModel = viewModel;
            _servicesStore = servicesStore;
            _navigationService = operationalViewNavigationService;

            _viewModel.PropertyChanged += OnViewModelPropertyChanged;
        }

        protected override void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_viewModel.SelectedClient) ||
                e.PropertyName == nameof(_viewModel.SelectedWorker) ||
                e.PropertyName == nameof(_viewModel.Cargos))
            {
                OnCanExecuteChanged();
            }
        }

        public override bool CanExecute(object? parameter)
        {
            return _viewModel.SelectedClient != null &&
                   _viewModel.SelectedWorker != null &&
                   _viewModel.Cargos.Count > 0 &&
                   base.CanExecute(parameter);
        }

        public override async Task ExecuteAsync(object? parameter)
        {
            var bud = new Bud(
                -1,
                _viewModel.SelectedClient.ID,
                _viewModel.SelectedWorker.ID,
                DateTime.Now,
                BudStatusValues.Waiting,
                _viewModel.Description); 

            try
            {
                await _servicesStore._budService.FindMaxEmptyID();
                int resultBud = await _servicesStore._budService.AddItemAsync(bud);
            }
            catch (Exception ex)
            {
                await LogManager.Instance.WriteLogAsync($"ERROR in {nameof(AddBudCommand)}: {ex.Message}");
                MessageBox.Show("Неизвестная ошибка!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}