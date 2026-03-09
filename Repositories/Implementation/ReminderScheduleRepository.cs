using CareSchedule.Models;
using CareSchedule.Infrastructure.Data;
using CareSchedule.Repositories.Interface;

namespace CareSchedule.Repositories.Implementation
{
    public class ReminderScheduleRepository : IReminderScheduleRepository
    {
        private readonly CareScheduleContext _db;
        public ReminderScheduleRepository(CareScheduleContext db) { _db = db; }

        public void Add(ReminderSchedule entity) => _db.ReminderSchedules.Add(entity);
    }
}