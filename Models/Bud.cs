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

        public Bud() 
        {
            ID = 0;
            ClientID = 0;
            WorkerID = 0;
            TimeBud = DateTime.MinValue;
            Status = BudStatusValues.Waiting;
            Description = string.Empty;
        }

        public Bud(int id, int clientID, int workerID, DateTime timeBud, string status, string? description)
        {
            ID = id;
            ClientID = clientID;
            WorkerID = workerID;
            TimeBud = timeBud;
            Description = description ?? null;

            Status = status switch
            {
                "В ожидании" => BudStatusValues.Waiting,
                "Принята" => BudStatusValues.Accepted,
                "Отклонена" => BudStatusValues.Cancelled,
                _ => throw new NotImplementedException()
            };
        }

        public Bud(int iD, int clientID, int workerID, DateTime timeBud, BudStatusValues status, string? description)
        {
            ID = iD;
            ClientID = clientID;
            WorkerID = workerID;
            TimeBud = timeBud;
            Status = status;
            Description = description;
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
                "Описание"
            };
        }

        public bool Equals(Bud? other) => other != null && ID == other.ID;

        public override bool Equals(object obj) => Equals(obj as Bud);

        public override int GetHashCode() => HashCode.Combine(ID, TimeBud);
    }
}