using System;
using System.ComponentModel;

namespace CourseProgram.Models
{
    public class Client : IModel, IEquatable<Client>
    {
        [DisplayName("Номер заказчика")]
        public int ID { get; } //КодЗаказчика
        [DisplayName("Название")]
        public string Name { get; } //Название
        [DisplayName("Тип заказчика")]
        public string Type { get; } //ТипЗаказчика
        [DisplayName("ИНН")]
        public string INN { get; } //ИНН
        [DisplayName("КПП")]
        public string KPP { get; } //КПП
        [DisplayName("ОГРН")]
        public string OGRN { get; } //ОГРН
        [DisplayName("Телефон")]
        public string Phone { get; } //Телефон
        [DisplayName("Расчётный счёт")]
        public string Checking { get; } //РасчётныйСчёт
        [DisplayName("БИК")]
        public string BIK { get; } //БИК
        [DisplayName("Корреспондентский счёт")]
        public string Correspondent { get; } //КорреспондентскийСчёт
        [DisplayName("Банк")]
        public string Bank { get; } //Банк
        [DisplayName("Контакт загрузки")]
        public string PhoneLoad { get; } //КонтактЗагрузки
        [DisplayName("Контакт выгрузки")]
        public string PhoneOnLoad { get; } //КонтактВыгрузки

        public Client()
        {
            ID = 0;
            Name = string.Empty;
            Type = string.Empty;
            INN = string.Empty;
            KPP = string.Empty;
            OGRN = string.Empty;
            Phone = string.Empty;
            Checking = string.Empty;
            BIK = string.Empty;
            Correspondent = string.Empty;
            Bank = string.Empty;
            PhoneLoad = string.Empty;
            PhoneOnLoad = string.Empty;
        }

        public Client(
            int id,
            string name,
            string type,
            string inn,
            string kpp,
            string ogrn,
            string phone,
            string checking,
            string bik,
            string correspondent,
            string bank,
            string phoneLoad,
            string phoneOnLoad)
        {
            ID = id;
            Name = name;
            Type = type;
            INN = inn;
            KPP = kpp;
            OGRN = ogrn;
            Phone = phone;
            Checking = checking;
            BIK = bik;
            Correspondent = correspondent;
            Bank = bank;
            PhoneLoad = phoneLoad;
            PhoneOnLoad = phoneOnLoad;
        }

        public string GetSelectors() => "\"КодЗаказчика\", \"Название\", \"ТипЗаказчика\", \"ИНН\", \"КПП\", \"ОГРН\", \"Телефон\", \"РасчётныйСчёт\", \"БИК\", \"КорреспондентскийСчёт\", \"Банк\", \"КонтактЗагрузки\", \"КонтактВыгрузки\"";
        public string GetTable() => "\"Заказчик\"";
        public string GetSelectorID() => "\"КодЗаказчика\"";
        public string GetProcedureDelete() => "\"DeleteClient\"";

        public bool Equals(Client? other)
        {
            if (other != null)
                return ID == other.ID;
            else return false;
        }

        public override bool Equals(object obj) => Equals(obj as Client);

        public override int GetHashCode() => HashCode.Combine(ID, Name);
    }
}