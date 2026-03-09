using CareSchedule.Models;
using CareSchedule.Infrastructure.Data;
using CareSchedule.Repositories.Interface;

namespace CareSchedule.Repositories.Implementation
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly CareScheduleContext _db;
        public NotificationRepository(CareScheduleContext db) { _db = db; }

        public void Add(Notification entity) => _db.Notifications.Add(entity);
    }
}