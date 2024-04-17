using System;
using System.Collections.Generic;
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

        public enum TypeMachineValues 
        {
            [Description("Грузовик")] Truck,
            [Description("Грузовик с прицепом")] TruckWithTrailer,
            [Description("Полуприцеп")] SemiTrailer,
            [Description("Микроавтобус")] Minibus
        }

        public enum TypeBodyworkValues 
        { 
            [Description("Тент")] Tent, 
            [Description("Реф")] Ref,
            [Description("Изотерм")] Isotherm,
            [Description("Борт")] Board
        }

        public enum TypeLoadingValues 
        {
            [Description("Бок")] Side,
            [Description("Вверх")] Up,
            [Description("Зад")] Behind
        }

        public enum MachineStatusValues 
        {
            [Description("Ремонт")] Repair,
            [Description("В ожидании")] Waiting,
            [Description("На стоянке")] Parking,
            [Description("В пути")] OnRoad
        }

        public static string GetEnumDescription(Enum value)
        {
            var field = value.GetType().GetField(value.ToString());
            var attribute = (DescriptionAttribute)Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute));
            return attribute?.Description ?? value.ToString();
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