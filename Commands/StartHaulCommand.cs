using CourseProgram.Models;
using CourseProgram.Stores;
using CourseProgram.ViewModels;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace CourseProgram.Commands
{
    public class StartHaulCommand : CommandBaseAsync
    {
        private readonly OperationalViewModel _viewModel;
        private readonly ServicesStore _servicesStore;

        public StartHaulCommand(OperationalViewModel viewModel, ServicesStore servicesStore)
        {
            _viewModel = viewModel;
            _servicesStore = servicesStore;
        }

        public override async Task ExecuteAsync(object? parameter)
        {

            MessageBoxResult message = MessageBox.Show("Вы уверены что хотите создать новый рейс?", "Создание рейса", MessageBoxButton.YesNo);
            if (message == MessageBoxResult.Yes)
            {
                IsExecuting = true;

                try
                {
                    await _servicesStore.GetService<Haul>().FindMaxEmptyID();
                    int newId = await _servicesStore.GetService<Haul>().GetFreeID();

                    var haul = new Haul(newId, DateOnly.FromDateTime(DateTime.Now), null, null);

                    int result = await _servicesStore.GetService<Haul>().AddItemAsync(haul);
                    if (result > 0)
                    {
                        await _viewModel.UpdateDataAsync();
                        MessageBox.Show("Рейс создан", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                        MessageBox.Show("Не удалось создать рейс", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
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