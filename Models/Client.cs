using System;
using System.ComponentModel;
using static CourseProgram.Models.Constants;

namespace CourseProgram.Models
{
    public class Client : IModel, IEquatable<Client>
    {
        [DisplayName("КодЗаказчика")]
        public int ID { get; }
        [DisplayName("Название")]
        public string Name { get; }
        [DisplayName("ТипЗаказчика")]
        public ClientTypeValues Type { get; }
        [DisplayName("ИНН")]
        public string INN { get; }
        [DisplayName("КПП")]
        public string KPP { get; }
        [DisplayName("ОГРН")]
        public string OGRN { get; }
        [DisplayName("Телефон")]
        public string Phone { get; }
        [DisplayName("РасчётныйСчёт")]
        public string Checking { get; }
        [DisplayName("БИК")]
        public string? BIK { get; }
        [DisplayName("КорреспондентскийСчёт")]
        public string? Correspondent { get; }
        [DisplayName("Банк")]
        public string? Bank { get; }

        public Client()
        {
            ID = 0;
            Name = string.Empty;
            Type = ClientTypeValues.Physical;
            INN = string.Empty;
            KPP = string.Empty;
            OGRN = string.Empty;
            Phone = string.Empty;
            Checking = string.Empty;
            BIK = null;
            Correspondent = null;
            Bank = null;
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
            string? bik,
            string? correspondent,
            string? bank)
        {
            ID = id;
            Name = name;
            INN = inn;
            KPP = kpp;
            OGRN = ogrn;
            Phone = phone;
            Checking = checking;
            BIK = bik;
            Correspondent = correspondent;
            Bank = bank;

            Type = type switch
            {
                "Физлицо" => ClientTypeValues.Physical,
                "Юрлицо" => ClientTypeValues.Legal,
                _ => throw new NotImplementedException()
            };
        }

        public Client(int iD, string name, ClientTypeValues type, string iNN, string kPP, string oGRN, string phone, string checking, string? bIK, string? correspondent, string? bank)
        {
            ID = iD;
            Name = name;
            Type = type;
            INN = iNN;
            KPP = kPP;
            OGRN = oGRN;
            Phone = phone;
            Checking = checking;
            BIK = bIK;
            Correspondent = correspondent;
            Bank = bank;
        }

        public static string GetTable() => "Заказчик";
        public static string GetSelectorID() => "КодЗаказчика";
        public static string[] GetFieldNames()
        {
            return new[]
            {
                "КодЗаказчика",
                "Название",
                "ТипЗаказчика",
                "ИНН",
                "КПП",
                "ОГРН",
                "Телефон",
                "РасчётныйСчёт",
                "БИК",
                "КорреспондентскийСчёт",
                "Банк"
            };
        }

        public bool Equals(Client? other) => other != null && ID == other.ID;

        public override bool Equals(object obj) => Equals(obj as Client);

        public override int GetHashCode() => HashCode.Combine(ID, Name);
    }
}