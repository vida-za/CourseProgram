using CourseProgram.Models;
using System;
using System.ComponentModel;
using static CourseProgram.Models.Constants;

namespace CourseProgram.ViewModels.EntityViewModel
{
    public class RouteViewModel : BaseViewModel
    {
        private readonly Route _model;
        private readonly Machine? _machineModel;
        private readonly Driver? _driverModel;
        private readonly Address? _addressStartModel;
        private readonly Address? _addressEndModel;

        public readonly int ID;

        public RouteViewModel(Route route, Machine? machine, Driver? driver, Address? addressStart, Address? addressEnd)
        {
            _model = route;
            _machineModel = machine;
            _driverModel = driver;
            _addressStartModel = addressStart;
            _addressEndModel = addressEnd;

            ID = _model.ID;
        }

        public Route GetModel() => _model;

        public Machine? GetMachine() => _machineModel;

        public Driver? GetDriver() => _driverModel;

        public Address? GetAddressStart() => _addressStartModel;

        public Address? GetAddressEnd() => _addressEndModel;

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
        public string CompleteTime => _model.CompleteTime != null ? ((DateTime)_model.CompleteTime).ToString("f") : "-";
    }
}