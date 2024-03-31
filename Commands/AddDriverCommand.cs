using CourseProgram.Exceptions;
using CourseProgram.Models;
using CourseProgram.Services;
using CourseProgram.Services.DataServices;
using CourseProgram.Stores;
using CourseProgram.ViewModels;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;

namespace CourseProgram.Commands
{
    public class AddDriverCommand : CommandBaseAsync
    {
        private readonly AddDriverViewModel _addDriverViewModel;
        private readonly DriverDataService _driverDataService;
        private readonly DriverCategoriesDataService _driverCategoriesDataService;
        private readonly INavigationService _driverViewNavigationService;

        public AddDriverCommand(AddDriverViewModel addDriverViewModel, ServicesStore servicesStore, INavigationService driverViewNavigationService)
        {
            _addDriverViewModel = addDriverViewModel;
            _driverDataService = servicesStore._driverService;
            _driverCategoriesDataService = servicesStore._driverCategoriesService;
            _driverViewNavigationService = driverViewNavigationService;

            _addDriverViewModel.PropertyChanged += OnViewModelPropertyChanged;


        }

        private void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_addDriverViewModel.DriverName) || 
                e.PropertyName == nameof(_addDriverViewModel.Passport) ||
                e.PropertyName == nameof(_addDriverViewModel.Phone))
            {
                OnCanExecuteChanged();
            }
        }

        public override bool CanExecute(object? parameter)
        {
            return !string.IsNullOrEmpty(_addDriverViewModel.DriverName) && 
                   !string.IsNullOrEmpty(_addDriverViewModel.Passport) &&
                   !string.IsNullOrEmpty(_addDriverViewModel.Phone) && 
                   base.CanExecute(parameter);
        }

        public override async Task ExecuteAsync(object? parameter)
        {
            int newID = await _driverDataService.FindMaxEmptyID();
            Driver driver = new(
                newID,
                _addDriverViewModel.DriverName,
                _addDriverViewModel.BirthDay,
                _addDriverViewModel.Passport,
                _addDriverViewModel.Phone,
                DateTime.Now,
                DateTime.MaxValue,
                string.Empty
                );

            try
            {
                await _driverDataService.AddItemAsync(driver);

                foreach(Category ctg in _addDriverViewModel.Categories)
                {
                    if (ctg.IsChecked)
                        await _driverCategoriesDataService.AddItemAsync(new DriverCategories(driver.ID, ctg.ID));
                }

                MessageBox.Show("Successfully add driver", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                _driverViewNavigationService.Navigate();
            }
            catch (RepeatConflictException<Driver>)
            {
                MessageBox.Show("This driver is repeat", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}