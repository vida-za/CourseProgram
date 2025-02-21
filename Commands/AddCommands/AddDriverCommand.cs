using CourseProgram.Exceptions;
using CourseProgram.Models;
using CourseProgram.Services;
using CourseProgram.Stores;
using CourseProgram.ViewModels.AddViewModel;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;

namespace CourseProgram.Commands.AddCommands
{
    public class AddDriverCommand : BaseAddCommand
    {
        private readonly AddDriverViewModel _viewModel;

        public AddDriverCommand(AddDriverViewModel addDriverViewModel, ServicesStore servicesStore, INavigationService driverViewNavigationService)
        {
            _viewModel = addDriverViewModel;
            _servicesStore = servicesStore;
            _navigationService = driverViewNavigationService;

            _viewModel.PropertyChanged += OnViewModelPropertyChanged;
        }

        protected override void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_viewModel.DriverName) ||
                e.PropertyName == nameof(_viewModel.Passport) ||
                e.PropertyName == nameof(_viewModel.Phone))
            {
                OnCanExecuteChanged();
            }
        }

        public override bool CanExecute(object? parameter)
        {
            return !string.IsNullOrEmpty(_viewModel.DriverName) &&
                   !string.IsNullOrEmpty(_viewModel.Passport) &&
                   _viewModel.Passport.Length == 11 &&
                   base.CanExecute(parameter);
        }

        public override async Task ExecuteAsync(object? parameter)
        {
            var driver = new Driver(
                -1,
                _viewModel.DriverName,
                _viewModel.BirthDay,
                _viewModel.Passport,
                _viewModel.Phone,
                DateOnly.FromDateTime(DateTime.Now),
                null
                );

            try
            {
                await _servicesStore.GetService<Driver>().FindMaxEmptyID();
                int resultDrv = await _servicesStore.GetService<Driver>().AddItemAsync(driver);
                if (resultDrv > 0)
                {
                    bool resultCat = true;
                    foreach (var cat in _viewModel.Categories)
                    {
                        if (cat.IsChecked)
                        {
                            int temp = await _servicesStore.GetService<DriverCategories>().AddItemAsync(new DriverCategories(resultDrv, (int)cat.EnumCategory));
                            if (temp == 0)
                                resultCat = false;
                        }
                    }
                    if (resultCat)
                        MessageBox.Show("Водитель добавлен", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                    else
                        MessageBox.Show("Водитель добавлен, но возникла проблема с категориями", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                    MessageBox.Show("Не удалось добавить водителя", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);

                _navigationService.Navigate();
            }
            catch (RepeatConflictException<Driver>)
            {
                MessageBox.Show("Такой водитель уже имеется", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                await LogManager.Instance.WriteLogAsync($"ERROR {ex.Message}");
                MessageBox.Show("Неизвестная ошибка", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}