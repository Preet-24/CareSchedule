using Microsoft.AspNetCore.Mvc;
using CareSchedule.API.Contracts;
using CareSchedule.DTOs;
using CareSchedule.Services.Interface;

namespace CareSchedule.API.Controllers
{
    [ApiController]
    [Route("slots")]
    public class SlotsController : ControllerBase
    {
        private readonly IAvailabilityService _availability;

        public SlotsController(IAvailabilityService availability)
        {
            _availability = availability;
        }

        // GET /slots?providerId=&serviceId=&siteId=&date=YYYY-MM-DD
        [HttpGet]
        public ActionResult<ApiResponse<IEnumerable<SlotResponseDto>>> Get([FromQuery] int providerId, [FromQuery] int serviceId, [FromQuery] int siteId, [FromQuery] string date)
        {
            var data = _availability.GetOpenSlots(new SlotSearchRequestDto
            {
                ProviderId = providerId,
                ServiceId = serviceId,
                SiteId = siteId,
                Date = date
            });
            return ApiResponse<IEnumerable<SlotResponseDto>>.Ok(data, "Slots fetched.");
        }
    }
}