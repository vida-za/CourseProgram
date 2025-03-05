using System;
using System.ComponentModel;
using static CourseProgram.Models.Constants;

namespace CourseProgram.Models
{
    public class Bud : IModel, IEquatable<Bud>
    {
        [DisplayName("КодЗаявки")]
        public int ID { get; }
        [DisplayName("КодЗаказчика")]
        public int ClientID { get; }
        [DisplayName("КодСотрудника")]
        public int WorkerID { get; }
        [DisplayName("ДатаВремяЗаявки")]
        public DateTime TimeBud { get; }
        [DisplayName("Статус")]
        public BudStatusValues Status { get; }
        [DisplayName("Описание")]
        public string? Description { get; }
        [DisplayName("КодАдресаПогрузки")]
        public int AddressLoadID { get; }
        [DisplayName("КодАдресаРазгрузки")]
        public int AddressOnLoadID { get; }
        [DisplayName("ДатаВремяПогрузкиПлан")]
        public DateTime DateTimeLoadPlan { get; }
        [DisplayName("ДатаВремяРазгрузкиПлан")]
        public DateTime DateTimeOnLoadPlan { get; }

        public Bud() 
        {
            ID = 0;
            ClientID = 0;
            WorkerID = 0;
            TimeBud = DateTime.Now;
            Status = BudStatusValues.Waiting;
            Description = null;
            AddressLoadID = 0;
            AddressOnLoadID = 0;
            DateTimeLoadPlan = DateTime.Now;
            DateTimeOnLoadPlan = DateTime.Now;
        }

        public Bud(int id, int clientID, int workerID, DateTime timeBud, string status, string? description, int addressLoadID, int addressOnLoadID, DateTime dateTimeLoadPlan, DateTime dateTimeOnLoadPlan)
        {
            ID = id;
            ClientID = clientID;
            WorkerID = workerID;
            TimeBud = timeBud;
            Description = description ?? null;
            AddressLoadID = addressLoadID;
            AddressOnLoadID = addressOnLoadID;
            DateTimeLoadPlan = dateTimeLoadPlan;
            DateTimeOnLoadPlan = dateTimeOnLoadPlan;

            Status = status switch
            {
                "В ожидании" => BudStatusValues.Waiting,
                "Принята" => BudStatusValues.Accepted,
                "Отклонена" => BudStatusValues.Cancelled,
                _ => throw new NotImplementedException()
            };
        }

        public Bud(int iD, int clientID, int workerID, DateTime timeBud, BudStatusValues status, string? description, int addressLoadID, int addressOnLoadID, DateTime dateTimeLoadPlan, DateTime dateTimeOnLoadPlan)
        {
            ID = iD;
            ClientID = clientID;
            WorkerID = workerID;
            TimeBud = timeBud;
            Status = status;
            Description = description;
            AddressLoadID = addressLoadID;
            AddressOnLoadID = addressOnLoadID;
            DateTimeLoadPlan = dateTimeLoadPlan;
            DateTimeOnLoadPlan = dateTimeOnLoadPlan;
        }

        public static string GetTable() => "Заявка";
        public static string GetSelectorID() => "КодЗаявки";
        public static string[] GetFieldNames()
        {
            return new[]
            {
                "КодЗаявки",
                "КодЗаказчика",
                "КодСотрудника",
                "ДатаВремяЗаявки",
                "Статус",
                "Описание",
                "КодАдресаПогрузки",
                "КодАдресаРазгрузки",
                "ДатаВремяПогрузкиПлан",
                "ДатаВремяРазгрузкиПлан"
            };
        }

        public bool Equals(Bud? other) => other != null && ID == other.ID;

        public override bool Equals(object obj) => Equals(obj as Bud);

        public override int GetHashCode() => HashCode.Combine(ID, TimeBud);
    }
}