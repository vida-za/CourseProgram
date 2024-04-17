using System;
using System.ComponentModel;

namespace CourseProgram.Models
{
    public class Order : IModel, IEquatable<Order>
    {
        [DisplayName("Номер заказа")]
        public int ID { get; } //КодЗаказа
        [DisplayName("Номер сотрудника")]
        public int WorkerID { get; } //КодСотрудника
        [DisplayName("Номер заказчика")]
        public int ClientID { get; } //КодЗаказчика
        [DisplayName("Дата заказа")]
        public DateTime TimeOrder { get; } //ДатаЗаказа
        [DisplayName("Дата загрузки")]
        public DateTime TimeLoad { get; } //ДатаЗагрузки
        [DisplayName("Дата выгрузки")]
        public DateTime TimeOnLoad { get; } //ДатаВыгрузки
        [DisplayName("Стоимость")]
        public float Price { get; } //Стоимость
        [DisplayName("Статус")]
        public string Status { get; } //Статус
        [DisplayName("Договор")]
        public string File { get; } //Договор

        public Order()
        {
            ID = 0;
            WorkerID = 0;
            ClientID = 0;
            TimeOrder = DateTime.MinValue;
            TimeLoad = DateTime.MinValue;
            TimeOnLoad = DateTime.MinValue;
            Price = 0;
            Status = string.Empty;
            File = string.Empty;
        }

        public Order(
            int id,
            int workerID,
            int clientID,
            DateTime timeOrder,
            DateTime timeLoad,
            DateTime timeOnLoad,
            float price,
            string status,
            string file)
        {
            ID = id;
            WorkerID = workerID;
            ClientID = clientID;
            TimeOrder = timeOrder;
            TimeLoad = timeLoad;
            TimeOnLoad = timeOnLoad;
            Price = price;
            Status = status;
            File = file;
        }

        public string GetSelectors() => "\"КодЗаказа\", \"КодСотрудника\", \"КодЗаказчика\", \"ДатаЗаказа\", \"ДатаЗагрузки\", \"ДатаВыгрузки\", \"Стоимость\", \"Статус\", \"Договор\"";
        public string GetTable() => "\"Заказ\"";
        public string GetSelectorID() => "\"КодЗаказа\"";
        public string GetProcedureDelete() => "\"DeleteOrder\"";

        public bool Equals(Order? other)
        {
            if (other != null)
                return ID == other.ID;
            else return false;
        }

        public override bool Equals(object obj) => Equals(obj as Order);

        public override int GetHashCode() => HashCode.Combine(ID, WorkerID);
    }
}