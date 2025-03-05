using CourseProgram.Models;
using System;
using System.ComponentModel;

namespace CourseProgram.ViewModels.EntityViewModel
{
    public class HaulViewModel : BaseViewModel
    {
        private readonly Haul _model;
        public readonly int ID;

        public Haul GetModel() => _model;

        [DisplayName("Дата начала")]
        public string DateStart => _model.DateStart.ToString("d");
        [DisplayName("Дата окончания")]
        public string DateEnd => _model.DateEnd != null ? ((DateOnly)_model.DateEnd).ToString("d") : "-";
        [DisplayName("Доход")]
        public string SunIncome => _model.SumIncome != null ? _model.SumIncome.ToString() : "-";
        [DisplayName("Период")]
        public string Period => _model.DateEnd != null ? $"{DateStart} - {DateEnd}" : $"{DateStart} - ";

        public HaulViewModel(Haul haul)
        {
            _model = haul;

            ID = _model.ID;
        }
    }
}