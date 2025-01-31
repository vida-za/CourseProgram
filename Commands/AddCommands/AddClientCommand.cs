using CourseProgram.Models;
using CourseProgram.Services;
using CourseProgram.Stores;
using CourseProgram.ViewModels.AddViewModel;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CourseProgram.Commands.AddCommands
{
    public class AddClientCommand : BaseAddCommand
    {
        #region fields
        private readonly AddClientViewModel _viewModel;
        #endregion

        public AddClientCommand(
            AddClientViewModel viewModel,
            ServicesStore servicesStore,
            INavigationService navigationService)
        {
            _viewModel = viewModel;
            _servicesStore = servicesStore;
            _navigationService = navigationService;

            _viewModel.PropertyChanged += OnViewModelPropertyChanged;
        }

        #region methods
        protected override void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_viewModel.Name) ||
                e.PropertyName == nameof(_viewModel.Type) ||
                e.PropertyName == nameof(_viewModel.INN) ||
                e.PropertyName == nameof(_viewModel.KPP) ||
                e.PropertyName == nameof(_viewModel.OGRN) ||
                e.PropertyName == nameof(_viewModel.Phone) ||
                e.PropertyName == nameof(_viewModel.Checking) ||
                e.PropertyName == nameof(_viewModel.PhoneLoad) ||
                e.PropertyName == nameof(_viewModel.PhoneOnLoad))
                OnCanExecuteChanged();
        }

        public override bool CanExecute(object? parameter)
        {
            return !string.IsNullOrEmpty(_viewModel.Name) &&
                !string.IsNullOrEmpty(_viewModel.Type) &&
                !string.IsNullOrEmpty(_viewModel.INN) &&
                !string.IsNullOrEmpty(_viewModel.KPP) &&
                !string.IsNullOrEmpty(_viewModel.OGRN) &&
                !string.IsNullOrEmpty(_viewModel.Phone) &&
                !string.IsNullOrEmpty(_viewModel.Checking) &&
                !string.IsNullOrEmpty(_viewModel.PhoneLoad) &&
                !string.IsNullOrEmpty(_viewModel.PhoneOnLoad) &&
                base.CanExecute(parameter);
        }

        public override async Task ExecuteAsync(object? parameter)
        {
            Client client = new(
                -1,
                _viewModel.Name,
                _viewModel.Type,
                _viewModel.INN,
                _viewModel.KPP,
                _viewModel.OGRN,
                _viewModel.Phone,
                _viewModel.Checking,
                _viewModel.BIK,
                _viewModel.Correspondent,
                _viewModel.Bank,
                _viewModel.PhoneLoad,
                _viewModel.PhoneOnLoad
                );

            try
            {
                await _servicesStore._clientService.FindMaxEmptyID();
                int result = await _servicesStore._clientService.AddItemAsync(client);
                if (result > 0)
                    MessageBox.Show("Заказчик добавлен", "Успешно", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else
                    MessageBox.Show("Не удалось добавить заказчика", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);

                _navigationService.Navigate();
            }
            catch(Exception) { }
        }
        #endregion
    }
}