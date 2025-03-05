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
        private readonly ControllersStore _controllersStore;
        private readonly RouteListingViewModel _routeListingViewModel;
        private Route _route;
        private Machine _machine;
        private Driver _driver;

        public StartRouteCommand(ServicesStore servicesStore, ControllersStore controllersStore, RouteListingViewModel routeListingViewModel)
        {
            _servicesStore = servicesStore;
            _controllersStore = controllersStore;
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
            _machine = await _controllersStore.GetController<Machine>().GetItemByID((int)_route.MachineID);
            _driver = await _controllersStore.GetController<Driver>().GetItemByID((int)_route.DriverID);

            if (_machine.AddressID != null && _machine.AddressID != _route.AddressStartID)
            {
                MessageBox.Show("Машина не находится в пункте начала маршрута", "Маршрут нельзя начать", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!_driver.GetListCategories().Contains(_machine.CategoryValue))
            {
                MessageBox.Show("У данного водителя нет прав на управление этим ТС", "Маршрут нельзя начать", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

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