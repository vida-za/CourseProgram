using CourseProgram.Models;
using CourseProgram.Services;
using CourseProgram.Stores;
using CourseProgram.ViewModels.ListingViewModel;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;

namespace CourseProgram.Commands.DeleteCommands
{
    public class DeleteDriverCommand : BaseDeleteCommand
    {
        private readonly DriverListingViewModel _viewModel;

        public DeleteDriverCommand(DriverListingViewModel listingViewModel, ServicesStore servicesStore)
        {
            _viewModel = listingViewModel;
            _servicesStore = servicesStore;

            _viewModel.PropertyChanged += OnViewModelPropertyChanged;
        }

        private void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_viewModel.SelectedItem))
            {
                OnCanExecuteChanged();
            }
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
                Driver temp = _viewModel.SelectedItem.GetModel();
                var newItem = new Driver(
                    temp.ID,
                    temp.FIO,
                    temp.BirthDay,
                    temp.Passport,
                    temp.Phone,
                    temp.DateStart,
                    DateOnly.FromDateTime(DateTime.Now)
                    );

                bool result = await _servicesStore._driverService.UpdateItemAsync(newItem);
                if (result)
                    MessageBox.Show("Водитель удален", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                else
                    MessageBox.Show("Не удалось удалить водителя", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);

                await _viewModel.UpdateDataAsync();
            }
            catch (Exception ex) 
            {
                await LogManager.Instance.WriteLogAsync($"ERROR {ex.Message}");
            }
        }
    }
}