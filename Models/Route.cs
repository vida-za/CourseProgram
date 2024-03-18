namespace CourseProgram.Models
{
    public class Route
    {
        public Route()
        {
            Machine = new Machine();
            Driver = new Driver();
            Type = string.Empty;
            Distance = float.NaN;
            Status = string.Empty;
        }

        public Route(/*int id*/, Machine machine, Driver driver, string type, float distance, string status)
        {
            //Id = id;
            Machine = machine;
            Driver = driver;
            Type = type;
            Distance = distance;
            Status = status;
        }

        //public int Id { get; } //КодМаршрута
        public Machine Machine { get; } //КодМашины
        public Driver Driver { get; } //КодВодителя
        public string Type { get; } //Тип
        public float Distance { get; } //Расстояние
        public string Status { get; } //Статус

        public string GetSelectors() => " \"КодМаршрута\", \"КодМашины\", \"КодВодителя\", \"Тип\", \"Расстояние\", \"Статус\"";

        public string GetTable() => " From \"Маршрут\";";
    }
}