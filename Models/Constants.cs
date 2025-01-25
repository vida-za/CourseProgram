using CourseProgram.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;

namespace CourseProgram.Models
{
    public static class Constants
    {
        public static class User
        {
            public static string Username { get; set; }
            public static string Password { get; set; }
        }

        #region Nomenclature enums
        public enum NomenclatureTypeValues
        {
            [Description("Продукты")] Products,
            [Description("Стройматериалы")] Buildings,
            [Description("Электроника")] Electronics,
            [Description("Химия")] Chemistry,
            [Description("Мебель")] Furniture,
            [Description("Другое")] Other,
            [Description("Неизвестно")] Null
        }

        public enum NomenclatureCategoriesValues
        {
            [Description("Опасные")] Dangerous,
            [Description("Скоропортящиеся")] Perishable,
            [Description("Стандартные")] Standard,
            [Description("Тяжелые")] Heavy,
            [Description("Габаритные")] Oversize,
            [Description("Неизвестно")] Null
        }

        public enum NomenclatureMeasureValues
        {
            [Description("шт")] sht,
            [Description("кг")] kg,
            [Description("м")] m,
            [Description("")] Null
        }

        public enum NomenclatureDangerousValues
        {
            [Description("Класс 1")] One,
            [Description("Класс 2")] Two,
            [Description("Класс 3")] Three,
            [Description("Не опасный")] None,
            [Description("Неизвестно")] Null
        }

        public enum NomenclaturePackingValues
        {
            [Description("Паллеты")] Pallet,
            [Description("Коробки")] Box,
            [Description("Контейнеры")] Container,
            [Description("Бочки")] Barrel,
            [Description("Мешки")] Bag,
            [Description("Без упаковки")] None,
            [Description("Неизвестно")] Null
        }
        #endregion

        #region Order enums
        public enum OrderStatusValues
        {
            [Description("Выполняется")] InProgress,
            [Description("Выполнен")] Completed,
            [Description("Отменен")] Cancelled,
            [Description("В ожидании")] Waiting,
            [Description("Ошибка!")] Null
        }
        #endregion

        #region Client enums
        public enum ClientTypeValues
        {
            [Description("Физлицо")] Physical,
            [Description("Юрлицо")] Legal,
            [Description("Ошибка!")] Null
        }
        #endregion

        #region Route enums
        public enum RouteStatusValues
        {
            [Description("Выполняется")] InProgress,
            [Description("Выполнен")] Completed,
            [Description("Отменен")] Cancelled,
            [Description("В ожидании")] Waiting,
            [Description("Ошибка!")] Null
        }

        public enum RouteTypeValues
        {
            [Description("Общий")] General,
            [Description("Обособленный")] Isolated,
            [Description("Пустой")] Empty,
            [Description("Ошибка!")] Null
        }
        #endregion

        #region Machine enums
        public enum MachineTypeValues
        {
            [Description("Грузовик")] Truck,
            [Description("Грузовик с прицепом")] TruckWithTrailer,
            [Description("Полуприцеп")] SemiTrailer,
            [Description("Микроавтобус")] Minibus,
            [Description("Ошибка!")] Null
        }

        public enum MachineTypeBodyworkValues 
        { 
            [Description("Тент")] Tent, 
            [Description("Реф")] Ref,
            [Description("Изотерм")] Isotherm,
            [Description("Борт")] Board,
            [Description("Неизвестно")] Null
        }

        public enum MachineTypeLoadingValues
        {
            [Description("Бок")] Side,
            [Description("Вверх")] Up,
            [Description("Зад")] Behind,
            [Description("Ошибка!")] Null
        }

        public enum MachineStatusValues
        {
            [Description("Ремонт")] Repair,
            [Description("В ожидании")] Waiting,
            [Description("На стоянке")] Parking,
            [Description("В пути")] OnRoad,
            [Description("Ошибка!")] Null
        }
        #endregion

        #region Bud enums
        public enum BudStatusValues
        {
            [Description("В ожидании")] Waiting,
            [Description("Принята")] Accepted,
            [Description("Отклонена")] Cancelled,
            [Description("Неизвестно")] Null
        }
        #endregion

        public enum Categories
        {
            [Description("A")] A,
            [Description("B")] B,
            [Description("BE")] BE,
            [Description("C")] C,
            [Description("CE")] CE,
            [Description("D")] D,
            [Description("DE")] DE,
        }

        public enum CommandTypes 
        { 
            SelectQuery, 
            InsertQuery, 
            UpdateQuery, 
            DeleteQuery, 
            Procedure, 
            ScalarFunction, 
            TableFunction 
        }


        public static string GetEnumDescription(Enum value)
        {
            var field = value.GetType().GetField(value.ToString());
            var attribute = (DescriptionAttribute)Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute));
            return attribute?.Description ?? value.ToString();
        }

        public static ObservableCollection<string> GetFullEnumDescription(Type enums)
        {
            ObservableCollection<string> array = new();
            foreach (Enum elem in Enum.GetValues(enums))
                if (GetEnumDescription(elem) != "Ошибка!")
                    array.Add(GetEnumDescription(elem));
            return array;
        }

        public static T Cast<T>(this Object myobj)
        {
            Type objectType = myobj.GetType();
            Type target = typeof(T);
            var temp = Activator.CreateInstance(target, false);
            var z = from source in objectType.GetMembers().ToList()
                    where source.MemberType == MemberTypes.Property
                    select source;
            var d = from source in target.GetMembers().ToList()
                    where source.MemberType == MemberTypes.Property
                    select source;
            List<MemberInfo> members = d.Where(memberInfo => d.Select(c => c.Name)
               .ToList().Contains(memberInfo.Name)).ToList();
            PropertyInfo propertyInfo;
            object value;
            foreach (var memberInfo in members)
            {
                propertyInfo = typeof(T).GetProperty(memberInfo.Name);
                value = myobj.GetType().GetProperty(memberInfo.Name).GetValue(myobj, null);

                propertyInfo.SetValue(temp, value, null);
            }
            return (T)temp;
        }

        #region Object to
        public static byte GetByteOrNull(object value, byte nullvalue)
        {
            if (Convert.DBNull != value && value != null)
                return Convert.ToByte(value);
            else return nullvalue;
        }

        public static int GetIntOrNull(object value, int nullvalue)
        {
            if (Convert.DBNull != value && value != null)
                return Convert.ToInt32(value);
            else return nullvalue;
        }

        public static Int64 GetInt64OrNull(object value, Int64 nullvalue)
        {
            if (Convert.DBNull != value && value != null)
                return Convert.ToInt64(value);
            else return nullvalue;
        }

        public static string? GetStringOrNull(object value, string nullvalue)
        {
            if (Convert.DBNull != value && value != null)
                return Convert.ToString(value);
            else return nullvalue;
        }

        public static bool GetBoolOrNull(object value, bool nullvalue)
        {
            if (Convert.DBNull != value && value != null)
                return Convert.ToBoolean(value);
            else return nullvalue;
        }

        public static DateTimeOffset GetDateTimeOffset(object value) => DateTimeOffset.Parse(input: Convert.ToString(value));

        public static DateTimeOffset GetDateTimeOffsetOrNull(object value, DateTimeOffset nullvalue)
        {
            if (Convert.DBNull != value && value != null)
                return DateTimeOffset.Parse(input: Convert.ToString(value));
            else return nullvalue;
        }

        public static double GetDoubleOrNull(object value, double nullvalue)
        {
            if (Convert.DBNull != value && value != null)
                return Convert.ToDouble(value);
            else return nullvalue;
        }

        public static float GetFloatOrNull(object value, float nullvalue)
        {
            if (Convert.DBNull != value && value != null)
                return (float)Convert.ToDouble(value);
            else return nullvalue;
        }

        public static DateTime GetDateTimeOrNull(object value, DateTime nullvalue)
        {
            if (Convert.DBNull != value && value != null)
                return Convert.ToDateTime(value);
            else return nullvalue;
        }

        public static DateOnly GetDateOnlyOrNull(object value, DateOnly nullvalue)
        {
            if (Convert.DBNull != value && value != null)
                return DateOnly.FromDateTime(Convert.ToDateTime(value));
            else return nullvalue;
        }
        #endregion
    }
}