﻿using CourseProgram.Models;
using CourseProgram.Stores;
using System;
using System.ComponentModel;

namespace CourseProgram.ViewModels.EntityViewModel
{
    public class MachineViewModel : BaseViewModel
    {
        private readonly Machine _model;
        private readonly ControllersStore _controllersStore;

        private Address _modelAddress;
        public readonly int ID;
        public readonly int? AddressID;

        public Machine GetModel() => _model;

        [DisplayName("Тип машины")]
        public string TypeMachine => Constants.GetEnumDescription(_model.TypeMachine);
        [DisplayName("Тип кузова")]
        public string TypeBodywork => Constants.GetEnumDescription(_model.TypeBodywork);
        [DisplayName("Тип загрузки")]
        public string TypeLoading => Constants.GetEnumDescription(_model.TypeLoading);
        [DisplayName("Грузоподъёмность")]
        public string LoadCapacity => _model.LoadCapacity.ToString();
        [DisplayName("Объём")]
        public string Volume => _model.Volume != null ? ((float)_model.Volume).ToString() : "-";
        [DisplayName("Гидроборт")]
        public string HydroBoard => _model.HydroBoard ? "Да" : "Нет";
        [DisplayName("Длина кузова")]
        public string LengthBodywork => _model.LengthBodywork != null ? ((float)_model.LengthBodywork).ToString() : "-";
        [DisplayName("Ширина кузова")]
        public string WidthBodywork => _model.WidthBodywork != null ? ((float)_model.WidthBodywork).ToString() : "-";
        [DisplayName("Высота кузова")]
        public string HeightBodywork => _model.HeightBodywork != null ? ((float)_model.HeightBodywork).ToString() : "-";
        [DisplayName("Марка")]
        public string Stamp => _model.Stamp;
        [DisplayName("Название")]
        public string Name => _model.Name;
        [DisplayName("Гос.номер")]
        public string StateNumber => _model.StateNumber;
        [DisplayName("Статус")]
        public string Status => Constants.GetEnumDescription(_model.Status);
        [DisplayName("Время поступления")]
        public string TimeStart => _model.TimeStart.ToString("g");
        [DisplayName("Время списания")]
        public string TimeEnd => _model.TimeEnd != null ? ((DateTime)_model.TimeEnd).ToString("g") : "-";
        private string? _fullAddress;
        [DisplayName("Текущая дислокация")]
        public string FullAddress
        {
            get => _fullAddress ?? "Неизвестно";
            set
            {
                if (_fullAddress != value)
                {
                    _fullAddress = value;
                    OnPropertyChanged(nameof(FullAddress));
                }
            }
        }
        [DisplayName("Категория")]
        public string CategoryName => _model.CategoryValue.Name;

        public MachineViewModel(Machine model, ControllersStore controllersStore)
        {
            _model = model;
            _controllersStore = controllersStore;

            ID = _model.ID;
            AddressID = _model.AddressID;

            UpdateData();
        }

        private async void UpdateData()
        {
            _modelAddress = AddressID != null ? await _controllersStore.GetController<Address>().GetItemByID((int)AddressID) : null;

            FullAddress = _modelAddress?.FullAddress;
        }
    }
}