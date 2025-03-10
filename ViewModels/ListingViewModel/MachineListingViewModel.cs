﻿using CourseProgram.Commands;
using CourseProgram.Commands.DeleteCommands;
using CourseProgram.Controllers.DataControllers.EntityDataControllers;
using CourseProgram.Models;
using CourseProgram.Services;
using CourseProgram.Services.DataServices;
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
    public class MachineListingViewModel : BaseListingViewModel
    {
        public MachineListingViewModel(
            ServicesStore servicesStore,
            SelectedStore selectedStore,
            ControllersStore controllersStore,
            INavigationService addMachineNavigationService,
            INavigationService detailMachineNavigationService)
        {
            _selectedStore = selectedStore;
            _controllersStore = controllersStore;

            _disMachines = new ObservableCollection<MachineViewModel>();
            _rdyMachines = new ObservableCollection<MachineViewModel>();
            _items = new ObservableCollection<MachineViewModel>();

            SwitchMachinesCommand = new SwitchHandlerCommand(this);
            AddMachineCommand = new NavigateCommand(addMachineNavigationService);
            DeleteMachineCommand = new DeleteMachineCommand(this, servicesStore);
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

        private async void UpdateTimer_Tick(object? sender, EventArgs e)
        {
            await UpdateDataAsync();
        }

        public override async void UpdateData()
        {
            ObservableCollection<MachineViewModel> _newDisMachines = new ObservableCollection<MachineViewModel>();
            ObservableCollection<MachineViewModel> _newRdyMachines = new ObservableCollection<MachineViewModel>();

            IEnumerable<Machine> temp = await ((MachineDataController)_controllersStore.GetController<Machine>()).GetActiveMachines(true);
            foreach (Machine machine in temp)
            {
                var machineViewModel = new MachineViewModel(machine, _controllersStore);
                _newRdyMachines.Add(machineViewModel);
            }

            temp = await ((MachineDataController)_controllersStore.GetController<Machine>()).GetActiveMachines(false);
            foreach (Machine machine in temp)
            {
                var machineViewModel = new MachineViewModel(machine, _controllersStore);
                _newDisMachines.Add(machineViewModel);
            }

            _disMachines.Clear();
            _rdyMachines.Clear();

            foreach (MachineViewModel model in _newDisMachines)
                _disMachines.Add(model);
            foreach (MachineViewModel model in _newRdyMachines)
                _rdyMachines.Add(model);
        }

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

        public override void SwitchHandler()
        {
            if (StateCheckedBusy)
                Items = new ObservableCollection<MachineViewModel>(_disMachines);
            else
                Items = new ObservableCollection<MachineViewModel>(_rdyMachines);

        }

        protected override void Find()
        {
            if (!string.IsNullOrEmpty(TextFilter))
                SelectedItem = Items.FirstOrDefault(obj => obj.Name.ToLower().Contains(TextFilter.ToLower()), SelectedItem);
        }

        public override async Task UpdateDataAsync()
        {
            var currentSelected = SelectedItem;

            ObservableCollection<MachineViewModel> _newDisMachines = new ObservableCollection<MachineViewModel>();
            ObservableCollection<MachineViewModel> _newRdyMachines = new ObservableCollection<MachineViewModel>();

            IEnumerable<Machine> temp = await ((MachineDataController)_controllersStore.GetController<Machine>()).GetActiveMachines(true);
            foreach (Machine machine in temp)
            {
                var machineViewModel = new MachineViewModel(machine, _controllersStore);
                _newRdyMachines.Add(machineViewModel);
            }

            temp = await ((MachineDataController)_controllersStore.GetController<Machine>()).GetActiveMachines(false);
            foreach (Machine machine in temp)
            {
                var machineViewModel = new MachineViewModel(machine, _controllersStore);
                _newDisMachines.Add(machineViewModel);
            }

            _disMachines.Clear();
            _rdyMachines.Clear();

            foreach (MachineViewModel model in _newDisMachines)
                _disMachines.Add(model);

            foreach (MachineViewModel model in _newRdyMachines)
                _rdyMachines.Add(model);

            SelectedItem = Items.FirstOrDefault(i => i.ID == currentSelected?.ID);
        }
    }
}