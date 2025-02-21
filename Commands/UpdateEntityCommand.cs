using CourseProgram.Models;
using CourseProgram.Services;
using CourseProgram.Services.DataServices;
using CourseProgram.ViewModels.DetailViewModel;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;

namespace CourseProgram.Commands
{
    public class UpdateEntityCommand<T> : CommandBaseAsync where T : IModel
    {
        private readonly BaseDetailViewModel _viewModel;
        private readonly IDataService<T> _dataService;
        private readonly Func<T> _getEntity;
        private readonly INavigationService _navigationService;
        private readonly Func<bool>? _canExecute;
        private readonly Action? _onSuccess;

        public UpdateEntityCommand(BaseDetailViewModel viewModel, IDataService<T> dataService, Func<T> getEntity, INavigationService navigationService, Func<bool>? canExecute = null, Action? onSuccess = null)
        { 
            _viewModel = viewModel;
            _dataService = dataService;
            _getEntity = getEntity;
            _navigationService = navigationService;
            _canExecute = canExecute;
            _onSuccess = onSuccess;

            _viewModel.PropertyChanged += _viewModel_PropertyChanged;
        }

        private void _viewModel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_viewModel.IsDirty))
            {
                OnCanExecuteChanged();
            }
        }

        public override bool CanExecute(object? parameter) => _canExecute?.Invoke() ?? true;

        public override async Task ExecuteAsync(object? parameter)
        {
            var entity = _getEntity();
            if (entity == null)
                return;

            try
            {
                bool result = await _dataService.UpdateItemAsync(entity);
                if (result)
                {
                    _onSuccess?.Invoke();
                    MessageBox.Show("Данные обновлены", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                    MessageBox.Show("Не удалось выполнить обновление данных в БД", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                await LogManager.Instance.WriteLogAsync($"Error in Update with {ex.Message}");
                MessageBox.Show("Неизвестная ошибка", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            _navigationService.Navigate();
        }
    }
}