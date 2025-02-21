using CourseProgram.Models;
using CourseProgram.Stores;
using CourseProgram.ViewModels;
using System;
using System.Threading.Tasks;
using System.Windows;
using static CourseProgram.Models.Constants;

namespace CourseProgram.Commands
{
    public class CompleteHaulCommand : CommandBaseAsync
    {
        private readonly OperationalViewModel _viewModel;
        private readonly ServicesStore _servicesStore;

        public CompleteHaulCommand(OperationalViewModel viewModel, ServicesStore servicesStore)
        {
            _viewModel = viewModel;
            _servicesStore = servicesStore;
        }

        public override async Task ExecuteAsync(object? parameter)
        {
            bool readyComplete = true;
            foreach (var temp in _viewModel.Orders)
            {
                var tempModel = temp.GetModel();
                if (tempModel.Status != OrderStatusValues.Completed && tempModel.Status != OrderStatusValues.Cancelled)
                    readyComplete = false;
            }

            if (readyComplete)
            {
                MessageBoxResult message = MessageBox.Show("Вы уверены что хотите завершить рейс", "Подтверждение закрытия рейса", MessageBoxButton.YesNo);
                if (message == MessageBoxResult.Yes)
                {
                    IsExecuting = true;

                    try
                    {
                        Haul tempHaul = _viewModel.Item.GetModel();
                        var newItem = new Haul(tempHaul.ID, tempHaul.DateStart, DateOnly.FromDateTime(DateTime.Now), tempHaul.SumIncome);

                        bool result = await _servicesStore.GetService<Haul>().UpdateItemAsync(newItem);
                        if (result)
                            MessageBox.Show("Рейс завершен", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                        else
                            MessageBox.Show("Не удалось завершить рейс", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
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
            else
                MessageBox.Show("Нельзя завершить рейс, так как не все заказы завершены", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }
}