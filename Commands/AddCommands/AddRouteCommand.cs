using CourseProgram.Models;
using CourseProgram.Services;
using CourseProgram.Services.DataServices;
using CourseProgram.Stores;
using CourseProgram.ViewModels.AddViewModel;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;

namespace CourseProgram.Commands.AddCommands
{
    public class AddRouteCommand : BaseAddCommand
    {
        private readonly AddRouteViewModel _viewModel;

        public AddRouteCommand(AddRouteViewModel viewModel, ServicesStore servicesStore, INavigationService closeNavigationService)
        {
            _viewModel = viewModel;
            _servicesStore = servicesStore;
            _navigationService = closeNavigationService;
        }

        protected override void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs e) 
        {
        }

        public override async Task ExecuteAsync(object? parameter)
        {
            IsExecuting = true;

            try
            {
                await _servicesStore.GetService<Route>().FindMaxEmptyID();
                int newID = await _servicesStore.GetService<Route>().GetFreeID();

                var route = new Route(
                    newID,
                    _viewModel.SelectedMachine?.ID,
                    _viewModel.SelectedDriver?.ID,
                    Constants.RouteTypeValues.Empty,
                    Constants.RouteStatusValues.Waiting,
                    null,
                    _viewModel.SelectedAddressStart?.ID,
                    _viewModel.SelectedAddressEnd?.ID);


                int result = await _servicesStore.GetService<Route>().AddItemAsync(route);
                if (result > 0)
                {
                    bool resultCatch = true;

                    if (_viewModel.SelectedOrders != null) 
                    {
                        foreach (var temp in _viewModel.SelectedOrders)
                        {
                            bool tempRes = await ((RouteDataService)_servicesStore.GetService<Route>()).AddCatchWithOrder(temp.ID, result);
                            if (!tempRes) resultCatch = false;
                        }
                    }

                    if (resultCatch)
                        MessageBox.Show("Маршрут добавлен", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                    else
                        MessageBox.Show("Маршрут добавлен, но не удалось привязать заказы", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                    MessageBox.Show("Не удалось добавить маршрут", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                await LogManager.Instance.WriteLogAsync($"ERROR in {nameof(AddRouteCommand)}: {ex.Message}");
                MessageBox.Show("Неизвестная ошибка!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                _navigationService.Navigate();
                IsExecuting = false;
            }
        }
    }
}