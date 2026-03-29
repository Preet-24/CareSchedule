using System;
using CareSchedule.DTOs;
using CareSchedule.Infrastructure;
using CareSchedule.Models;
using CareSchedule.Repositories.Interface;
using CareSchedule.Services.Interface;

namespace CareSchedule.Services.Implementation
{
    public class BillingService(
            IChargeRefRepository _chargeRepo,
            IAuditLogService _auditService,
            IUnitOfWork _uow)
            : IBillingService
    {
        public ChargeRefResponseDto CreateCharge(CreateChargeRefDto dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            if (dto.AppointmentId <= 0) throw new ArgumentException("Invalid AppointmentId.", nameof(dto.AppointmentId));
            if (dto.ServiceId <= 0) throw new ArgumentException("Invalid ServiceId.", nameof(dto.ServiceId));
            if (dto.ProviderId <= 0) throw new ArgumentException("Invalid ProviderId.", nameof(dto.ProviderId));
            if (dto.Amount <= 0) throw new ArgumentException("Amount must be greater than zero.", nameof(dto.Amount));

            var existing = _chargeRepo.GetByAppointmentId(dto.AppointmentId);
            if (existing != null)
                throw new ArgumentException($"Charge already exists for appointment {dto.AppointmentId}.");

            var charge = new ChargeRef
            {
                AppointmentId = dto.AppointmentId,
                ServiceId = dto.ServiceId,
                ProviderId = dto.ProviderId,
                Amount = dto.Amount,
                Currency = string.IsNullOrWhiteSpace(dto.Currency) ? "INR" : dto.Currency.Trim(),
                Status = "Pending"
            };

            _chargeRepo.Add(charge);
            _uow.SaveChanges();

            _auditService.CreateAudit(new AuditLogCreateDto
            {
                Action = "CreateCharge",
                Resource = "ChargeRef",
                Metadata = $"AppointmentId={dto.AppointmentId}; ChargeRefId={charge.ChargeRefId}; Amount={dto.Amount}; Currency={charge.Currency}"
            });

            return Map(charge);
        }

        public ChargeRefResponseDto GetByAppointment(int appointmentId)
        {
            if (appointmentId <= 0) throw new ArgumentException("Invalid appointmentId.", nameof(appointmentId));

            var charge = _chargeRepo.GetByAppointmentId(appointmentId)
                ?? throw new KeyNotFoundException($"Charge not found for appointment {appointmentId}.");

            return Map(charge);
        }

        private static ChargeRefResponseDto Map(ChargeRef charge) => new()
        {
            ChargeRefId = charge.ChargeRefId,
            AppointmentId = charge.AppointmentId,
            ServiceId = charge.ServiceId,
            ProviderId = charge.ProviderId,
            Amount = charge.Amount,
            Currency = charge.Currency,
            Status = charge.Status
        };
    }
}