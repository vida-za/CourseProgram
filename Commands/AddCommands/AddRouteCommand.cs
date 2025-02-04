using CourseProgram.Models;
using CourseProgram.Services;
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

        protected override void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs e) { }

        public override async Task ExecuteAsync(object? parameter)
        {
            var route = new Route(
                -1,
                _viewModel.SelectedMachine.ID,
                _viewModel.SelectedDriver.ID,
                Constants.RouteTypeValues.Empty,
                Constants.RouteStatusValues.Waiting,
                null,
                _viewModel.SelectedAddressStart.ID,
                _viewModel.SelectedAddressEnd.ID);

            IsExecuting = true;

            try
            {
                await _servicesStore._routeService.FindMaxEmptyID();
                int result = await _servicesStore._routeService.AddItemAsync(route);
                if (result > 0)
                {
                    MessageBox.Show("Маршрут добавлен", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
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
                IsExecuting = false;
            }
        }
    }
}