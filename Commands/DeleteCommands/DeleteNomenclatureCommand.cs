using CourseProgram.Services;
using CourseProgram.Stores;
using CourseProgram.ViewModels.ListingViewModel;
using System;
using System.Threading.Tasks;

namespace CourseProgram.Commands.DeleteCommands
{
    public class DeleteNomenclatureCommand : BaseDeleteCommand
    {
        private readonly NomenclatureListingViewModel _viewModel;

        public DeleteNomenclatureCommand(NomenclatureListingViewModel viewModel, ServicesStore servicesStore)
        {
            _viewModel = viewModel;
            _servicesStore = servicesStore;

            _viewModel.PropertyChanged += OnViewModelPropertyChanged;
        }

        private void OnViewModelPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_viewModel.SelectedItem))
                OnCanExecuteChanged();
        }

        protected override bool IsItemSelected()
        {
            return _viewModel.SelectedItem != null;
        }

        protected override async Task ExecuteDeleteAsync(object? parameter)
        {
            try
            {
                // TO DO
            }
            catch (Exception ex)
            {
                await LogManager.Instance.WriteLogAsync($"ERROR {ex.Message}");
            }
        }
    }
}