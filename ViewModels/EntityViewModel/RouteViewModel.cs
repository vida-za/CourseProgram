using CourseProgram.Models;
using CourseProgram.Stores;
using System;
using System.ComponentModel;
using static CourseProgram.Models.Constants;

namespace CourseProgram.ViewModels.EntityViewModel
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

        [DisplayName("Номер маршрута")]
        public int ID => _model.ID;
        [DisplayName("Номер машины")]
        public int MachineID => _model.MachineID;
        [DisplayName("Название машины")]
        public string MachineName { get; set; }
        [DisplayName("Номер водителя")]
        public int DriverID => _model.DriverID;
        [DisplayName("ФИО водителя")]
        public string DriverName { get; set; }
        [DisplayName("Тип")]
        public string Type => GetEnumDescription(_model.Type);
        [DisplayName("Дистанция")]
        public float Distance => _model.Distance;
        [DisplayName("Статус")]
        public string Status => GetEnumDescription(_model.Status);
        [DisplayName("Время выполнения")]
        public DateTime CompleteTime => _model.CompleteTime;

        private async void UpdateData()
        {
            MachineName = (await _servicesStore._machineService.GetItemAsync(MachineID)).Name;
            DriverName = (await _servicesStore._driverService.GetItemAsync(DriverID)).FIO;
        }
    }
}