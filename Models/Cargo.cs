using System;
using System.ComponentModel;

namespace CourseProgram.Models
{
    public class Cargo : IModel, IEquatable<Cargo>
    {
        [DisplayName("КодГруза")]
        public int ID { get; }
        [DisplayName("КодЗаявки")]
        public int BudID { get; }
        [DisplayName("КодНоменклатуры")]
        public int NomenclatureID { get; }
        [DisplayName("Объём")]
        public float? Volume { get; }
        [DisplayName("Вес")]
        public float Weight { get; }
        [DisplayName("Количество")]
        public int Count { get; }

        public Cargo()
        {
            ID = 0;
            BudID = 0;
            NomenclatureID = 0;
            Volume = null;
            Weight = 0;
            Count = 0;
        }

        public Cargo(
            int id,
            int budID,
            int nomenclatureID,
            float? volume,
            float weight,
            int count)
        {
            ID = id;
            BudID = budID;
            NomenclatureID = nomenclatureID;
            Volume = volume;
            Weight = weight;
            Count = count;
        }

        public static string GetTable() => "Груз";
        public static string GetSelectorID() => "КодГруза";
        public static string[] GetFieldNames()
        {
            return new[]
            {
                "КодГруза",
                "КодЗаявки",
                "КодНоменклатуры",
                "Объём",
                "Вес",
                "Количество"
            };
        }

        public bool Equals(Cargo? other) => other != null && ID == other.ID;

        public override bool Equals(object obj) => Equals(obj as Cargo);

        public override int GetHashCode() => HashCode.Combine(ID, BudID);
    }
}