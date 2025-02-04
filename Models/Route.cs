using System;
using System.ComponentModel;
using static CourseProgram.Models.Constants;

namespace CourseProgram.Models
{
    public class Route : IModel, IEquatable<Route>
    {
        [DisplayName("КодМаршрута")]
        public int ID { get; }
        [DisplayName("КодМашины")]
        public int? MachineID { get; }
        [DisplayName("КодВодителя")]
        public int? DriverID { get; }
        [DisplayName("Тип")]
        public RouteTypeValues Type { get; }
        [DisplayName("Статус")]
        public RouteStatusValues Status { get; }
        [DisplayName("ВремяВыполнения")]
        public DateTime? CompleteTime { get; }
        [DisplayName("КодАдресаНачала")]
        public int? AddressStartID { get; }
        [DisplayName("КодАдресаОкончания")]
        public int? AddressEndID { get; }

        public Route()
        {
            ID = 0;
            MachineID = null;
            DriverID = null;
            Type = RouteTypeValues.General;
            Status = RouteStatusValues.Waiting;
            CompleteTime = null;
            AddressStartID = null;
            AddressEndID = null;
        }

        public Route(int id, int? machine, int? driver, string type, string status, DateTime? completeTime, int? addressStartID, int? addressEndID)
        {
            ID = id;
            MachineID = machine;
            DriverID = driver;
            CompleteTime = completeTime;
            AddressStartID = addressStartID;
            AddressEndID = addressEndID;

            Type = type switch
            {
                "Общий" => RouteTypeValues.General,
                "Обособленный" => RouteTypeValues.Isolated,
                "Пустой" => RouteTypeValues.Empty,
                _ => throw new NotImplementedException()
            };
            Status = status switch
            {
                "Выполнен" => RouteStatusValues.Completed,
                "В очереди" => RouteStatusValues.Waiting,
                "Выполняется" => RouteStatusValues.InProgress,
                "Отменен" => RouteStatusValues.Cancelled,
                _ => throw new NotImplementedException()
            };
        }

        public Route(int iD, int? machineID, int? driverID, RouteTypeValues type, RouteStatusValues status, DateTime? completeTime, int? addressStartID, int? addressEndID)
        {
            ID = iD;
            MachineID = machineID;
            DriverID = driverID;
            Type = type;
            Status = status;
            CompleteTime = completeTime;
            AddressStartID = addressStartID;
            AddressEndID = addressEndID;
        }

        public static string GetTable() => "Маршрут";
        public static string GetSelectorID() => "КодМаршрута";
        public static string[] GetFieldNames()
        {
            return new[]
            {
                "КодМаршрута",
                "КодМашины",
                "КодВодителя",
                "Тип",
                "Статус",
                "ВремяВыполнения",
                "КодАдресаНачала",
                "КодАдресаОкончания"
            };
        }

        public bool Equals(Route? other) => other != null && ID == other.ID;

        public override bool Equals(object obj) => Equals(obj as Route);

        public override int GetHashCode() => HashCode.Combine(ID, MachineID);
    }
}