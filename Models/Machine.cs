using System;

namespace CourseProgram.Models
{
    public class Machine
    {
        public Machine()
        {
            //Id = 0;
            TypeMachine = string.Empty;
            TypeBodywork = string.Empty;
            TypeLoading = string.Empty;
            LoadCapacity = float.NaN;
            Volume = float.NaN;
            HydroBoard = false;
            LengthBodywork = float.NaN;
            WidthBodywork = float.NaN;
            HeightBodywork = float.NaN;
            Stamp = string.Empty;
            Name = string.Empty;
            StateNumber = string.Empty;
            Status = string.Empty;
            TimeStart = new DateTime();
            TimeEnd = new DateTime();
        }

        public Machine(/*int id, */string typeMachine, string typeBodywork, string typeLoading, float loadCapacity, float volume, bool hydroBoard, float lengthBodywork, float widthBodywork, float heightBodywork, string stamp, string name, string stateNumber, string status, DateTime timeStart, DateTime timeEnd)
        {
            //Id = id;
            TypeMachine = typeMachine;
            TypeBodywork = typeBodywork;
            TypeLoading = typeLoading;
            LoadCapacity = loadCapacity;
            Volume = volume;
            HydroBoard = hydroBoard;
            LengthBodywork = lengthBodywork;
            WidthBodywork = widthBodywork;
            HeightBodywork = heightBodywork;
            Stamp = stamp;
            Name = name;
            StateNumber = stateNumber;
            Status = status;
            TimeStart = timeStart;
            TimeEnd = timeEnd;
        }

        //public int Id { get; } //КодМашины
        public string TypeMachine { get; } //ТипМашины
        public string TypeBodywork { get; } //ТипКузова
        public string TypeLoading { get; } //ТипЗагрузки
        public float LoadCapacity { get; } //Грузоподъёмность
        public float Volume { get; } //Объём
        public bool HydroBoard { get; } //Гидроборт
        public float LengthBodywork { get; } //ДлинаКузова
        public float WidthBodywork { get; } //ШиринаКузова
        public float HeightBodywork { get; } //ВысотаКузова
        public string Stamp { get; } //Марка
        public string Name { get; } //Название
        public string StateNumber { get; } //ГосНомер
        public string Status { get; } //Состояние
        public DateTime TimeStart { get; } //ВремяПоступления
        public DateTime TimeEnd { get; } //ВремяОкончания

        public string GetSelectors() => " \"КодМашины\", \"ТипМашины\", \"ТипКузова\", \"ТипЗагрузки\", \"Грузоподъёмность\", \"Объём\", \"Гидроборт\", \"ДлинаКузова\", \"ШиринаКузова\", \"ВысотаКузова\", \"Марка\", \"Название\", \"ГосНомер\", \"Состояние\", \"ВремяПоступления\", \"ВремяОкончания\"";

        public string GetTable() => " From \"Машина\";";

        public override string ToString()
        {
            return $"{TypeMachine}{TypeBodywork}{TypeLoading}{LoadCapacity}{Volume}{HydroBoard}{LengthBodywork}{WidthBodywork}{HeightBodywork}{Stamp}{Name}{StateNumber}{Status}{TimeStart}{TimeEnd}";
        }

        public override bool Equals(object obj) => obj is Machine machine && Name == machine.Name;

        public override int GetHashCode() => HashCode.Combine(Name);

        public static bool operator ==(Machine machine1, Machine machine2)
        {
            if (machine1 is null && machine2 is null) return true;

            return machine1 is not null && machine1.Equals(machine2);
        }

        public static bool operator !=(Machine machine1, Machine machine2) => !(machine1 == machine2);
    }
}