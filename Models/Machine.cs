using System;

namespace CourseProgram.Models
{
    public class Machine
    {
        public Machine(int id, string typeMachine, string typeBodywork, string typeLoading, float loadCapacity, float volume, bool hydroBoard, float lengthBodywork, float widthBodywork, float heightBodywork, string stamp, string name, string stateNumber, string status, DateTime timeStart, DateTime timeEnd)
        {
            Id = id;
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

        public int Id { get; } //КодМашины
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
    }
}