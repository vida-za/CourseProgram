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
    public class AddNomenclatureCommand : BaseAddCommand
    {
        private readonly AddNomenclatureViewModel _viewModel;

        public AddNomenclatureCommand(AddNomenclatureViewModel viewModel, ServicesStore servicesStore, INavigationService navigationService)
        {
            _viewModel = viewModel;
            _servicesStore = servicesStore;
            _navigationService = navigationService;

            _viewModel.PropertyChanged += OnViewModelPropertyChanged;
        }

        protected override void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_viewModel.Name) ||
                e.PropertyName == nameof(_viewModel.Type) ||
                e.PropertyName == nameof(_viewModel.CategoryCargo) ||
                e.PropertyName == nameof(_viewModel.Unit))
            {
                OnCanExecuteChanged();
            }
        }

        public override bool CanExecute(object? parameter)
        {
            return !string.IsNullOrEmpty(_viewModel.Name)
                && !string.IsNullOrEmpty(_viewModel.Type)
                && !string.IsNullOrEmpty(_viewModel.CategoryCargo)
                && !string.IsNullOrEmpty(_viewModel.Unit)
                && base.CanExecute(parameter);
        }

        public override async Task ExecuteAsync(object? parameter)
        {
            Nomenclature nomenclature = new Nomenclature(
                -1,
                _viewModel.Name,
                _viewModel.Type,
                _viewModel.CategoryCargo,
                _viewModel.Length == null ? null : float.Parse(_viewModel.Length),
                _viewModel.Width == null ? null : float.Parse(_viewModel.Width),
                _viewModel.Height == null ? null : float.Parse(_viewModel.Height),
                _viewModel.Weight == null ? null : float.Parse(_viewModel.Weight),
                _viewModel.Unit,
                _viewModel.Pack == null ? Constants.GetEnumDescription(Constants.NomenclaturePackingValues.Null) : _viewModel.Pack,
                _viewModel.NeedTemperature,
                _viewModel.DangerousClass == null ? Constants.GetEnumDescription(Constants.NomenclatureDangerousValues.Null) : _viewModel.DangerousClass,
                _viewModel.Manufacturer,
                _viewModel.ExpiryDate == null ? null : int.Parse(_viewModel.ExpiryDate)
                );

            try
            {
                await _servicesStore.GetService<Nomenclature>().FindMaxEmptyID();
                int result = await _servicesStore.GetService<Nomenclature>().AddItemAsync(nomenclature);
                if (result > 0)
                    MessageBox.Show("Элемент добавлен!", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                else
                    MessageBox.Show("Не удалось добавить номенклатуру", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);

                _navigationService.Navigate();
            }
            catch (Exception)
            {
                MessageBox.Show("Неизвестная ошибка!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}