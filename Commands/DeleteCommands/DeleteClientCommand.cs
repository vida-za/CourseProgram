using CourseProgram.Models;
using CourseProgram.Services.DataServices;
using CourseProgram.Stores;
using CourseProgram.ViewModels.ListingViewModel;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;

namespace CourseProgram.Commands.DeleteCommands
{
    public class DeleteClientCommand : BaseDeleteCommand
    {
        private readonly ClientListingViewModel _viewModel;

        public DeleteClientCommand(ClientListingViewModel viewModel, ServicesStore servicesStore)
        {
            _viewModel = viewModel;
            _servicesStore = servicesStore;

            _viewModel.PropertyChanged += OnViewModelPropertyChanged;
        }

        private void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
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
                bool check = await ((ClientDataService)_servicesStore.GetService<Client>()).CheckCanDelete(_viewModel.SelectedItem.ID);

                if (!check)
                    MessageBox.Show("Нельзя удалить клиента так как он уже используется", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                else
                {
                    bool result = await _servicesStore.GetService<Client>().DeleteItemAsync(_viewModel.SelectedItem.ID);
                    if (result)
                        MessageBox.Show("Клиент удален", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                    else
                        MessageBox.Show("Не удалось удалить клиента", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                await _viewModel.UpdateDataAsync();
            }
            catch(Exception) 
            {
                MessageBox.Show("Неизвестная ошибка!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}