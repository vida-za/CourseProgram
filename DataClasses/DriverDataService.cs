using System.Collections.Generic;
using System;
using System.Data;
using System.Threading.Tasks;
using System.Threading;
using CourseProgram.Models;
using CourseProgram.DataClasses;
using System.Collections.ObjectModel;
using System.Linq;

namespace CourseProgram.Services
{
    public class DriverDataService : IDataService<Driver>
    {
        private readonly DBConnection db = new(5, null, null, "Base connection");
        readonly ObservableCollection<Driver> drivers;
        private readonly Driver driver = new();
        private readonly string query = string.Empty;

        public DriverDataService()
        {
            db.Login = User.Username;
            db.Password = User.Password;
            drivers = new ObservableCollection<Driver>();
            query = "Select" + driver.GetSelectors() + driver.GetTable();
            db.AutoConnect();
            Thread.Sleep(1000);
            foreach (DataRow datarow in db.GetDataTable(query))
            {
                drivers.Add(new Driver
                {
                    ID = DBConnection.GetIntOrNull(datarow["КодВодителя"], int.MaxValue),
                    Category = DBConnection.GetStringOrNull(datarow["Категория"], string.Empty),
                    FIO = DBConnection.GetStringOrNull(datarow["ФИО"], string.Empty),
                    BirthDay = DBConnection.GetDateOnlyOrNull(datarow["ДатаРождения"], DateOnly.MinValue),
                    Passport = DBConnection.GetStringOrNull(datarow["ПаспортныеДанные"], string.Empty),
                    Phone = DBConnection.GetStringOrNull(datarow["Телефон"], string.Empty),
                    DateStart = DBConnection.GetDateOnlyOrNull(datarow["ДатаНачала"], DateOnly.MinValue),
                    DateEnd = DBConnection.GetDateOnlyOrNull(datarow["ДатаОкончания"], DateOnly.MinValue)
                });
            }
            db.CancelConnect();
        }

        public async Task<bool> AddItemAsync(Driver drv)
        {
            drivers.Add(drv);
            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateItemAsync(Driver drv)
        {
            var oldDrv = drivers.Where((Driver arg) => arg.ID == drv.ID).FirstOrDefault();
            if (oldDrv != null) drivers.Remove(oldDrv);
            drivers.Add(drv);
            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(int id)
        {
            var oldDrv = drivers.Where((Driver arg) => arg.ID == id).FirstOrDefault();
            if (oldDrv != null) drivers.Remove(oldDrv);
            return await Task.FromResult(true);
        }

        public async Task<Driver> GetItemAsync(int id) => await Task.FromResult(drivers.FirstOrDefault(s => s.ID == id));

        public async Task<IEnumerable<Driver>> GetItemsAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(drivers);
        }
    }
}