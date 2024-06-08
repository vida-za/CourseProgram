using CourseProgram.Services.DataServices;
using CourseProgram.ViewModels.ListingViewModel;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace CourseProgram.Commands.DeleteCommands
{
    public class DeleteWorkerCommand : CommandBaseAsync
    {
        private readonly WorkerListingViewModel _viewModel;
        private readonly WorkerDataService _dataService;

        public DeleteWorkerCommand(WorkerListingViewModel viewModel, WorkerDataService dataService)
        {
            _viewModel = viewModel;
            _dataService = dataService;

            _viewModel.PropertyChanged += OnViewModelPropertyChanged;
        }

        private void OnViewModelPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_viewModel.SelectedItem))
                OnCanExecuteChanged();
        }

        public override bool CanExecute(object? parameter)
        {
            return base.CanExecute(parameter) && _viewModel.SelectedItem is not null;
        }

        public override async Task ExecuteAsync(object? parameter) 
        {
            try
            {
                await _dataService.DeleteItemAsync(_viewModel.SelectedItem.GetModel().ID);

                _viewModel.UpdateData();
            }
            catch (Exception ex) { Debug.WriteLine(ex); }
        }
    }
}