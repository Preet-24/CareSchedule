using System.Collections.Generic;
using CareSchedule.DTOs;

namespace CareSchedule.Services.Interface
{
    public interface IBookingService
    {
        AppointmentResponseDto Book(BookAppointmentRequestDto dto);
        AppointmentResponseDto Reschedule(int appointmentId, RescheduleAppointmentRequestDto dto);
        void Cancel(int appointmentId, CancelAppointmentRequestDto dto);
        AppointmentResponseDto GetById(int appointmentId);
        IEnumerable<AppointmentResponseDto> Search(AppointmentSearchRequestDto dto);
    }
}