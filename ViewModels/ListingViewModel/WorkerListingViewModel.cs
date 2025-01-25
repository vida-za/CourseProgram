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
    public class WorkerListingViewModel : BaseListingViewModel
    {
        #region fields
        private readonly ObservableCollection<WorkerViewModel> _allWorkers;
        private readonly ObservableCollection<WorkerViewModel> _rdyWorkers;
        private readonly ObservableCollection<WorkerViewModel> _disWorkers;

        public ICommand AddWorkerCommand { get; }
        public ICommand DeleteWorkerCommand { get; }
        public ICommand DetailWorkerCommand { get; }
        public ICommand SelectionChangedCommand { get; }
        public ICommand SwitchHandlerCommand { get; }
        #endregion

        public WorkerListingViewModel(
            ServicesStore servicesStore,
            SelectedStore selectedStore,
            INavigationService addWorkerNavigationService,
            INavigationService detailWorkerNavigationService)
        {
            _servicesStore = servicesStore;
            _selectedStore = selectedStore;

            _allWorkers = new ObservableCollection<WorkerViewModel>();
            _rdyWorkers = new ObservableCollection<WorkerViewModel>();
            _disWorkers = new ObservableCollection<WorkerViewModel>();

            AddWorkerCommand = new NavigateCommand(addWorkerNavigationService);
            DeleteWorkerCommand = new DeleteWorkerCommand(this, _servicesStore);
            DetailWorkerCommand = new NavigateDetailCommand(detailWorkerNavigationService);
            SelectionChangedCommand = new RelayCommand<DataGrid>(SelectionChangedExecute);
            SwitchHandlerCommand = new SwitchHandlerCommand(this);

            UpdateData();
            _items = _allWorkers;

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

        public override async void UpdateData()
        {
            ObservableCollection<WorkerViewModel> _newAllWorkers = new ObservableCollection<WorkerViewModel>();
            ObservableCollection<WorkerViewModel> _newRdyWorkers = new ObservableCollection<WorkerViewModel>();
            ObservableCollection<WorkerViewModel> _newDisWorkers = new ObservableCollection<WorkerViewModel>();

            IEnumerable<Worker> temp = await _servicesStore._workerService.GetItemsAsync();
            foreach (Worker worker in temp)
            {
                WorkerViewModel workerViewModel = new(worker);
                _newAllWorkers.Add(workerViewModel);
            }

            temp = await _servicesStore._workerService.GetItemsAsync();
            foreach (Worker worker in temp)
            {
                WorkerViewModel workerViewModel = new(worker);
                _newRdyWorkers.Add(workerViewModel);
            }

            temp = await _servicesStore._workerService.GetItemsAsync();
            foreach (Worker worker in temp)
            {
                WorkerViewModel workerViewModel = new(worker);
                _newDisWorkers.Add(workerViewModel);
            }

            _allWorkers.Clear();
            _rdyWorkers.Clear();
            _disWorkers.Clear();

            foreach (WorkerViewModel model in _newAllWorkers)
                _allWorkers.Add(model);

            foreach (WorkerViewModel model in _newRdyWorkers)
                _rdyWorkers.Add(model);

            foreach (WorkerViewModel model in _newDisWorkers)
                _disWorkers.Add(model);
        }

        protected override void Find()
        {
            if (!string.IsNullOrEmpty(TextFilter))
                SelectedItem = Items.FirstOrDefault(obj => obj.FIO.ToLower().Contains(TextFilter.ToLower()), SelectedItem);
        }

        private void SelectionChangedExecute(DataGrid dataGrid)
        {
            if (dataGrid.SelectedItem != null)
                dataGrid.ScrollIntoView(dataGrid.SelectedItem);
        }

        public override void SwitchHandler()
        {
            base.SwitchHandler();
        }

        public override async Task UpdateDataAsync()
        {
            ObservableCollection<WorkerViewModel> _newAllWorkers = new ObservableCollection<WorkerViewModel>();
            ObservableCollection<WorkerViewModel> _newRdyWorkers = new ObservableCollection<WorkerViewModel>();
            ObservableCollection<WorkerViewModel> _newDisWorkers = new ObservableCollection<WorkerViewModel>();

            IEnumerable<Worker> temp = await _servicesStore._workerService.GetItemsAsync();
            foreach (Worker worker in temp)
            {
                WorkerViewModel workerViewModel = new(worker);
                _newAllWorkers.Add(workerViewModel);
            }

            temp = await _servicesStore._workerService.GetItemsAsync();
            foreach (Worker worker in temp)
            {
                WorkerViewModel workerViewModel = new(worker);
                _newRdyWorkers.Add(workerViewModel);
            }

            temp = await _servicesStore._workerService.GetItemsAsync();
            foreach (Worker worker in temp)
            {
                WorkerViewModel workerViewModel = new(worker);
                _newDisWorkers.Add(workerViewModel);
            }

            _allWorkers.Clear();
            _rdyWorkers.Clear();
            _disWorkers.Clear();

            foreach (WorkerViewModel model in _newAllWorkers)
                _allWorkers.Add(model);

            foreach (WorkerViewModel model in _newRdyWorkers)
                _rdyWorkers.Add(model);

            foreach (WorkerViewModel model in _newDisWorkers)
                _disWorkers.Add(model);
        }
        #endregion

        #region properties
        private ObservableCollection<WorkerViewModel> _items;
        public ObservableCollection<WorkerViewModel> Items
        {
            get => _items;
            set
            { 
                _items = value;
                OnPropertyChanged(nameof(Items));
            }
        }

        private WorkerViewModel _selectedItem;
        public WorkerViewModel SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                _selectedStore.CurrentWorker = value;
                OnPropertyChanged(nameof(SelectedItem));
            }
        }
        #endregion
    }
}