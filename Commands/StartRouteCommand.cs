using CourseProgram.Models;
using CourseProgram.Services;
using CourseProgram.Services.DataServices;
using CourseProgram.Stores;
using CourseProgram.ViewModels.ListingViewModel;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace CourseProgram.Commands
{
    public class StartRouteCommand : CommandBaseAsync
    {
        private readonly ServicesStore _servicesStore;
        private readonly RouteListingViewModel _routeListingViewModel;
        private Route _route;

        public StartRouteCommand(ServicesStore servicesStore, RouteListingViewModel routeListingViewModel)
        {
            _servicesStore = servicesStore;
            _routeListingViewModel = routeListingViewModel;

            _routeListingViewModel.PropertyChanged += _routeListingViewModel_PropertyChanged;
        }

        private void _routeListingViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_routeListingViewModel.SelectedWaitRoute))
            {
                _route = _routeListingViewModel.SelectedWaitRoute?.GetModel();
                OnCanExecuteChanged();
            }
        }

        public override bool CanExecute(object? parameter)
        {
            return base.CanExecute(parameter)
                && _route?.MachineID != null
                && _route?.DriverID != null
                && _route?.AddressStartID != null
                && _route?.AddressEndID != null;
        }

        public override async Task ExecuteAsync(object? parameter)
        {
            MessageBoxResult message = MessageBox.Show("Начать маршрут?", "Подтверждение старта", MessageBoxButton.YesNo);
            if (message == MessageBoxResult.Yes)
            {
                IsExecuting = true;

                try
                {
                    bool result = await ((RouteDataService)_servicesStore.GetService<Route>()).StartRouteAsync(_route.ID);
                    if (result)
                    {
                        await _routeListingViewModel.UpdateDataAsync();
                        MessageBox.Show("Маршрут обновлен", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                        MessageBox.Show("Не удалость начать маршрут", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch (Exception ex)
                {
                    await LogManager.Instance.WriteLogAsync($"Error in {nameof(StartRouteCommand)}: {ex.Message}");
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