using System;
using System.ComponentModel;
using static CourseProgram.Models.Constants;

namespace CourseProgram.Models
{
    public class Machine : IModel, IEquatable<Machine>
    {
        [DisplayName("КодМашины")]
        public int ID { get; }
        [DisplayName("ТипМашины")]
        public MachineTypeValues TypeMachine { get; }
        [DisplayName("ТипКузова")]
        public MachineTypeBodyworkValues TypeBodywork { get; }
        [DisplayName("ТипЗагрузки")]
        public MachineTypeLoadingValues TypeLoading { get; }
        [DisplayName("Грузоподъёмность")]
        public float LoadCapacity { get; }
        [DisplayName("Объём")]
        public float? Volume { get; }
        [DisplayName("Гидроборт")]
        public bool HydroBoard { get; }
        [DisplayName("ДлинаКузова")]
        public float? LengthBodywork { get; }
        [DisplayName("ШиринаКузова")]
        public float? WidthBodywork { get; }
        [DisplayName("ВысотаКузова")]
        public float? HeightBodywork { get; }
        [DisplayName("Марка")]
        public string Stamp { get; }
        [DisplayName("Название")]
        public string Name { get; }
        [DisplayName("ГосНомер")]
        public string? StateNumber { get; }
        [DisplayName("Состояние")]
        public MachineStatusValues Status { get; }
        [DisplayName("ДатаВремяПоступления")]
        public DateTime TimeStart { get; }
        [DisplayName("ДатаВремяСписания")]
        public DateTime? TimeEnd { get; }
        public string? Town { get; }

        public Machine()
        {
            ID = 0;
            TypeMachine = MachineTypeValues.Truck;
            TypeBodywork = MachineTypeBodyworkValues.Null;
            TypeLoading = MachineTypeLoadingValues.Behind;
            LoadCapacity = float.NaN;
            Volume = float.NaN;
            HydroBoard = false;
            LengthBodywork = float.NaN;
            WidthBodywork = float.NaN;
            HeightBodywork = float.NaN;
            Stamp = string.Empty;
            Name = string.Empty;
            StateNumber = string.Empty;
            Status = MachineStatusValues.Waiting;
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
            float? volume,
            bool hydroBoard,
            float? lengthBodywork,
            float? widthBodywork,
            float? heightBodywork,
            string stamp,
            string name,
            string? stateNumber,
            string status,
            DateTime timeStart,
            DateTime? timeEnd,
            string? town)
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
                "Микроавтобус" => MachineTypeValues.Minibus,
                "Грузовик" => MachineTypeValues.Truck,
                "Грузовик с прицепом" => MachineTypeValues.TruckWithTrailer,
                "Полуприцеп" => MachineTypeValues.SemiTrailer,
                _ => throw new NotImplementedException(),
            };
            TypeBodywork = typeBodywork switch
            {
                "Борт" => MachineTypeBodyworkValues.Board,
                "Изотерм" => MachineTypeBodyworkValues.Isotherm,
                "Тент" => MachineTypeBodyworkValues.Tent,
                "Реф" => MachineTypeBodyworkValues.Ref,
                _ => MachineTypeBodyworkValues.Null,
            };
            TypeLoading = typeLoading switch
            {
                "Вверх" => MachineTypeLoadingValues.Up,
                "Зад" => MachineTypeLoadingValues.Behind,
                "Бок" => MachineTypeLoadingValues.Side,
                _ => throw new NotImplementedException(),
            };
            Status = status switch
            {
                "В ожидании" => MachineStatusValues.Waiting,
                "На стоянке" => MachineStatusValues.Parking,
                "В пути" => MachineStatusValues.OnRoad,
                "Ремонт" => MachineStatusValues.Repair,
                _ => throw new NotImplementedException(),
            };
        }

        public Machine(
            int iD,
            MachineTypeValues typeMachine,
            MachineTypeBodyworkValues typeBodywork,
            MachineTypeLoadingValues typeLoading,
            float loadCapacity,
            float? volume,
            bool hydroBoard,
            float? lengthBodywork,
            float? widthBodywork,
            float? heightBodywork,
            string stamp,
            string name,
            string? stateNumber,
            MachineStatusValues status,
            DateTime timeStart,
            DateTime? timeEnd,
            string? town)
        {
            ID = iD;
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
            Town = town;
        }

        public static string GetTable() => "Машина";
        public static string GetSelectorID() => "КодМашины";
        public static string[] GetFieldNames()
        {
            return new[]
            {
                "КодМашины",
                "ТипМашины",
                "ТипКузова",
                "ТипЗагрузки",
                "Грузоподъёмность",
                "Объём",
                "Гидроборт",
                "ДлинаКузова",
                "ШиринаКузова",
                "ВысотаКузова",
                "Марка",
                "Название",
                "ГосНомер",
                "Состояние",
                "ДатаВремяПоступления",
                "ДатаВремяСписания"
            };
        }

        public override string ToString()
        {
            return $"{TypeMachine}{TypeBodywork}{TypeLoading}{LoadCapacity}{Volume}{HydroBoard}{LengthBodywork}{WidthBodywork}{HeightBodywork}{Stamp}{Name}{StateNumber}{Status}{TimeStart}{TimeEnd}";
        }

        public bool Equals(Machine? other) => other != null && ID == other.ID;

        public override bool Equals(object obj) => Equals(obj as Machine);

        public override int GetHashCode() => HashCode.Combine(ID);
    }
}