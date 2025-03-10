﻿using CourseProgram.Commands;
using CourseProgram.Controllers.DataControllers.EntityDataControllers;
using CourseProgram.Models;
using CourseProgram.Services;
using CourseProgram.Stores;
using CourseProgram.ViewModels.EntityViewModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace CourseProgram.ViewModels.DetailViewModel
{
    public class MachineDetailViewModel : BaseDetailViewModel
    {
        private readonly MachineViewModel _machineViewModel;
        private readonly ControllersStore _controllersStore;

        private readonly ObservableCollection<RouteViewModel> _routes = new ObservableCollection<RouteViewModel>();
        public IEnumerable<RouteViewModel> Routes => _routes;

        public ICommand BackCommand { get; }

        public MachineDetailViewModel(SelectedStore selectedStore, ControllersStore controllersStore, INavigationService closeNavigationService)
        {
            _machineViewModel = selectedStore.CurrentMachine;
            _controllersStore = controllersStore;

            BackCommand = new NavigateCommand(closeNavigationService);

            UpdateData();
        }

        private async void UpdateData()
        {
            _routes.Clear();

            IEnumerable<Route> temp = await ((RouteDataController)_controllersStore.GetController<Route>()).GetRoutesByMachine(ID);

            foreach (Route route in temp)
            {
                var routeViewModel = new RouteViewModel(route, _controllersStore);
                _routes.Add(routeViewModel);
            }
        }

        public int ID => _machineViewModel.ID;
        public string TypeMachine => _machineViewModel.TypeMachine;
        public string TypeBodywork => _machineViewModel.TypeBodywork;
        public string TypeLoading => _machineViewModel.TypeLoading;
        public string LoadCapacity => _machineViewModel.LoadCapacity.ToString();
        public string Volume => _machineViewModel.Volume.ToString();
        public string HydroBoard => _machineViewModel.HydroBoard;
        public string LengthBodywork => _machineViewModel.LengthBodywork.ToString();
        public string WidthBodywork => _machineViewModel.WidthBodywork.ToString();
        public string HeightBodywork => _machineViewModel.HeightBodywork.ToString();
        public string Stamp => _machineViewModel.Stamp;
        public string Name => _machineViewModel.Name;
        public string StateNumber => _machineViewModel.StateNumber;
        public string Status => _machineViewModel.Status;
        public string TimeStart => _machineViewModel.TimeStart.ToString();
        public string TimeEnd => _machineViewModel.TimeEnd;
        public string FullAddress => _machineViewModel.FullAddress;
        public string CategoryName => _machineViewModel.CategoryName;
    }
}