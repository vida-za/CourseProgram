using CourseProgram.Models;
using CourseProgram.Services;
using CourseProgram.Stores;
using CourseProgram.ViewModels.AddViewModel;
using CourseProgram.ViewModels.EntityViewModel;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using static CourseProgram.Models.Constants;

namespace CourseProgram.Commands.AddCommands
{
    public class AddBudCommand : BaseAddCommand
    {
        private readonly AddBudViewModel _viewModel;

        public AddBudCommand(AddBudViewModel viewModel, ServicesStore servicesStore, INavigationService operationalViewNavigationService)
        {
            _viewModel = viewModel;
            _servicesStore = servicesStore;
            _navigationService = operationalViewNavigationService;

            _viewModel.PropertyChanged += OnViewModelPropertyChanged;
        }

        protected override void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_viewModel.SelectedClient) ||
                e.PropertyName == nameof(_viewModel.SelectedWorker) ||
                e.PropertyName == nameof(_viewModel.Cargos.Count))
            {
                OnCanExecuteChanged();
            }
        }

        public override bool CanExecute(object? parameter)
        {
            return _viewModel.SelectedClient != null &&
                   _viewModel.SelectedWorker != null &&
                   _viewModel.Cargos.Count > 0 &&
                   base.CanExecute(parameter);
        }

        public override async Task ExecuteAsync(object? parameter)
        {
            var bud = new Bud(
                -1,
                _viewModel.SelectedClient.ID,
                _viewModel.SelectedWorker.ID,
                DateTime.Now,
                BudStatusValues.Waiting,
                _viewModel.Description); 

            try
            {
                await _servicesStore.GetService<Bud>().FindMaxEmptyID();
                int resultBud = await _servicesStore.GetService<Bud>().AddItemAsync(bud);
                if (resultBud > 0)
                {
                    bool resultCargo = true;
                    foreach (var cargo in _viewModel.Cargos)
                    {
                        if (cargo.Count != "0")
                        {
                            FixFieldsCargo(cargo);

                            await _servicesStore.GetService<Cargo>().FindMaxEmptyID();
                            int temp = await _servicesStore.GetService<Cargo>().AddItemAsync(cargo.ToCargo(resultBud));
                            if (temp == 0)
                                resultCargo = false;
                        }
                    }

                    if (resultCargo)
                        MessageBox.Show("Заявка успешно создана", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                    else
                        MessageBox.Show("Заявка создана, но груз добавит в БД не удалось", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                    MessageBox.Show("Не удалось добавить заявку в БД", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);

                _navigationService.Navigate();
            }
            catch (Exception ex)
            {
                await LogManager.Instance.WriteLogAsync($"ERROR in {nameof(AddBudCommand)}: {ex.Message}");
                MessageBox.Show("Неизвестная ошибка!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void FixFieldsCargo(CargoViewModel cargo)
        {
            if (!(cargo.Volume != null && cargo.Weight != "0"))
            {
                Nomenclature nomenclature = await _servicesStore.GetService<Nomenclature>().GetItemAsync(cargo.NomenclatureID);

                if (!float.TryParse(cargo.Volume, out _) && int.TryParse(cargo.Count, out int counts) && nomenclature.Length != null && nomenclature.Width != null && nomenclature.Height != null)
                {
                    float newVolume = (float)Math.Round((float)nomenclature.Length * (float)nomenclature.Width * (float)nomenclature.Height * counts, 2);
                    if (newVolume <= 0)
                        newVolume = 0.01f;

                    cargo.Volume = newVolume.ToString();
                }

                if (cargo.Weight == "0" && nomenclature.Weight != null)
                    cargo.Weight = nomenclature.Weight.ToString();
            }
        }
    }
}