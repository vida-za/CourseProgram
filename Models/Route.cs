﻿using System;
using System.ComponentModel;
using static CourseProgram.Models.Constants;

namespace CourseProgram.Models
{
    public class Route : IModel, IEquatable<Route>
    {
        [DisplayName("КодМаршрута")]
        public int ID { get; }
        [DisplayName("КодМашины")]
        public int MachineID { get; }
        [DisplayName("КодВодителя")]
        public int DriverID { get; }
        [DisplayName("Тип")]
        public RouteTypeValues Type { get; }
        [DisplayName("Расстояние")]
        public float Distance { get; }
        [DisplayName("Статус")]
        public RouteStatusValues Status { get; }
        [DisplayName("ВремяВыполнения")]
        public DateTime CompleteTime { get; }
        [DisplayName("КодАдресаНачала")]
        public int AddressStartID { get; }
        [DisplayName("КодАдресаОкончания")]
        public int AddressEndID { get; }

        public Route()
        {
            ID = 0;
            MachineID = 0;
            DriverID = 0;
            Type = RouteTypeValues.Null;
            Distance = float.NaN;
            Status = RouteStatusValues.Null;
            CompleteTime = DateTime.MinValue;
            AddressStartID = 0;
            AddressEndID = 0;
        }

        public Route(int id, int machine, int driver, string type, float distance, string status, DateTime completeTime, int addressStartID, int addressEndID)
        {
            ID = id;
            MachineID = machine;
            DriverID = driver;
            Distance = distance;
            CompleteTime = completeTime;
            AddressStartID = addressStartID;
            AddressEndID = addressEndID;

            Type = type switch
            {
                "Общий" => RouteTypeValues.General,
                "Обособленный" => RouteTypeValues.Isolated,
                "Пустой" => RouteTypeValues.Empty,
                _ => RouteTypeValues.Null,
            };
            Status = status switch
            {
                "Выполнен" => RouteStatusValues.Completed,
                "В ожидании" => RouteStatusValues.Waiting,
                "Выполняется" => RouteStatusValues.InProgress,
                "Отменен" => RouteStatusValues.Cancelled,
                _ => RouteStatusValues.Null,
            };
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
                "Расстояние",
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