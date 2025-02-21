﻿using CourseProgram.Models;
using CourseProgram.Stores;
using CourseProgram.ViewModels.ListingViewModel;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace CourseProgram.Commands
{
    public class CancelRouteCommand : CommandBaseAsync
    {
        ServicesStore _servicesStore;
        RouteListingViewModel _routeListingViewModel;

        public CancelRouteCommand(ServicesStore servicesStore, RouteListingViewModel routeListingViewModel)
        {
            _servicesStore = servicesStore;
            _routeListingViewModel = routeListingViewModel;
        }

        public override async Task ExecuteAsync(object? parameter)
        {
            MessageBoxResult message = MessageBox.Show("Вы уверены что хотите отменить маршрут", "Подтверждение отмены", MessageBoxButton.YesNo);
            if (message == MessageBoxResult.Yes && _routeListingViewModel.SelectedWaitRoute != null)
            {
                IsExecuting = true;

                try
                {
                    Route tempRoute = _routeListingViewModel.SelectedWaitRoute.GetModel();
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
                        await _routeListingViewModel.UpdateDataAsync();
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