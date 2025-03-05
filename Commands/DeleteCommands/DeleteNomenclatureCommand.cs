using CourseProgram.Models;
using CourseProgram.Services;
using CourseProgram.Services.DataServices;
using CourseProgram.Stores;
using CourseProgram.ViewModels.ListingViewModel;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CourseProgram.Commands.DeleteCommands
{
    public class DeleteNomenclatureCommand : BaseDeleteCommand
    {
        private readonly NomenclatureListingViewModel _viewModel;

        public DeleteNomenclatureCommand(NomenclatureListingViewModel viewModel, ServicesStore servicesStore)
        {
            _viewModel = viewModel;
            _servicesStore = servicesStore;

            _viewModel.PropertyChanged += OnViewModelPropertyChanged;
        }

        private void OnViewModelPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_viewModel.SelectedItem))
                OnCanExecuteChanged();
        }

        protected override bool IsItemSelected()
        {
            return _viewModel.SelectedItem != null;
        }

        protected override async Task ExecuteDeleteAsync(object? parameter)
        {
            try
            {
                bool check = await ((NomenclatureDataService)_servicesStore.GetService<Nomenclature>()).CheckCanDelete(_viewModel.SelectedItem.ID);

                if (!check)
                    MessageBox.Show("Нельзя удалить номенклатуру так как она уже используется", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                else
                {
                    bool result = await _servicesStore.GetService<Nomenclature>().DeleteItemAsync(_viewModel.SelectedItem.ID);
                    if (result)
                        MessageBox.Show("Номенклатура удалена", "Успешно", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    else
                        MessageBox.Show("Не удалось удалить номенклатуру", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                await _viewModel.UpdateDataAsync();
            }
            catch (Exception ex)
            {
                await LogManager.Instance.WriteLogAsync($"ERROR {ex.Message}");
                MessageBox.Show("Неизвестная ошибка!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}