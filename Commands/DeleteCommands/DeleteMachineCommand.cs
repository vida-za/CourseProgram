using CourseProgram.Models;
using CourseProgram.Services;
using CourseProgram.Stores;
using CourseProgram.ViewModels.ListingViewModel;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using static CourseProgram.Models.Constants;

namespace CourseProgram.Commands.DeleteCommands
{
    public class DeleteMachineCommand : BaseDeleteCommand
    {
        private readonly MachineListingViewModel _viewModel;

        public DeleteMachineCommand(MachineListingViewModel listingViewModel, ServicesStore servicesStore)
        {
            _viewModel = listingViewModel;
            _servicesStore = servicesStore;

            _viewModel.PropertyChanged += OnViewModelPropertyChanged;
        }

        private void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_viewModel.SelectedItem))
            {
                OnCanExecuteChanged();
            }
        }

        public override bool CanExecute(object? parameter)
        {
            return base.CanExecute(parameter) && _viewModel.SelectedItem.GetModel().TimeEnd == null;
        }

        protected override bool IsItemSelected()
        {
            return _viewModel.SelectedItem != null;
        }

        protected override async Task ExecuteDeleteAsync(object? parameter)
        {
            try
            {
                Machine temp = _viewModel.SelectedItem.GetModel();
                var newItem = new Machine(
                    temp.ID,
                    temp.TypeMachine,
                    temp.TypeBodywork,
                    temp.TypeLoading,
                    temp.LoadCapacity,
                    temp.Volume,
                    temp.HydroBoard,
                    temp.LengthBodywork,
                    temp.WidthBodywork,
                    temp.HeightBodywork,
                    temp.Stamp,
                    temp.Name,
                    temp.StateNumber,
                    temp.Status,
                    temp.TimeStart,
                    DateTime.Now,
                    temp.AddressID,
                    temp.CategoryValue);

                bool result = await _servicesStore.GetService<Machine>().UpdateItemAsync(newItem);
                if (result)
                    MessageBox.Show("Машина успешно удалена", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                else
                    MessageBox.Show("Не удалось удалить машину", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);

                await _viewModel.UpdateDataAsync();
            }
            catch (Exception ex) 
            {
                await LogManager.Instance.WriteLogAsync($"ERROR {ex.Message}");
            }
        }
    }
}