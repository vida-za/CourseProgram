using System;
using System.ComponentModel;
using static CourseProgram.Models.Constants;

namespace CourseProgram.Models
{
    public class Route : IModel, IEquatable<Route>
    {
        [DisplayName("Номер маршрута")]
        public int ID { get; } //КодМаршрута
        [DisplayName("Номер машины")]
        public int MachineID { get; } //КодМашины
        [DisplayName("Номер водителя")]
        public int DriverID { get; } //КодВодителя
        [DisplayName("Тип")]
        public TypeRouteValues Type { get; } //Тип
        [DisplayName("Расстояние")]
        public float Distance { get; } //Расстояние
        [DisplayName("Статус")]
        public StatusRouteValues Status { get; } //Статус
        [DisplayName("Время выполнения")]
        public DateTime CompleteTime { get; } //ВремяВыполнения

        public Route()
        {
            ID = 0;
            MachineID = 0;
            DriverID = 0;
            Type = TypeRouteValues.Null;
            Distance = float.NaN;
            Status = StatusRouteValues.Null;
            CompleteTime = DateTime.MinValue;
        }

        public Route(int id, int machine, int driver, string type, float distance, string status, DateTime completeTime)
        {
            ID = id;
            MachineID = machine;
            DriverID = driver;
            Distance = distance;
            CompleteTime = completeTime;

            Type = type switch
            {
                "Общий" => TypeRouteValues.General,
                "Обособленный" => TypeRouteValues.Isolated,
                "Пустой" => TypeRouteValues.Empty,
                _ => TypeRouteValues.Null,
            };
            Status = status switch
            {
                "Выполнен" => StatusRouteValues.Completed,
                "В ожидании" => StatusRouteValues.Waiting,
                "Выполняется" => StatusRouteValues.InProgress,
                "Отменен" => StatusRouteValues.Cancelled,
                _ => StatusRouteValues.Null,
            };
        }

        public string GetSelectors() => "\"КодМаршрута\", \"КодМашины\", \"КодВодителя\", \"Тип\", \"Расстояние\", \"Статус\"";
        public string GetTable() => "\"Маршрут\"";
        public string GetSelectorID() => "\"КодМаршрута\"";
        public string GetProcedureDelete() => "\"DeleteRoute\"";


        public bool Equals(Route? other)
        {
            if (other != null)
                return ID == other.ID;
            else return false;
        }

        public override bool Equals(object obj) => Equals(obj as Route);

        public override int GetHashCode() => HashCode.Combine(ID, MachineID);
    }
}