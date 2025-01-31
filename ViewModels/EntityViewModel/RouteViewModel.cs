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
        public readonly int ID;
        public readonly int? MachineID;
        public readonly int? DriverID;

        public RouteViewModel(Route route, ServicesStore servicesStore)
        {
            _model = route;
            _servicesStore = servicesStore;

            ID = _model.ID;
            MachineID = _model.MachineID;
            DriverID = _model.DriverID;

            UpdateData();
        }

        public Route GetModel() => _model;


        [DisplayName("Название машины")]
        public string MachineName { get; set; }
        [DisplayName("ФИО водителя")]
        public string DriverName { get; set; }
        [DisplayName("Тип")]
        public string Type => GetEnumDescription(_model.Type);
        [DisplayName("Дистанция")]
        public string Distance => _model.Distance != null ? ((float)_model.Distance).ToString() : "-";
        [DisplayName("Статус")]
        public string Status => GetEnumDescription(_model.Status);
        [DisplayName("Время выполнения")]
        public string CompleteTime => _model.CompleteTime != null ? ((DateTime)_model.CompleteTime).ToString("d") : "-";

        private async void UpdateData()
        {
            if (MachineID != null)
                MachineName = (await _servicesStore._machineService.GetItemAsync((int)MachineID)).Name;
            else
                MachineName = "Не указана";

            if (DriverID != null)
                DriverName = (await _servicesStore._driverService.GetItemAsync((int)DriverID)).FIO;
            else
                DriverName = "Не указана";
        }
    }
}