using CourseProgram.Commands;
using CourseProgram.Commands.DeleteCommands;
using CourseProgram.Models;
using CourseProgram.Services;
using CourseProgram.Stores;
using CourseProgram.ViewModels.EntityViewModel;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace CourseProgram.ViewModels.ListingViewModel
{
    public class NomenclatureListingViewModel : BaseListingViewModel
    {
        #region fields
        private readonly ObservableCollection<NomenclatureViewModel> _allNomenclatures;

        public ICommand AddNomenclatureCommand {  get; }
        public ICommand DeleteNomenclatureCommand { get; }
        #endregion

        public NomenclatureListingViewModel(
            ServicesStore servicesStore, 
            SelectedStore selectedStore, 
            ControllersStore controllersStore,
            INavigationService addNomenclatureNavigationService) 
        {
            _selectedStore = selectedStore;
            _controllersStore = controllersStore;

            _allNomenclatures = new ObservableCollection<NomenclatureViewModel>();

            AddNomenclatureCommand = new NavigateCommand(addNomenclatureNavigationService);
            DeleteNomenclatureCommand = new DeleteNomenclatureCommand(this, servicesStore);
            SelectionChangedCommand = new RelayCommand<DataGrid>(SelectionChangedExecute);

            UpdateData();
            _items = _allNomenclatures;

            updateTimer = new()
            {
                Interval = TimeSpan.FromSeconds(3)
            };
            updateTimer.Tick += UpdateTimer_Tick;
            updateTimer.Start();
        }

        #region methods
        private async void UpdateTimer_Tick(object? sender, EventArgs e)
        {
            await UpdateDataAsync();
        }

        public override async Task UpdateDataAsync()
        {
            var currentSelected = SelectedItem;

            ObservableCollection<NomenclatureViewModel> _newAllNomenclatures = new ObservableCollection<NomenclatureViewModel>();

            IEnumerable<Nomenclature> temp = await _controllersStore.GetController<Nomenclature>().GetItems();
            foreach (var tempItem in temp)
            {
                var nomenclatureViewModel = new NomenclatureViewModel(tempItem);
                _newAllNomenclatures.Add(nomenclatureViewModel);
            }

            _allNomenclatures.Clear();

            foreach (NomenclatureViewModel model in _newAllNomenclatures)
                _allNomenclatures.Add(model);

            SelectedItem = Items.FirstOrDefault(i => i.ID == currentSelected?.ID);
        }

        protected override void Find()
        {
            if (!string.IsNullOrEmpty(TextFilter))
                SelectedItem = Items
                    .Where(obj => obj.Name.ToLower().Contains(TextFilter.ToLower()))
                    .FirstOrDefault();
        }

        public override async void UpdateData()
        {
            ObservableCollection<NomenclatureViewModel> _newAllNomenclatures = new ObservableCollection<NomenclatureViewModel>();

            IEnumerable<Nomenclature> temp = await _controllersStore.GetController<Nomenclature>().GetItems();
            foreach (var tempItem in temp)
            {
                var nomenclatureViewModel = new NomenclatureViewModel(tempItem);
                _newAllNomenclatures.Add(nomenclatureViewModel);
            }

            _allNomenclatures.Clear();

            foreach (NomenclatureViewModel model in _newAllNomenclatures)
                _allNomenclatures.Add(model);
        }
        #endregion

        #region properties
        private ObservableCollection<NomenclatureViewModel> _items;
        public ObservableCollection<NomenclatureViewModel> Items
        {
            get => _items;
            set
            {
                _items = value;
                OnPropertyChanged(nameof(Items));
            }
        }

        private NomenclatureViewModel _selectedItem;
        public NomenclatureViewModel SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                _selectedStore.CurrentNomenclature = value;
                OnPropertyChanged(nameof(SelectedItem));
            }
        }
        #endregion
    }
}