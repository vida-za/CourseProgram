using CourseProgram.DataClasses;
using CourseProgram.Models;
using CourseProgram.ViewModels;
using System;
using System.ComponentModel;
using System.Diagnostics;

namespace CourseProgram.Commands
{
    public class DeleteDriverCommand : CommandBase
    {
        private readonly DriverListingViewModel _driverListingViewModel;
        private readonly DriverData _driverData;

        public DeleteDriverCommand(DriverListingViewModel driverListingViewModel, DriverData driverData)
        {
            _driverListingViewModel = driverListingViewModel;
            _driverData = driverData;

            _driverListingViewModel.PropertyChanged += OnViewModelPropertyChanged;
        }

        private void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_driverListingViewModel.SelectedDriver))
            {
                OnCanExecuteChanged();
            }
        }

        public override bool CanExecute(object? parameter)
        {
            return base.CanExecute(parameter) && _driverListingViewModel.SelectedDriver is not null;
        }

        public override void Execute(object? parameter)
        {
            try
            {
                _driverData.RemoveDriver(_driverListingViewModel.SelectedDriver.GetDriver());

                _driverListingViewModel.UpdateDrivers();
            }
            catch (Exception ex) { Debug.Write(ex.Message); }
        }
    }
}