﻿using CourseProgram.Exceptions;
using CourseProgram.Models;
using CourseProgram.Services;
using CourseProgram.Services.DataServices;
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
        private readonly NavigationService _driverViewNavigationService;

        public AddDriverCommand(AddDriverViewModel addDriverViewModel, DriverDataService driverDataService, NavigationService driverViewNavigationService)
        {
            _addDriverViewModel = addDriverViewModel;
            _driverDataService = driverDataService;
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
            Driver driver = new(
                _driverDataService.FindMaxID(),
                _addDriverViewModel.DriverName,
                _addDriverViewModel.BirthDay,
                _addDriverViewModel.Passport,
                _addDriverViewModel.Phone,
                DateTime.Now,
                DateTime.MaxValue
                );

            try
            {
                await _driverDataService.AddItemAsync(driver);

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