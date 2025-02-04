using System;
using System.ComponentModel;

namespace CourseProgram.Models
{
    public class Haul : IModel, IEquatable<Haul>
    {
        [DisplayName("КодРейса")]
        public int ID { get; }
        [DisplayName("ДатаНачала")]
        public DateOnly DateStart { get; }
        [DisplayName("ДатаОкончания")]
        public DateOnly? DateEnd { get; }
        [DisplayName("СуммарныйДоход")]
        public float? SumIncome { get; }

        public Haul()
        {
            ID = 0;
            DateStart = new DateOnly();
            DateEnd = null;
            SumIncome = null;
        }

        public Haul(int id, DateOnly dateStart, DateOnly? dateEnd, float? sumIncome)
        {
            ID = id;
            DateStart = dateStart;
            DateEnd = dateEnd;
            SumIncome = sumIncome;
        }

        public static string GetTable() => "Рейс";
        public static string GetSelectorID() => "КодРейса";
        public static string[] GetFieldNames()
        {
            return new[]
            {
                "КодРейса",
                "ДатаНачала",
                "ДатаОкончания",
                "СуммарныйДоход"
            };
        }

        public bool Equals(Haul? other) => other != null && ID == other.ID;

        public override bool Equals(object obj) => Equals(obj as Haul);

        public override int GetHashCode() => HashCode.Combine(ID, DateStart);
    }
}