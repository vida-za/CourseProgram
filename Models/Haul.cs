using System;
using System.ComponentModel;

namespace CourseProgram.Models
{
    public class Haul : IModel, IEquatable<Haul>
    {
        [DisplayName("Номер рейса")]
        public int ID { get; } //КодРейса
        [DisplayName("Дата начала")]
        public DateOnly DateStart { get; } //ДатаНачала
        [DisplayName("Дата окончания")]
        public DateOnly DateEnd { get; } //ДатаОкончания
        [DisplayName("Суммарный доход")]
        public float SumIncome { get; } //СуммарныйДоход

        public Haul()
        {
            ID = 0;
            DateStart = new DateOnly();
            DateEnd = new DateOnly();
            SumIncome = 0;
        }

        public Haul(int id, DateOnly dateStart, DateOnly dateEnd, float sumIncome)
        {
            ID = id;
            DateStart = dateStart;
            DateEnd = dateEnd;
            SumIncome = sumIncome;
        }

        public string GetSelectors() => "\"КодРейса\", \"ДатаНачала\", \"ДатаОкончания\", \"СуммарныйДоход\"";
        public string GetTable() => "\"Рейс\"";
        public string GetSelectorID() => "\"КодРейса\"";
        public string GetProcedureDelete() => "\"DeleteHaul\"";

        public bool Equals(Haul? other)
        {
            if (other != null)
                return ID == other.ID;
            else return false;
        }

        public override bool Equals(object obj) => Equals(obj as Haul);

        public override int GetHashCode() => HashCode.Combine(ID, DateStart);
    }
}