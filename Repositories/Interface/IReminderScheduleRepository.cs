using CareSchedule.Models;

namespace CareSchedule.Repositories.Interface
{
    public interface IReminderScheduleRepository
    {
        void Add(ReminderSchedule entity);
    }
}