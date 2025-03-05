using CourseProgram.Models;
using CourseProgram.Stores;
using System;
using System.ComponentModel;
using static CourseProgram.Models.Constants;

namespace CourseProgram.ViewModels.EntityViewModel
{
    public class RouteViewModel : BaseViewModel
    {
        private readonly Route _model;
        private readonly ControllersStore _controllersStore;

        private Machine? _machineModel;
        private Driver? _driverModel;
        private Address? _addressStartModel;
        private Address? _addressEndModel;

        private string _machineName;
        private string _driverName;
        private string _addressStart;
        private string _addressEnd;

        public readonly int ID;

        public RouteViewModel(Route route, ControllersStore controllersStore)
        {
            _model = route;
            _controllersStore = controllersStore;

            ID = _model.ID;

            UpdateData();
        }

        private async void UpdateData()
        {
            if (_model.MachineID != null)
            {
                _machineModel = await _controllersStore.GetController<Machine>().GetItemByID((int)_model.MachineID);
                MachineName = _machineModel.Name;
            }
            else
                MachineName = "Не указано";

            if (_model.DriverID != null)
            {
                _driverModel = await _controllersStore.GetController<Driver>().GetItemByID((int)_model.DriverID);
                DriverName = _driverModel.FIO;
            }
            else
                DriverName = "Не указано";

            if (_model.AddressStartID != null)
            {
                _addressStartModel = await _controllersStore.GetController<Address>().GetItemByID((int)_model.AddressStartID);
                AddressStart = _addressStartModel.FullAddress;
            }
            else
                AddressStart = "Не указано";

            if (_model.AddressEndID != null)
            {
                _addressEndModel = await _controllersStore.GetController<Address>().GetItemByID((int)_model.AddressEndID);
                AddressEnd = _addressEndModel.FullAddress;
            }
            else
                AddressEnd = "Не указано";
        }

        public Route GetModel() => _model;

        public Machine? GetMachine() => _machineModel;

        public Driver? GetDriver() => _driverModel;

        public Address? GetAddressStart() => _addressStartModel;

        public Address? GetAddressEnd() => _addressEndModel;

        [DisplayName("Название машины")]
        public string MachineName
        {
            get => _machineName;
            set
            {
                _machineName = value;
                OnPropertyChanged(nameof(MachineName));
            }
        }
        [DisplayName("ФИО водителя")]
        public string DriverName
        {
            get => _driverName;
            set
            {
                _driverName = value;
                OnPropertyChanged(nameof(DriverName));
            }
        }
        [DisplayName("Начало маршрута")]
        public string AddressStart
        {
            get => _addressStart;
            set
            {
                _addressStart = value;
                OnPropertyChanged(nameof(AddressStart));
            }
        }
        [DisplayName("Конец маршрута")]
        public string AddressEnd
        {
            get => _addressEnd;
            set
            {
                _addressEnd = value;
                OnPropertyChanged(nameof(AddressEnd));
            }
        }
        [DisplayName("Тип")]
        public string Type => GetEnumDescription(_model.Type);
        [DisplayName("Статус")]
        public string Status => GetEnumDescription(_model.Status);
        [DisplayName("Время выполнения")]
        public string CompleteTime => _model.CompleteTime != null ? ((DateTime)_model.CompleteTime).ToString("f") : "-";
    }
}