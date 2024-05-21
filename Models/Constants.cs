using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace CourseProgram.Models
{
    public static class Constants
    {
        private const string ServerConst = "localhost:5433";
        private const string DatabaseConst = "CourseDB";
        public static string Server = ServerConst;
        public static string Database = DatabaseConst;

        static public class User
        {
            static public string Username { get; set; }
            static public string Password { get; set; }
        }

        #region Cargo enums
        public enum TypeCargoValues
        {
            [Description("Негабаритный")] Oversize,
            [Description("Насыпной")] Bulk,
            [Description("Пылевидный")] Dusty,
            [Description("Наливной")] Tanker,
            [Description("Газообразный")] Gaseous,
            [Description("Штучный")] Retail,
            [Description("Скоропортящийся")] Perishable,
            [Description("Опасный")] Dangerous,
            [Description("Неизвестно")] Null
        }

        public enum CategoriesCargoValues
        {
            [Description("Легковесный")] Light,
            [Description("Обычный")] Usual,
            [Description("Тяжеловесный")] Heavy,
            [Description("Неизвестно")] Null
        }
        #endregion

        #region Order enums
        public enum StatusOrderValues
        {
            [Description("Выполняется")] InProgress,
            [Description("Выполнен")] Completed,
            [Description("Отменен")] Cancelled,
            [Description("В ожидании")] Waiting,
            [Description("Ошибка!")] Null
        }
        #endregion

        #region Client enums
        public enum TypeClientValues
        {
            [Description("Физлицо")] Physical,
            [Description("Юрлицо")] Legal,
            [Description("Ошибка!")] Null
        }
        #endregion

        #region Route enums
        public enum StatusRouteValues
        {
            [Description("Выполняется")] InProgress,
            [Description("Выполнен")] Completed,
            [Description("Отменен")] Cancelled,
            [Description("В ожидании")] Waiting,
            [Description("Ошибка!")] Null
        }

        public enum TypeRouteValues
        {
            [Description("Общий")] General,
            [Description("Обособленный")] Isolated,
            [Description("Пустой")] Empty,
            [Description("Ошибка!")] Null
        }
        #endregion

        #region Machine enums
        public enum TypeMachineValues 
        {
            [Description("Грузовик")] Truck,
            [Description("Грузовик с прицепом")] TruckWithTrailer,
            [Description("Полуприцеп")] SemiTrailer,
            [Description("Микроавтобус")] Minibus,
            [Description("Ошибка!")] Null
        }

        public enum TypeBodyworkValues 
        { 
            [Description("Тент")] Tent, 
            [Description("Реф")] Ref,
            [Description("Изотерм")] Isotherm,
            [Description("Борт")] Board,
            [Description("Неизвестно")] Null
        }

        public enum TypeLoadingValues 
        {
            [Description("Бок")] Side,
            [Description("Вверх")] Up,
            [Description("Зад")] Behind,
            [Description("Ошибка!")] Null
        }

        public enum StatusMachineValues 
        {
            [Description("Ремонт")] Repair,
            [Description("В ожидании")] Waiting,
            [Description("На стоянке")] Parking,
            [Description("В пути")] OnRoad,
            [Description("Ошибка!")] Null
        }
        #endregion

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
    }
}