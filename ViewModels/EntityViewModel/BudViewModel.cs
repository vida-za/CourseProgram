using CourseProgram.Models;
using CourseProgram.Stores;
using System.ComponentModel;
using static CourseProgram.Models.Constants;

namespace CourseProgram.ViewModels.EntityViewModel
{
    public class BudViewModel : BaseViewModel
    {
        private readonly ServicesStore _servicesStore;
        private readonly Bud _model;

        public readonly int ID;
        public readonly int ClientID;
        public readonly int WorkerID;

        [DisplayName("Время поступления")]
        public string TimeBud => _model.TimeBud.ToString("g");
        [DisplayName("Статус")]
        public string Status => GetEnumDescription(_model.Status);
        [DisplayName("Описание")]
        public string Description => _model.Description ?? "-";

        public BudViewModel(Bud bud, ServicesStore servicesStore)
        {
            _model = bud;
            _servicesStore = servicesStore;

            ID = _model.ID;
            ClientID = _model.ClientID;
            WorkerID = _model.WorkerID;
        }

        public Bud GetModel() => _model;
    }
}