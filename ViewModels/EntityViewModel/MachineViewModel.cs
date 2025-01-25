using CourseProgram.Models;
using System;
using System.ComponentModel;

namespace CourseProgram.ViewModels.EntityViewModel
{
    public class MachineViewModel : BaseViewModel
    {
        private readonly Machine _model;

        public Machine GetModel() => _model;

        [DisplayName("Номер машины")]
        public int ID => _model.ID;
        [DisplayName("Тип машины")]
        public string TypeMachine => Constants.GetEnumDescription(_model.TypeMachine);
        [DisplayName("Тип кузова")]
        public string TypeBodywork => Constants.GetEnumDescription(_model.TypeBodywork);
        [DisplayName("Тип загрузки")]
        public string TypeLoading => Constants.GetEnumDescription(_model.TypeLoading);
        [DisplayName("Грузоподъёмность")]
        public float LoadCapacity => _model.LoadCapacity;
        [DisplayName("Объём")]
        public float Volume => _model.Volume;
        [DisplayName("Гидроборт")]
        public bool HydroBoard => _model.HydroBoard;
        [DisplayName("Длина кузова")]
        public float LengthBodywork => _model.LengthBodywork;
        [DisplayName("Ширина кузова")]
        public float WidthBodywork => _model.WidthBodywork;
        [DisplayName("Высота кузова")]
        public float HeightBodywork => _model.HeightBodywork;
        [DisplayName("Марка")]
        public string Stamp => _model.Stamp;
        [DisplayName("Название")]
        public string Name => _model.Name;
        [DisplayName("Гос.номер")]
        public string StateNumber => _model.StateNumber;
        [DisplayName("Статус")]
        public string Status => Constants.GetEnumDescription(_model.Status);
        [DisplayName("Время поступления")]
        public string TimeStart => _model.TimeStart.ToString("d");
        [DisplayName("Время списания")]
        public string TimeEnd => _model.TimeEnd == DateTime.MinValue ? "-" : _model.TimeEnd.ToString("d");
        [DisplayName("Текущая дислокация")]
        public string Town => _model.Town;

        public MachineViewModel(Machine model)
        {
            _model = model;
        }
    }
}