using CourseProgram.Models;
using CourseProgram.Services;
using CourseProgram.Stores;
using CourseProgram.ViewModels.ListingViewModel;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace CourseProgram.Commands.DeleteCommands
{
    public class DeleteWorkerCommand : BaseDeleteCommand
    {
        private readonly WorkerListingViewModel _viewModel;

        public DeleteWorkerCommand(WorkerListingViewModel viewModel, ServicesStore servicesStore)
        {
            _viewModel = viewModel;
            _servicesStore = servicesStore;

            _viewModel.PropertyChanged += OnViewModelPropertyChanged;
        }

        private void OnViewModelPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_viewModel.SelectedItem))
                OnCanExecuteChanged();
        }

        public override bool CanExecute(object? parameter)
        {
            return base.CanExecute(parameter) && _viewModel.SelectedItem.DateEnd == DateOnly.MinValue.ToString();
        }

        protected override bool IsItemSelected()
        {
            return _viewModel.SelectedItem != null;
        }

        protected override async Task ExecuteDeleteAsync(object? parameter) 
        {
            try
            {
                Worker temp = _viewModel.SelectedItem.GetModel();
                var newItem = new Worker(
                    temp.ID,
                    temp.FIO,
                    temp.BirthDay,
                    temp.Passport,
                    temp.Phone,
                    temp.DateStart,
                    DateOnly.FromDateTime(DateTime.Now)
                    );

                bool result = await _servicesStore.GetService<Worker>().UpdateItemAsync(newItem);
                if (result)
                    MessageBox.Show("Сотрудник удален!", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                else
                    MessageBox.Show("Не удалось удалить сотрудника", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);

                await _viewModel.UpdateDataAsync();
            }
            catch (Exception ex) 
            {
                await LogManager.Instance.WriteLogAsync($"ERROR {ex.Message}");
            }
        }
    }
}