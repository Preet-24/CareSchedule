using CareSchedule.API.Contracts;
using CareSchedule.DTOs;
using CareSchedule.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace CareSchedule.API.Controllers
{
    [ApiController]
    [Route("api/admin/systemconfigs")]
    public class SystemConfigsController : ControllerBase
    {
        private readonly ISystemConfigService _service;
        public SystemConfigsController(ISystemConfigService service) => _service = service;

        [HttpGet]
        public IActionResult Search([FromQuery] SystemConfigSearchQuery q)
            => Ok(ApiResponse<object>.Ok(_service.Search(q)));

        [HttpGet("{id:int}")]
        public ActionResult<ApiResponse<SystemConfigDto>> Get(int id)
            => Ok(ApiResponse<SystemConfigDto>.Ok(_service.Get(id)));

        [HttpPost]
        public ActionResult<ApiResponse<SystemConfigDto>> Create([FromBody] SystemConfigCreateDto dto)
        {
            if (dto is null) return BadRequest(ApiResponse<object>.Fail(new { code = "BAD_REQUEST" }, "Request body is required."));
            var created = _service.Create(dto);
            return CreatedAtAction(nameof(Get), new { id = created.ConfigId }, ApiResponse<SystemConfigDto>.Ok(created, "System config created."));
        }

        [HttpPut("{id:int}")]
        public ActionResult<ApiResponse<SystemConfigDto>> Update(int id, [FromBody] SystemConfigUpdateDto dto)
        {
            if (dto is null) return BadRequest(ApiResponse<object>.Fail(new { code = "BAD_REQUEST" }, "Request body is required."));
            return Ok(ApiResponse<SystemConfigDto>.Ok(_service.Update(id, dto), "System config updated."));
        }

        [HttpDelete("{id:int}")]
        public ActionResult<ApiResponse<object>> Delete(int id)
        {
            _service.Delete(id);
            return Ok(ApiResponse<object>.Ok(new { id }, "System config deleted."));
        }
    }
}