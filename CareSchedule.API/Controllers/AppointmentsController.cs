using Microsoft.AspNetCore.Mvc;
using CareSchedule.API.Contracts;
using CareSchedule.DTOs;
using CareSchedule.Services.Interface;

namespace CareSchedule.API.Controllers
{
    [ApiController]
    [Route("appointments")]
    public class AppointmentsController : ControllerBase
    {
        private readonly IBookingService _service;

        public AppointmentsController(IBookingService service)
        {
            _service = service;
        }

        // POST /appointments  (Book)
        [HttpPost]
        public ActionResult<ApiResponse<AppointmentResponseDto>> Book([FromBody] BookAppointmentRequestDto dto)
        {
            var result = _service.Book(dto);
            return ApiResponse<AppointmentResponseDto>.Ok(result, "Appointment booked.");
        }

        // PATCH /appointments/{appointmentId}  (Reschedule)
        [HttpPatch("{appointmentId:int}")]
        public ActionResult<ApiResponse<AppointmentResponseDto>> Reschedule(int appointmentId, [FromBody] RescheduleAppointmentRequestDto dto)
        {
            var result = _service.Reschedule(appointmentId, dto);
            return ApiResponse<AppointmentResponseDto>.Ok(result, "Appointment rescheduled.");
        }

        // PATCH /appointments/{appointmentId}/cancel  (Cancel)
        [HttpPatch("{appointmentId:int}/cancel")]
        public ActionResult<ApiResponse<object>> Cancel(int appointmentId, [FromBody] CancelAppointmentRequestDto dto)
        {
            _service.Cancel(appointmentId, dto);
            return ApiResponse<object>.Ok(null, "Appointment cancelled.");
        }

        // GET /appointments?patientId=&providerId=&siteId=&date=yyyy-MM-dd&status=
        [HttpGet]
        public ActionResult<ApiResponse<IEnumerable<AppointmentResponseDto>>> Search([FromQuery] AppointmentSearchRequestDto dto)
        {
            var list = _service.Search(dto);
            return ApiResponse<IEnumerable<AppointmentResponseDto>>.Ok(list, "Appointments fetched.");
        }

        // GET /appointments/{appointmentId}
        [HttpGet("{appointmentId:int}")]
        public ActionResult<ApiResponse<AppointmentResponseDto>> GetById(int appointmentId)
        {
            var item = _service.GetById(appointmentId);
            return ApiResponse<AppointmentResponseDto>.Ok(item, "Appointment fetched.");
        }
    }
}