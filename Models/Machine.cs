using System;
using System.ComponentModel;
using static CourseProgram.Models.Constants;

namespace CourseProgram.Models
{
    public class Machine : IModel, IEquatable<Machine>
    {
        [DisplayName("Номер машины")]
        public int ID { get; } //КодМашины
        [DisplayName("Тип машины")]
        public TypeMachineValues TypeMachine { get; } //ТипМашины
        [DisplayName("Тип кузова")]
        public TypeBodyworkValues TypeBodywork { get; } //ТипКузова
        [DisplayName("Тип загрузки")]
        public TypeLoadingValues TypeLoading { get; } //ТипЗагрузки
        [DisplayName("Грузоподъёмность")]
        public float LoadCapacity { get; } //Грузоподъёмность
        [DisplayName("Объём")]
        public float Volume { get; } //Объём
        [DisplayName("Гидроборт")]
        public bool HydroBoard { get; } //Гидроборт
        [DisplayName("Длина кузова")]
        public float LengthBodywork { get; } //ДлинаКузова
        [DisplayName("Ширина кузова")]
        public float WidthBodywork { get; } //ШиринаКузова
        [DisplayName("Высота кузова")]
        public float HeightBodywork { get; } //ВысотаКузова
        [DisplayName("Марка")]
        public string Stamp { get; } //Марка
        [DisplayName("Название")]
        public string Name { get; } //Название
        [DisplayName("ГосНомер")]
        public string StateNumber { get; } //ГосНомер
        [DisplayName("Состояние")]
        public StatusMachineValues Status { get; } //Состояние
        [DisplayName("Получена")]
        public DateTime TimeStart { get; } //ВремяПоступления
        [DisplayName("Списана")]
        public DateTime TimeEnd { get; } //ВремяОкончания
        [DisplayName("Город")]
        public string Town { get; } //Город

        public Machine()
        {
            ID = 0;
            TypeMachine = TypeMachineValues.Null;
            TypeBodywork = TypeBodyworkValues.Null;
            TypeLoading = TypeLoadingValues.Null;
            LoadCapacity = float.NaN;
            Volume = float.NaN;
            HydroBoard = false;
            LengthBodywork = float.NaN;
            WidthBodywork = float.NaN;
            HeightBodywork = float.NaN;
            Stamp = string.Empty;
            Name = string.Empty;
            StateNumber = string.Empty;
            Status = StatusMachineValues.Null;
            TimeStart = new DateTime();
            TimeEnd = new DateTime();
            Town = string.Empty;
        }

        public Machine(
            int id,
            string typeMachine,
            string typeBodywork,
            string typeLoading,
            float loadCapacity,
            float volume,
            bool hydroBoard,
            float lengthBodywork,
            float widthBodywork,
            float heightBodywork,
            string stamp,
            string name,
            string stateNumber,
            string status,
            DateTime timeStart,
            DateTime timeEnd,
            string town)
        {
            ID = id;
            LoadCapacity = loadCapacity;
            Volume = volume;
            HydroBoard = hydroBoard;
            LengthBodywork = lengthBodywork;
            WidthBodywork = widthBodywork;
            HeightBodywork = heightBodywork;
            Stamp = stamp;
            Name = name;
            StateNumber = stateNumber;
            TimeStart = timeStart;
            TimeEnd = timeEnd;
            Town = town;

            TypeMachine = typeMachine switch
            {
                "Микроавтобус" => TypeMachineValues.Minibus,
                "Грузовик" => TypeMachineValues.Truck,
                "Грузовик с прицепом" => TypeMachineValues.TruckWithTrailer,
                "Полуприцеп" => TypeMachineValues.SemiTrailer,
                _ => TypeMachineValues.Null,
            };
            TypeBodywork = typeBodywork switch
            {
                "Борт" => TypeBodyworkValues.Board,
                "Изотерм" => TypeBodyworkValues.Isotherm,
                "Тент" => TypeBodyworkValues.Tent,
                "Реф" => TypeBodyworkValues.Ref,
                _ => TypeBodyworkValues.Null,
            };
            TypeLoading = typeLoading switch
            {
                "Вверх" => TypeLoadingValues.Up,
                "Зад" => TypeLoadingValues.Behind,
                "Бок" => TypeLoadingValues.Side,
                _ => TypeLoadingValues.Null,
            };
            Status = status switch
            {
                "В ожидании" => StatusMachineValues.Waiting,
                "На стоянке" => StatusMachineValues.Parking,
                "В пути" => StatusMachineValues.OnRoad,
                "Ремонт" => StatusMachineValues.Repair,
                _ => StatusMachineValues.Null,
            };
        }

        public string GetSelectors() => "\"КодМашины\", \"ТипМашины\", \"ТипКузова\", \"ТипЗагрузки\", \"Грузоподъёмность\", \"Объём\", \"Гидроборт\", \"ДлинаКузова\", \"ШиринаКузова\", \"ВысотаКузова\", \"Марка\", \"Название\", \"ГосНомер\", \"Состояние\", \"ВремяПоступления\", \"ВремяОкончания\"";
        public string GetTable() => "\"Машина\"";
        public string GetSelectorID() => "\"КодМашины\"";
        public string GetProcedureDelete() => "\"DeleteMachine\"";

        public override string ToString()
        {
            return $"{TypeMachine}{TypeBodywork}{TypeLoading}{LoadCapacity}{Volume}{HydroBoard}{LengthBodywork}{WidthBodywork}{HeightBodywork}{Stamp}{Name}{StateNumber}{Status}{TimeStart}{TimeEnd}";
        }

        public bool Equals(Machine? other)
        {
            if (other != null)
                return ID == other.ID;
            else return false;
        }

        public override bool Equals(object obj) => Equals(obj as Machine);

        public override int GetHashCode() => HashCode.Combine(ID);
    }
}