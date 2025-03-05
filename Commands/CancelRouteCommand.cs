using CourseProgram.Models;
using CourseProgram.Stores;
using CourseProgram.ViewModels.ListingViewModel;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace CourseProgram.Commands
{
    public class CancelRouteCommand : CommandBaseAsync
    {
        private readonly ServicesStore _servicesStore;
        private readonly RouteListingViewModel _viewModel;

        public CancelRouteCommand(ServicesStore servicesStore, RouteListingViewModel viewModel)
        {
            _servicesStore = servicesStore;
            _viewModel = viewModel;

            _viewModel.PropertyChanged += _viewModel_PropertyChanged;
        }

        private void _viewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_viewModel.SelectedWaitRoute))
            {
                OnCanExecuteChanged();
            }
        }

        public override bool CanExecute(object? parameter)
        {
            return base.CanExecute(parameter) && _viewModel.SelectedWaitRoute != null;
        }

        public override async Task ExecuteAsync(object? parameter)
        {
            MessageBoxResult message = MessageBox.Show("Вы уверены что хотите отменить маршрут", "Подтверждение отмены", MessageBoxButton.YesNo);
            if (message == MessageBoxResult.Yes && _viewModel.SelectedWaitRoute != null)
            {
                IsExecuting = true;

                try
                {
                    Route tempRoute = _viewModel.SelectedWaitRoute.GetModel();
                    var newItem = new Route(
                        tempRoute.ID,
                        tempRoute.MachineID,
                        tempRoute.DriverID,
                        tempRoute.Type,
                        Constants.RouteStatusValues.Cancelled,
                        tempRoute.CompleteTime,
                        tempRoute.AddressStartID,
                        tempRoute.AddressEndID);

                    bool result = await _servicesStore.GetService<Route>().UpdateItemAsync(newItem);
                    if (result)
                    {
                        await _viewModel.UpdateDataAsync();
                        MessageBox.Show("Маршрут отменен!", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                        MessageBox.Show("Не удалось отменить маршрут", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch (Exception)
                {
                    MessageBox.Show("Неизвестная ошибка!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                finally
                {
                    IsExecuting = false;
                }
            }
        }
    }
}