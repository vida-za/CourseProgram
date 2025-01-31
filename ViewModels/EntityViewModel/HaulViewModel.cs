using CourseProgram.Models;
using System.ComponentModel;

namespace CourseProgram.ViewModels.EntityViewModel
{
    public class HaulViewModel : BaseViewModel
    {
        private readonly Haul _model;
        public readonly int ID;

        public Haul GetModel() => _model;

        [DisplayName("Дата начала")]
        public string DateStart => _model.DateStart.ToString();
        [DisplayName("Дата окончания")]
        public string DateEnd => _model.DateEnd.ToString();
        [DisplayName("Доход")]
        public string SunIncome => _model.SumIncome.ToString();

        public HaulViewModel(Haul haul)
        {
            _model = haul;

            ID = _model.ID;
        }
    }
}