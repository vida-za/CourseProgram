using CourseProgram.Models;
using CourseProgram.Stores;
using System;

namespace CourseProgram.ViewModels
{
    public class RouteViewModel : BaseViewModel
    {
        private readonly ServicesStore _servicesStore;
        private readonly Route _model;
        public RouteViewModel(Route route, ServicesStore servicesStore) 
        {
            _model = route;
            _servicesStore = servicesStore;

            UpdateData();
        }

        public Route GetModel() => _model;

        public int ID => _model.ID;
        public int MachineID => _model.MachineID;
        public string MachineName;
        public int DriverID => _model.DriverID;
        public string DriverName;
        public string Type => _model.Type;
        public float Distance => _model.Distance;
        public string Status => _model.Status;
        public DateTime CompleteTime => _model.CompleteTime;

        private async void UpdateData()
        {
            Machine tempMach = await _servicesStore._machineService.GetItemAsync(MachineID);
            MachineName = tempMach.Name;
            Driver tempDrv = await _servicesStore._driverService.GetItemAsync(DriverID);
            DriverName = tempDrv.FIO;
        }
    }
}