namespace CourseProgram.Models
{
    public class Route
    {
        public Route()
        {
            ID = 0;
            Machine = new Machine();
            Driver = new Driver();
            Type = string.Empty;
            Distance = float.NaN;
            Status = string.Empty;
        }

        public Route(int id, Machine machine, Driver driver, string type, float distance, string status)
        {
            ID = id;
            Machine = machine;
            Driver = driver;
            Type = type;
            Distance = distance;
            Status = status;
        }

        public int ID { get; } //КодМаршрута
        public Machine Machine { get; } //КодМашины
        public Driver Driver { get; } //КодВодителя
        public string Type { get; } //Тип
        public float Distance { get; } //Расстояние
        public string Status { get; } //Статус

        public static string GetSelectors() => "\"КодМашины\", \"КодВодителя\", \"Тип\", \"Расстояние\", \"Статус\"";

        public static string GetTable() => "\"Маршрут\"";

        public static string GetSelectorID() => "\"КодМаршрута\"";
    }
}