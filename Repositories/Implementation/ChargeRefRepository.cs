using System;
using System.Collections.Generic;
using System.Linq;
using CareSchedule.Models;
using CareSchedule.Infrastructure.Data;
using CareSchedule.Repositories.Interface;

namespace CareSchedule.Repositories.Implementation
{
    public class ChargeRefRepository(CareScheduleContext _db) : IChargeRefRepository
    {
        public void Add(ChargeRef entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            _db.ChargeRefs.Add(entity);
        }

        public ChargeRef? GetByAppointmentId(int appointmentId)
        {
            if (appointmentId <= 0) throw new ArgumentException("Invalid appointmentId.", nameof(appointmentId));
            return _db.ChargeRefs.FirstOrDefault(c => c.AppointmentId == appointmentId);
        }

        public IEnumerable<ChargeRef> Search(int? appointmentId, int? providerId, string? status)
        {
            var q = _db.ChargeRefs.AsQueryable();

            if (appointmentId.HasValue && appointmentId.Value > 0)
                q = q.Where(c => c.AppointmentId == appointmentId.Value);
            if (providerId.HasValue && providerId.Value > 0)
                q = q.Where(c => c.ProviderId == providerId.Value);
            if (!string.IsNullOrWhiteSpace(status))
                q = q.Where(c => c.Status == status.Trim());

            return q.ToList();
        }
    }
}
