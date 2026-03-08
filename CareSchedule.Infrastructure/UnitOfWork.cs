using CareSchedule.Infrastructure.Data;

namespace CareSchedule.Infrastructure
{
    public interface IUnitOfWork
    {
        int SaveChanges();
    }

    public class UnitOfWork : IUnitOfWork
    {
        private readonly CareScheduleContext _db;

        public UnitOfWork(CareScheduleContext db)
        {
            _db = db;
        }

        public int SaveChanges()
        {
            return _db.SaveChanges();
        }
    }
}