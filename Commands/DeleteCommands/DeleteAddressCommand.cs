using CourseProgram.Services.DataServices;
using CourseProgram.ViewModels.ListingViewModel;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;

namespace CourseProgram.Commands.DeleteCommands
{
    public class DeleteAddressCommand : BaseDeleteCommand
    {
        private readonly AddressListingViewModel _listingViewModel;
        private readonly AddressDataService _dataService;

        public DeleteAddressCommand(AddressListingViewModel listingViewModel, AddressDataService dataService) 
        {
            _listingViewModel = listingViewModel;
            _dataService = dataService;

            _listingViewModel.PropertyChanged += OnViewModelPropertyChanged;
        }

        private void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_listingViewModel.SelectedItem))
                OnCanExecuteChanged();
        }

        public override bool CanExecute(object? parameter)
        {
            return base.CanExecute(parameter) && _listingViewModel.SelectedItem is not null;
        }

        protected override async Task ExecuteDeleteAsync(object? parameter)
        {
            try
            {
                await _dataService.DeleteItemAsync(_listingViewModel.SelectedItem.GetModel().ID);

                _listingViewModel.Items.Remove(_listingViewModel.SelectedItem);
                //_listingViewModel.UpdateData();
            }
            catch(Exception ex) { Debug.WriteLine(ex.Message); }
        }
    }
}