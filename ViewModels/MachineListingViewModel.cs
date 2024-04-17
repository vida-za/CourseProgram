﻿using CourseProgram.Commands;
using CourseProgram.Models;
using CourseProgram.Services;
using CourseProgram.Services.DataServices;
using CourseProgram.Stores;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace CourseProgram.ViewModels
{
    public class MachineListingViewModel : BaseListingViewModel
    {
        public MachineListingViewModel(
            ServicesStore servicesStore,
            SelectedStore selectedStore,
            INavigationService addMachineNavigationService,
            INavigationService detailMachineNavigationService) 
        {
            _servicesStore = servicesStore;
            _selectedStore = selectedStore;

            _disMachines = new ObservableCollection<MachineViewModel>();
            _rdyMachines = new ObservableCollection<MachineViewModel>();
            _items = new ObservableCollection<MachineViewModel>();

            SwitchMachinesCommand = new SwitchMachinesCommand(this);
            AddMachineCommand = new NavigateCommand(addMachineNavigationService);
            DeleteMachineCommand = new DeleteMachineCommand(this, _servicesStore._machineService);
            DetailMachineCommand = new NavigateDetailCommand(detailMachineNavigationService);
            SelectionChangedCommand = new RelayCommand<DataGrid>(SelectionChangedExecute);

            UpdateData();
            _items = _rdyMachines;

            updateTimer = new()
            {
                Interval = TimeSpan.FromSeconds(3)
            };
            updateTimer.Tick += UpdateTimer_Tick;
            updateTimer.Start();
        }

        private void UpdateTimer_Tick(object? sender, EventArgs e)
        {
            UpdateData();
        }

        public override async void UpdateData()
        {
            _disMachines.Clear();
            _rdyMachines.Clear();

            IEnumerable<Machine> temp = await _servicesStore._machineService.GetItemsAsync();
            foreach (Machine machine in temp)
            {
                MachineViewModel machineViewModel = new(machine);
                _rdyMachines.Add(machineViewModel);
            }

            temp = await _servicesStore._machineService.GetDisMachinesAsync();
            foreach (Machine machine in temp)
            {
                MachineViewModel machineViewModel = new(machine);
                _disMachines.Add(machineViewModel);
            }
        }

        private readonly ServicesStore _servicesStore;
        private readonly SelectedStore _selectedStore;

        private readonly DispatcherTimer updateTimer;

        private readonly ObservableCollection<MachineViewModel> _disMachines;
        private readonly ObservableCollection<MachineViewModel> _rdyMachines;

        private ObservableCollection<MachineViewModel> _items;
        public ObservableCollection<MachineViewModel> Items
        {
            get => _items;
            set
            {
                _items = value;
                OnPropertyChanged(nameof(Items));
            }
        }

        public ICommand SwitchMachinesCommand { get; }
        public ICommand AddMachineCommand { get; }
        public ICommand DeleteMachineCommand { get; }
        public ICommand DetailMachineCommand { get; }
        public ICommand SelectionChangedCommand { get; }

        private MachineViewModel _selectedItem;
        public MachineViewModel SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                _selectedStore.CurrentMachine = SelectedItem;
                OnPropertyChanged(nameof(SelectedItem));
            }
        }

        private bool _stateCheckedBusy;
        public bool StateCheckedBusy
        {
            get => _stateCheckedBusy;
            set
            {
                _stateCheckedBusy = value;
                OnPropertyChanged(nameof(StateCheckedBusy));
            }
        }

        public void SwitchMachines()
        {
            if (StateCheckedBusy)
                Items = new ObservableCollection<MachineViewModel>(_disMachines);
            else
                Items = new ObservableCollection<MachineViewModel>(_rdyMachines);
                
        }

        protected override void Find()
        {
            if (!String.IsNullOrEmpty(TextFilter))
                SelectedItem = Items.FirstOrDefault(obj => obj.Name.Contains(TextFilter), SelectedItem);
        }

        private void SelectionChangedExecute(DataGrid dataGrid)
        {
            if (dataGrid.SelectedItem != null)
                dataGrid.ScrollIntoView(dataGrid.SelectedItem);
        }
    }
}