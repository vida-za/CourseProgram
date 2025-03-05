using CourseProgram.Models;
using CourseProgram.Services;
using CourseProgram.Stores;
using CourseProgram.ViewModels.EntityViewModel;
using System;
using System.Threading.Tasks;
using System.Windows;
using static CourseProgram.Models.Constants;

namespace CourseProgram.Commands
{
    public class UpdateBudCommand : CommandBaseAsync
    {
        private readonly ServicesStore _servicesStore;
        private readonly BudViewModel _budViewModel;
        private readonly bool _isAccept;
        private readonly INavigationService _navigationService;

        public UpdateBudCommand(ServicesStore servicesStore, BudViewModel budViewModel, bool isAccept, INavigationService operationlViewNavigationService)
        {
            _servicesStore = servicesStore;
            _budViewModel = budViewModel;
            _isAccept = isAccept;
            _navigationService = operationlViewNavigationService;
        }

        public override async Task ExecuteAsync(object? parameter)
        {
            try
            {
                Bud temp = _budViewModel.GetModel();
                var newItem = new Bud(
                    temp.ID,
                    temp.ClientID,
                    temp.WorkerID,
                    temp.TimeBud,
                    _isAccept ? BudStatusValues.Accepted : BudStatusValues.Cancelled,
                    temp.Description,
                    temp.AddressLoadID,
                    temp.AddressOnLoadID,
                    temp.DateTimeLoadPlan,
                    temp.DateTimeOnLoadPlan);

                bool result = await _servicesStore.GetService<Bud>().UpdateItemAsync(newItem);
                if (result)
                    MessageBox.Show("Статус заявки успешно обновлен", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                else
                    MessageBox.Show("Не удалось изменить статус", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);

                _navigationService.Navigate();
            }
            catch (Exception)
            {
                MessageBox.Show("Неизвестная ошибка!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}