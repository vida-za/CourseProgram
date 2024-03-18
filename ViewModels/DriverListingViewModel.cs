﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using CourseProgram.Commands;
using CourseProgram.Models;
using CourseProgram.Services;
using CourseProgram.DataClasses;

namespace CourseProgram.ViewModels
{
    public class DriverListingViewModel : BaseViewModel
    {
        public DriverListingViewModel(DriverData driverData, NavigationService addDriverNavigationService)
        {
            _driverData = driverData;
            _drivers = new ObservableCollection<DriverViewModel>();

            AddDriverCommand = new NavigateCommand(addDriverNavigationService);
            DeleteDriverCommand = new DeleteDriverCommand(this, driverData);

            UpdateDrivers();
        }

        public void UpdateDrivers()
        {
            _drivers.Clear();

            foreach (Driver driver in _driverData.GetDriversAll())
            {
                DriverViewModel driverViewModel = new DriverViewModel(driver);
                _drivers.Add(driverViewModel);
            }
        }

        private readonly ObservableCollection<DriverViewModel> _drivers;
        private readonly DriverData _driverData;

        public IEnumerable<DriverViewModel> Drivers => _drivers;

        public ICommand AddDriverCommand { get; }
        public ICommand DeleteDriverCommand { get; }

        private DriverViewModel _selectedDriver;
        public DriverViewModel SelectedDriver
        {
            get => _selectedDriver;
            set
            {
                _selectedDriver = value;
                OnPropertyChanged(nameof(SelectedDriver));
            }
        }

    }
}