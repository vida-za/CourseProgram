using CourseProgram.Services.DataServices;
using CourseProgram.ViewModels;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;

namespace CourseProgram.Commands
{
    public class DeleteDriverCommand : CommandBaseAsync
    {
        private readonly DriverListingViewModel _driverListingViewModel;
        private readonly DriverDataService _driverDataService;

        public DeleteDriverCommand(DriverListingViewModel driverListingViewModel, DriverDataService driverDataService)
        {
            _driverListingViewModel = driverListingViewModel;
            _driverDataService = driverDataService;

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

        public override async Task ExecuteAsync(object? parameter)
        {
            try
            {
                await _driverDataService.DeleteItemAsync(_driverListingViewModel.SelectedDriver.GetDriver().ID);

                _driverListingViewModel.UpdateDrivers();
            }
            catch (Exception ex) { Debug.Write(ex.Message); }
        }
    }
}