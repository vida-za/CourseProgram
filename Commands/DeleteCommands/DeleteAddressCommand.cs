using CourseProgram.Models;
using CourseProgram.Services;
using CourseProgram.Stores;
using CourseProgram.ViewModels.ListingViewModel;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;

namespace CourseProgram.Commands.DeleteCommands
{
    public class DeleteAddressCommand : BaseDeleteCommand
    {
        private readonly AddressListingViewModel _viewModel;

        public DeleteAddressCommand(AddressListingViewModel listingViewModel, ServicesStore servicesStore) 
        {
            _viewModel = listingViewModel;
            _servicesStore = servicesStore;

            _viewModel.PropertyChanged += OnViewModelPropertyChanged;
        }

        private void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_viewModel.SelectedItem))
                OnCanExecuteChanged();
        }

        public override bool CanExecute(object? parameter)
        {
            return base.CanExecute(parameter) && _viewModel.SelectedItem.GetModel().Active;
        }

        protected override bool IsItemSelected()
        {
            return _viewModel.SelectedItem is not null;
        }

        protected override async Task ExecuteDeleteAsync(object? parameter)
        {
            try
            {
                Address temp = _viewModel.SelectedItem.GetModel();
                var newItem = new Address(
                    temp.ID,
                    temp.City,
                    temp.Street,
                    temp.House,
                    temp.Structure,
                    temp.Frame,
                    false
                    );

                bool result = await _servicesStore._addressService.UpdateItemAsync(newItem);
                if (result)
                    MessageBox.Show("Адрес удален", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                else
                    MessageBox.Show("Не удалось удалить адрес", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);

                await _viewModel.UpdateDataAsync();
            }
            catch(Exception ex) 
            {
                await LogManager.Instance.WriteLogAsync($"ERROR {ex.Message}");
            }
        }
    }
}