using CareSchedule.Models;
using CareSchedule.Infrastructure.Data;
using CareSchedule.Repositories.Interface;

namespace CareSchedule.Repositories.Implementation
{
    public class AppointmentChangeRepository : IAppointmentChangeRepository
    {
        private readonly CareScheduleContext _db;
        public AppointmentChangeRepository(CareScheduleContext db) { _db = db; }

        public void Add(AppointmentChange entity) => _db.AppointmentChanges.Add(entity);
    }
}