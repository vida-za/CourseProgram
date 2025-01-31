using System;
using System.ComponentModel;
using static CourseProgram.Models.Constants;

namespace CourseProgram.Models
{
    public class Order : IModel, IEquatable<Order>
    {
        [DisplayName("КодЗаказа")]
        public int ID { get; }
        [DisplayName("КодЗаявки")]
        public int BudID { get; }
        [DisplayName("ДатаЗаказа")]
        public DateTime TimeOrder { get; }
        [DisplayName("ДатаЗагрузки")]
        public DateTime? TimeLoad { get; }
        [DisplayName("ДатаВыгрузки")]
        public DateTime? TimeOnLoad { get; }
        [DisplayName("Стоимость")]
        public float? Price { get; }
        [DisplayName("Статус")]
        public OrderStatusValues Status { get; }
        [DisplayName("Договор")]
        public string? File { get; }

        public Order()
        {
            ID = 0;
            BudID = 0;
            TimeOrder = DateTime.MinValue;
            TimeLoad = DateTime.MinValue;
            TimeOnLoad = DateTime.MinValue;
            Price = 0;
            Status = OrderStatusValues.Waiting;
            File = string.Empty;
        }

        public Order(
            int id,
            int budID,
            DateTime timeOrder,
            DateTime? timeLoad,
            DateTime? timeOnLoad,
            float? price,
            string status,
            string? file)
        {
            ID = id;
            BudID = budID;
            TimeOrder = timeOrder;
            TimeLoad = timeLoad;
            TimeOnLoad = timeOnLoad;
            Price = price;
            File = file;

            Status = status switch
            {
                "Завершён" => OrderStatusValues.Completed,
                "Отменён" => OrderStatusValues.Cancelled,
                "Создан" => OrderStatusValues.Waiting,
                "В процессе" => OrderStatusValues.InProgress,
                _ => throw new NotImplementedException(),
            };
        }

        public Order(int iD, int budID, DateTime timeOrder, DateTime? timeLoad, DateTime? timeOnLoad, float? price, OrderStatusValues status, string? file)
        {
            ID = iD;
            BudID = budID;
            TimeOrder = timeOrder;
            TimeLoad = timeLoad;
            TimeOnLoad = timeOnLoad;
            Price = price;
            Status = status;
            File = file;
        }

        public static string GetTable() => "Заказ";
        public static string GetSelectorID() => "КодЗаказа";
        public static string[] GetFieldNames()
        {
            return new[]
            {
                "КодЗаказа",
                "НомерЗаявки",
                "ДатаЗаказа",
                "ДатаЗагрузки",
                "ДатаВыгрузки",
                "Стоимость",
                "Статус",
                "Договор"
            };
        }

        public bool Equals(Order? other) => other != null && ID == other.ID;

        public override bool Equals(object obj) => Equals(obj as Order);

        public override int GetHashCode() => HashCode.Combine(ID, BudID);
    }
}