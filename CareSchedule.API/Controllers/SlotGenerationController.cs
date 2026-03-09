using Microsoft.AspNetCore.Mvc;
using CareSchedule.API.Contracts;
using CareSchedule.DTOs;
using CareSchedule.Services.Interface;

namespace CareSchedule.API.Controllers
{
    [ApiController]
    [Route("availability/slot-generation")]
    public class SlotGenerationController : ControllerBase
    {
        private readonly IAvailabilityService _availability;

        public SlotGenerationController(IAvailabilityService availability)
        {
            _availability = availability;
        }

        // POST /availability/slot-generation/run
        // This is a controlled trigger for MVP; in production it would be a scheduled job.
        [HttpPost("run")]
        public ActionResult<ApiResponse<GenerateSlotsResponseDto>> Run([FromBody] GenerateSlotsRequestDto dto)
        {
            var result = _availability.GenerateSlots(dto);
            return ApiResponse<GenerateSlotsResponseDto>.Ok(result, "Slot generation completed.");
        }
    }
}