using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using CourseProgram.Models;
using CourseProgram.Services;
using CourseProgram.Services.DataServices;
using CourseProgram.Stores;
using CourseProgram.ViewModels.AddViewModel;

namespace CourseProgram.Commands.AddCommands
{
    public class AddAddressCommand : CommandBaseAsync
    {
        private readonly AddAddressViewModel _addAddressViewModel;
        private readonly AddressDataService _addressDataService;
        private readonly INavigationService _addressViewNavigationService;

        public AddAddressCommand(
            AddAddressViewModel addAddressViewModel,
            ServicesStore servicesStore,
            INavigationService addressViewNavigationService)
        {
            _addAddressViewModel = addAddressViewModel;
            _addressDataService = servicesStore._addressService;
            _addressViewNavigationService = addressViewNavigationService;

            _addAddressViewModel.PropertyChanged += OnViewModelPropertyChanged;
        }

        private void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_addAddressViewModel.City) ||
                e.PropertyName == nameof(_addAddressViewModel.Street))
            {
                OnCanExecuteChanged();
            }
        }

        public override bool CanExecute(object? parameter)
        {
            return !string.IsNullOrEmpty(_addAddressViewModel.City) &&
                   !string.IsNullOrEmpty(_addAddressViewModel.Street) &&
                   base.CanExecute(parameter);
        }

        public override async Task ExecuteAsync(object? parameter)
        {
            int newID = await _addressDataService.FindMaxEmptyID();
            Address address = new(
                newID,
                _addAddressViewModel.City,
                _addAddressViewModel.Street,
                _addAddressViewModel.House == null ? string.Empty : _addAddressViewModel.House,
                _addAddressViewModel.Structure == null ? string.Empty : _addAddressViewModel.Structure,
                _addAddressViewModel.Frame == null ? string.Empty : _addAddressViewModel.Frame
                );

            try
            {
                await _addressDataService.AddItemAsync(address);
                MessageBox.Show("Адрес добавлен", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
               _addressViewNavigationService.Navigate();
            }
            catch (Exception)
            {
                MessageBox.Show("Неизвестная ошибка!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}