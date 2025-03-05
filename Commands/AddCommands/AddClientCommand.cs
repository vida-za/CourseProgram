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
                e.PropertyName == nameof(_viewModel.Checking))
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
                base.CanExecute(parameter);
        }

        public override async Task ExecuteAsync(object? parameter)
        {
            try
            {
                await _servicesStore.GetService<Client>().FindMaxEmptyID();
                int newID = await _servicesStore.GetService<Client>().GetFreeID();

                Client client = new(
                    newID,
                    _viewModel.Name,
                    _viewModel.Type,
                    _viewModel.INN,
                    _viewModel.KPP,
                    _viewModel.OGRN,
                    _viewModel.Phone,
                    _viewModel.Checking,
                    _viewModel.BIK,
                    _viewModel.Correspondent,
                    _viewModel.Bank
                    );

                int result = await _servicesStore.GetService<Client>().AddItemAsync(client);
                if (result > 0)
                    MessageBox.Show("Заказчик добавлен", "Успешно", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else
                    MessageBox.Show("Не удалось добавить заказчика", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);

                _navigationService.Navigate();
            }
            catch(Exception) 
            {
                MessageBox.Show("Неизвестная ошибка", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion
    }
}