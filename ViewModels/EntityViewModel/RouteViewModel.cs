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
        private Machine _machineModel;
        private Driver _driverModel;
        private Address _addressStartModel;
        private Address _addressEndModel;

        public readonly int ID;
        public readonly int? MachineID;
        public readonly int? DriverID;
        public readonly int? AddressStartID;
        public readonly int? AddressEndID;

        public RouteViewModel(Route route, ServicesStore servicesStore)
        {
            _model = route;
            _servicesStore = servicesStore;

            ID = _model.ID;
            MachineID = _model.MachineID;
            DriverID = _model.DriverID;
            AddressStartID = _model.AddressStartID;
            AddressEndID = _model.AddressEndID;

            UpdateData();
        }

        public Route GetModel() => _model;


        [DisplayName("Название машины")]
        public string MachineName => _machineModel?.Name ?? "Не указано";
        [DisplayName("ФИО водителя")]
        public string DriverName => _driverModel?.FIO ?? "Не указано";
        [DisplayName("Начало маршрута")]
        public string AddressStart => _addressStartModel?.FullAddress ?? "Не указано";
        [DisplayName("Конец маршрута")]
        public string AddressEnd => _addressEndModel?.FullAddress ?? "Не указано";
        [DisplayName("Тип")]
        public string Type => GetEnumDescription(_model.Type);
        [DisplayName("Статус")]
        public string Status => GetEnumDescription(_model.Status);
        [DisplayName("Время выполнения")]
        public string CompleteTime => _model.CompleteTime != null ? ((DateTime)_model.CompleteTime).ToString("d") : "-";

        private async void UpdateData()
        {
            _machineModel = MachineID != null ? await _servicesStore._machineService.GetItemAsync((int)MachineID) : null;
            _driverModel = DriverID != null ? await _servicesStore._driverService.GetItemAsync((int)DriverID) : null;
            _addressStartModel = AddressStartID != null ? await _servicesStore._addressService.GetItemAsync((int)AddressStartID) : null;
            _addressEndModel = AddressEndID != null ? await _servicesStore._addressService.GetItemAsync((int)AddressEndID) : null;
        }
    }
}