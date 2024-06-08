using CourseProgram.Services.DataServices;
using CourseProgram.ViewModels.ListingViewModel;
using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace CourseProgram.Commands.DeleteCommands
{
    public class DeleteClientCommand : CommandBaseAsync
    {
        private readonly ClientListingViewModel _viewModel;
        private readonly ClientDataService _dataService;

        public DeleteClientCommand(ClientListingViewModel viewModel, ClientDataService dataService)
        {
            _viewModel = viewModel;
            _dataService = dataService;

            _viewModel.PropertyChanged += OnViewModelPropertyChanged;
        }

        private void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
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
            catch(Exception ex) { }
        }
    }
}