using CareSchedule.API.Contracts;
using CareSchedule.DTOs;
using CareSchedule.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace CareSchedule.API.Controllers
{
    [ApiController]
    [Route("api/masterdata/rooms")]
    public class RoomsController : ControllerBase
    {
        private readonly IRoomService _service;
        public RoomsController(IRoomService service) => _service = service;

        [HttpGet]
        public IActionResult Search([FromQuery] RoomSearchQuery q)
            => Ok(ApiResponse<object>.Ok(_service.SearchRoom(q)));

        [HttpGet("{id:int}")]
        public ActionResult<ApiResponse<RoomDto>> Get(int id)
            => Ok(ApiResponse<RoomDto>.Ok(_service.GetRoom(id)));

        [HttpPost]
        public ActionResult<ApiResponse<RoomDto>> Create([FromBody] RoomCreateDto dto)
        {
            if (dto is null) return BadRequest(ApiResponse<object>.Fail(new { code = "BAD_REQUEST" }, "Request body is required."));
            var created = _service.CreateRoom(dto);
            return CreatedAtAction(nameof(Get), new { id = created.RoomId }, ApiResponse<RoomDto>.Ok(created, "Room created."));
        }

        [HttpPut("{id:int}")]
        public ActionResult<ApiResponse<RoomDto>> Update(int id, [FromBody] RoomUpdateDto dto)
        {
            if (dto is null) return BadRequest(ApiResponse<object>.Fail(new { code = "BAD_REQUEST" }, "Request body is required."));
            return Ok(ApiResponse<RoomDto>.Ok(_service.UpdateRoom(id, dto), "Room updated."));
        }

        [HttpDelete("{id:int}")]
        public ActionResult<ApiResponse<object>> Deactivate(int id)
        {
            _service.DeactivateRoom(id);
            return Ok(ApiResponse<object>.Ok(new { id }, "Room deactivated."));
        }

        [HttpPost("{id:int}/activate")]
        public ActionResult<ApiResponse<object>> Activate(int id)
        {
            _service.ActivateRoom(id);
            return Ok(ApiResponse<object>.Ok(new { id }, "Room activated."));
        }
    }
}