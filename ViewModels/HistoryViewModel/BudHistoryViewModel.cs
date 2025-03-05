using CourseProgram.Models;
using CourseProgram.Stores;
using CourseProgram.ViewModels.EntityViewModel;
using CourseProgram.ViewModels.ListingViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Command;
using System.Windows.Controls;
using CourseProgram.Services;
using System.Windows.Input;
using CourseProgram.Commands;

namespace CourseProgram.ViewModels.HistoryViewModel
{
    public class BudHistoryViewModel : BaseListingViewModel
    {
        public ICommand DetailBudCommand { get; }

        public BudHistoryViewModel(SelectedStore selectedStore, ControllersStore controllersStore, INavigationService detailBudNavigationService)
        {
            _selectedStore = selectedStore;
            _controllersStore = controllersStore;

            _items = new ObservableCollection<BudViewModel>();

            DetailBudCommand = new NavigateDetailCommand(detailBudNavigationService);
            SelectionChangedCommand = new RelayCommand<DataGrid>(SelectionChangedExecute);

            UpdateData();
            updateTimer = new()
            {
                Interval = TimeSpan.FromSeconds(3)
            };
            updateTimer.Tick += UpdateTimer_Tick;
            updateTimer.Start();
        }

        private async void UpdateTimer_Tick(object? sender, EventArgs e)
        {
            await UpdateDataAsync();
        }

        public override async void UpdateData()
        {
            IEnumerable<Bud> temp = await _controllersStore.GetController<Bud>().GetItems();
            foreach (var item in temp)
            {
                if (item.Status != Constants.BudStatusValues.Waiting)
                {
                    var budViewModel = new BudViewModel(item, _controllersStore);
                    _items.Add(budViewModel);
                }
            }
        }

        public override async Task UpdateDataAsync()
        {
            var currentSelected = Selecteditem;

            ObservableCollection<BudViewModel> _newItems = new ObservableCollection<BudViewModel>();

            IEnumerable<Bud> temp = await _controllersStore.GetController<Bud>().GetItems();
            foreach (var item in temp)
            {
                if (item.Status != Constants.BudStatusValues.Waiting)
                {
                    var budViewModel = new BudViewModel(item, _controllersStore);
                    _newItems.Add(budViewModel);
                }
            }

            _items.Clear();

            foreach (var item in _newItems)
                _items.Add(item);

            Selecteditem = Items.FirstOrDefault(i => i.ID == currentSelected?.ID);
        }

        protected override void Find()
        {
            if (!string.IsNullOrEmpty(TextFilter))
                Selecteditem = Items.FirstOrDefault(obj => obj.ClientName.ToLower().Contains(TextFilter.ToLower()), Selecteditem);
        }

        private ObservableCollection<BudViewModel> _items;
        public ObservableCollection<BudViewModel> Items
        {
            get => _items;
            set
            {
                _items = value;
                OnPropertyChanged(nameof(Items));
            }
        }

        private BudViewModel _selectedItem;
        public BudViewModel Selecteditem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                _selectedStore.CurrentBud = _selectedItem;
                OnPropertyChanged(nameof(Selecteditem));
            }
        }
    }
}