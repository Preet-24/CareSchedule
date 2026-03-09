using Microsoft.AspNetCore.Mvc;
using CareSchedule.API.Contracts;
using CareSchedule.DTOs;
using CareSchedule.Services.Interface;

namespace CareSchedule.API.Controllers
{
    [ApiController]
    [Route("availability-blocks")]
    public class AvailabilityBlocksController : ControllerBase
    {
        private readonly IAvailabilityService _availability;

        public AvailabilityBlocksController(IAvailabilityService availability)
        {
            _availability = availability;
        }

        // POST /availability-blocks
        [HttpPost]
        public ActionResult<ApiResponse<IdResponseDto>> Create([FromBody] CreateAvailabilityBlockRequestDto dto)
        {
            var id = _availability.CreateBlock(dto);
            return ApiResponse<IdResponseDto>.Ok(new IdResponseDto { Id = id }, "Block created.");
        }

        // DELETE /availability-blocks/{blockId}
        [HttpDelete("{blockId:int}")]
        public ActionResult<ApiResponse<object>> Delete(int blockId)
        {
            _availability.RemoveBlock(blockId);
            return ApiResponse<object>.Ok(null, "Block removed.");
        }

        // GET /availability-blocks?providerId=&siteId=&date=
        [HttpGet]
        public ActionResult<ApiResponse<IEnumerable<AvailabilityBlockResponseDto>>> List([FromQuery] int providerId, [FromQuery] int siteId, [FromQuery] string? date)
        {
            var data = _availability.ListBlocks(providerId, siteId, date);
            return ApiResponse<IEnumerable<AvailabilityBlockResponseDto>>.Ok(data, "Blocks fetched.");
        }
    }
}