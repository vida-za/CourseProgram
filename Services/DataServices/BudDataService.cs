using CourseProgram.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using static CourseProgram.Models.Constants;

namespace CourseProgram.Services.DataServices
{
    public class BudDataService : BaseService<Bud>
    {
        public BudDataService() : base(User.Username, User.Password) { }

        public Task<IEnumerable<Bud>> GetBudsByWorkerController(int workerID) => Task.FromResult(items.Where(b => b.WorkerID == workerID));

        public Task<IEnumerable<Bud>> GetBudsByClientController(int clientID) => Task.FromResult(items.Where(b => b.ClientID == clientID));

        public override Task<Bud> CreateElement(DataRow row)
        {
            return Task.FromResult(new Bud(GetInt(row["КодЗаявки"], 0),
                GetInt(row["КодЗаказчика"], 0),
                GetInt(row["КодСотрудника"], 0),
                GetDateTime(row["ДатаВремяЗаявки"], DateTime.MinValue),
                GetString(row["Статус"], string.Empty),
                GetStringOrNull(row["Описание"])));
        }
    }
}