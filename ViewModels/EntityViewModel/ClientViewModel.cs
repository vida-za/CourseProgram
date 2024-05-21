using CourseProgram.Models;
using System.ComponentModel;

namespace CourseProgram.ViewModels.EntityViewModel
{
    public class ClientViewModel : BaseViewModel
    {
        private readonly Client _model;

        public Client GetModel() => _model;

        [DisplayName("Номер заказчика")] 
        public int ID => _model.ID;
        [DisplayName("Название")]
        public string Name => _model.Name;
        [DisplayName("Тип")]
        public string Type => Constants.GetEnumDescription(_model.Type);
        [DisplayName("ИНН")]
        public string INN => _model.INN;
        [DisplayName("КПП")]
        public string KPP => _model.KPP;
        [DisplayName("ОГРН")]
        public string OGRN => _model.OGRN;
        [DisplayName("Телефон")]
        public string Phone => _model.Phone;
        [DisplayName("Расчётный счёт")]
        public string Checking => _model.Checking;
        [DisplayName("БИК")]
        public string BIK => _model.BIK;
        [DisplayName("Кор.счёт")]
        public string Correspondent => _model.Correspondent;
        [DisplayName("Банк")]
        public string Bank => _model.Bank;
        [DisplayName("Контакт по загрузке")]
        public string PhoneLoad => _model.PhoneLoad;
        [DisplayName("Контакт по разгрузке")]
        public string PhoneOnLoad => _model.PhoneOnLoad;

        public ClientViewModel(Client client)
        {
            _model = client;
        }
    }
}