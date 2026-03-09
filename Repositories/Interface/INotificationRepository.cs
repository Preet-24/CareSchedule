using CareSchedule.Models;

namespace CareSchedule.Repositories.Interface
{
    public interface INotificationRepository
    {
        void Add(Notification entity);
    }
}