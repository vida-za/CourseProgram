using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using CourseProgram.Models;
using CourseProgram.Services;
using CourseProgram.Stores;
using CourseProgram.ViewModels.AddViewModel;

namespace CourseProgram.Commands.AddCommands
{
    public class AddAddressCommand : BaseAddCommand
    {
        private readonly AddAddressViewModel _viewModel;

        public AddAddressCommand(
            AddAddressViewModel addAddressViewModel,
            ServicesStore servicesStore,
            INavigationService addressViewNavigationService)
        {
            _viewModel = addAddressViewModel;
            _servicesStore = servicesStore;
            _navigationService = addressViewNavigationService;

            _viewModel.PropertyChanged += OnViewModelPropertyChanged;
        }

        protected override void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_viewModel.City) ||
                e.PropertyName == nameof(_viewModel.Street))
            {
                OnCanExecuteChanged();
            }
        }

        public override bool CanExecute(object? parameter)
        {
            return !string.IsNullOrEmpty(_viewModel.City) &&
                   !string.IsNullOrEmpty(_viewModel.Street) &&
                   base.CanExecute(parameter);
        }

        public override async Task ExecuteAsync(object? parameter)
        {
             var address = new Address(
                -1,
                _viewModel.City,
                _viewModel.Street,
                _viewModel.House,
                _viewModel.Structure,
                _viewModel.Frame,
                true
                );

            try
            {
                await _servicesStore.GetService<Address>().FindMaxEmptyID();
                int result = await _servicesStore.GetService<Address>().AddItemAsync(address);
                if (result > 0)
                    MessageBox.Show("Адрес добавлен", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                else
                    MessageBox.Show("Не удалось добавить адрес", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);

               _navigationService.Navigate();
            }
            catch (Exception)
            {
                MessageBox.Show("Неизвестная ошибка!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}