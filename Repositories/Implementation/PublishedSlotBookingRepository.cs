using System.Linq;
using CareSchedule.Models;
using CareSchedule.Infrastructure.Data;
using CareSchedule.Repositories.Interface;

namespace CareSchedule.Repositories.Implementation
{
    public class PublishedSlotBookingRepository : IPublishedSlotBookingRepository
    {
        private readonly CareScheduleContext _db;
        public PublishedSlotBookingRepository(CareScheduleContext db) { _db = db; }

        public PublishedSlot? GetById(int publishedSlotId)
        {
            return _db.PublishedSlots.FirstOrDefault(s => s.PubSlotId == publishedSlotId);
        }

        public void Update(PublishedSlot entity) => _db.PublishedSlots.Update(entity);

        public PublishedSlot? FindExact(int providerId, int siteId, DateOnly date, TimeOnly start, TimeOnly end)
        {
            return _db.PublishedSlots.FirstOrDefault(s =>
                s.ProviderId == providerId &&
                s.SiteId == siteId &&
                s.SlotDate == date &&
                s.StartTime == start &&
                s.EndTime == end);
        }
    }
}