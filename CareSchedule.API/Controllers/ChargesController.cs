using Microsoft.AspNetCore.Mvc;
using CareSchedule.API.Contracts;
using CareSchedule.DTOs;
using CareSchedule.Services.Interface;

namespace CareSchedule.API.Controllers
{
    [ApiController]
    [Route("charges")]
    public class ChargesController(IBillingService _billingservice) : ControllerBase
    {
        [HttpPost]
        public ActionResult<ApiResponse<ChargeRefResponseDto>> Create([FromBody] CreateChargeRefDto dto)
        {
            var result = _billingservice.CreateCharge(dto);
            return ApiResponse<ChargeRefResponseDto>.Ok(result, "Charge created.");
        }

        [HttpGet("by-appointment/{appointmentId:int}")]
        public ActionResult<ApiResponse<ChargeRefResponseDto>> GetByAppointment(int appointmentId)
        {
            var result = _billingservice.GetByAppointment(appointmentId);
            return ApiResponse<ChargeRefResponseDto>.Ok(result, "Charge fetched.");
        }
    }
}
