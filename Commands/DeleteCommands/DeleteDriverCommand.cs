using CourseProgram.Services.DataServices;
using CourseProgram.ViewModels.ListingViewModel;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;

namespace CourseProgram.Commands.DeleteCommands
{
    public class DeleteDriverCommand : CommandBaseAsync
    {
        private readonly DriverListingViewModel _listingViewModel;
        private readonly DriverDataService _dataService;

        public DeleteDriverCommand(DriverListingViewModel listingViewModel, DriverDataService dataService)
        {
            _listingViewModel = listingViewModel;
            _dataService = dataService;

            _listingViewModel.PropertyChanged += OnViewModelPropertyChanged;
        }

        private void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_listingViewModel.SelectedItem))
            {
                OnCanExecuteChanged();
            }
        }

        public override bool CanExecute(object? parameter)
        {
            return base.CanExecute(parameter) && _listingViewModel.SelectedItem is not null && _listingViewModel.SelectedItem.DateEnd == DateOnly.MinValue.ToString();
        }

        public override async Task ExecuteAsync(object? parameter)
        {
            try
            {
                await _dataService.DeleteItemAsync(_listingViewModel.SelectedItem.GetModel().ID);

                _listingViewModel.UpdateData();
            }
            catch (Exception ex) { Debug.Write(ex.Message); }
        }
    }
}