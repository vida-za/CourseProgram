using CourseProgram.Services.DataServices;
using CourseProgram.ViewModels.ListingViewModel;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace CourseProgram.Commands.DeleteCommands
{
    public class DeleteWorkerCommand : CommandBaseAsync
    {
        private readonly WorkerListingViewModel _listingViewModel;
        private readonly WorkerDataService _dataService;

        public DeleteWorkerCommand(WorkerListingViewModel viewModel, WorkerDataService dataService)
        {
            _listingViewModel = viewModel;
            _dataService = dataService;

            _listingViewModel.PropertyChanged += OnViewModelPropertyChanged;
        }

        private void OnViewModelPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_listingViewModel.SelectedItem))
                OnCanExecuteChanged();
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
            catch (Exception ex) { Debug.WriteLine(ex); }
        }
    }
}