﻿using CourseProgram.Commands;
using CourseProgram.Commands.AddCommands;
using CourseProgram.Controllers.DataControllers.EntityDataControllers;
using CourseProgram.Models;
using CourseProgram.Services;
using CourseProgram.Services.DataServices;
using CourseProgram.Stores;
using CourseProgram.ViewModels.EntityViewModel;
using GalaSoft.MvvmLight.Command;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace CourseProgram.ViewModels.AddViewModel
{
    public class AddRouteViewModel : BaseAddViewModel
    {
        private readonly ControllersStore _controllersStore;

        public ICommand SelectionChangedCommand { get; }

        public AddRouteViewModel(ServicesStore servicesStore, ControllersStore controllersStore, INavigationService closeNavigationService) 
        {
            _controllersStore = controllersStore;

            _orders = new ObservableCollection<OrderViewModel>();

            SubmitCommand = new AddRouteCommand(this, servicesStore, closeNavigationService);
            CancelCommand = new NavigateCommand(closeNavigationService);
            SelectionChangedCommand = new RelayCommand<object>(UpdateSelectedOrders);

            UpdateData();
        }

        public IEnumerable<Machine> Machines { get; set; }
        public IEnumerable<Driver> Drivers { get; set; }
        public IEnumerable<Address> Addresses { get; set; }
        private ObservableCollection<OrderViewModel> _orders;
        public ObservableCollection<OrderViewModel> Orders 
        { 
            get => _orders;
            set
            {
                _orders = value;
                OnPropertyChanged(nameof(Orders));
            } 
        }

        private async void UpdateData()
        {
            Machines = await ((MachineDataController)_controllersStore.GetController<Machine>()).GetActiveMachines(true);
            Drivers = await ((DriverDataController)_controllersStore.GetController<Driver>()).GetActiveDrivers(true);
            Addresses = await ((AddressDataController)_controllersStore.GetController<Address>()).GetItems();

            IEnumerable<Order> temp = await _controllersStore.GetController<Order>().GetItems();
            foreach (var ord in temp)
            {
                if (ord.Status == Constants.OrderStatusValues.InProgress || ord.Status == Constants.OrderStatusValues.Waiting)
                {
                    var orderViewModel = new OrderViewModel(ord, _controllersStore);
                    Orders.Add(orderViewModel);
                }
            }

            OnPropertyChanged(nameof(Machines));
            OnPropertyChanged(nameof(Drivers));
            OnPropertyChanged(nameof(Addresses));
        }

        private void UpdateSelectedOrders(object parameter)
        {
            if (parameter is IList items)
            {
                SelectedOrders = new ObservableCollection<OrderViewModel>(items.OfType<OrderViewModel>().ToList());
            }
        }

        private Machine _selectedMachine;
        public Machine SelectedMachine
        {
            get => _selectedMachine;
            set
            {
                _selectedMachine = value;
                OnPropertyChanged(nameof(SelectedMachine));
            }
        }

        private Driver _selectedDriver;
        public Driver SelectedDriver
        {
            get => _selectedDriver;
            set
            {
                _selectedDriver = value;
                OnPropertyChanged(nameof(SelectedDriver));
            }
        }

        private Address _selectedAddressStart;
        public Address SelectedAddressStart
        {
            get => _selectedAddressStart;
            set
            {
                _selectedAddressStart = value;
                OnPropertyChanged(nameof(SelectedAddressStart));
            }
        }

        private Address _selectedAddressEnd;
        public Address SelectedAddressEnd
        {
            get => _selectedAddressEnd;
            set
            {
                _selectedAddressEnd = value;
                OnPropertyChanged(nameof(SelectedAddressEnd));
            }
        }

        private ObservableCollection<OrderViewModel> _selectedOrders;
        public ObservableCollection<OrderViewModel> SelectedOrders
        {
            get => _selectedOrders;
            set
            {
                _selectedOrders = value;
                OnPropertyChanged(nameof(SelectedOrders));
            }
        }
    }
}