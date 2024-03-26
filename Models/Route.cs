using System;

namespace CourseProgram.Models
{
    public class Route
    {
        public Route()
        {
            ID = 0;
            MachineID = 0;
            DriverID = 0;
            Type = string.Empty;
            Distance = float.NaN;
            Status = string.Empty;
            CompleteTime = DateTime.MinValue;
        }

        public Route(int id, int machine, int driver, string type, float distance, string status, DateTime completeTime)
        {
            ID = id;
            MachineID = machine;
            DriverID = driver;
            Type = type;
            Distance = distance;
            Status = status;
            CompleteTime = completeTime;
        }

        public int ID { get; } //КодМаршрута
        public int MachineID { get; } //КодМашины
        public int DriverID { get; } //КодВодителя
        public string Type { get; } //Тип
        public float Distance { get; } //Расстояние
        public string Status { get; } //Статус
        public DateTime CompleteTime { get; }

        public static string GetSelectors() => "\"КодМашины\", \"КодВодителя\", \"Тип\", \"Расстояние\", \"Статус\"";

        public static string GetTable() => "\"Маршрут\"";

        public static string GetSelectorID() => "\"КодМаршрута\"";
    }
}