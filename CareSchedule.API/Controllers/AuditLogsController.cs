using CareSchedule.API.Contracts;
using CareSchedule.DTOs;
using CareSchedule.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace CareSchedule.API.Controllers
{
    [ApiController]
    [Route("api/iam/auditlogs")]
    public class AuditLogsController : ControllerBase
    {
        private readonly IAuditLogService _service;
        public AuditLogsController(IAuditLogService service) => _service = service;

        [HttpGet]
        public IActionResult Search([FromQuery] AuditLogSearchQuery q)
            => Ok(ApiResponse<object>.Ok(_service.SearchAudit(q)));

        [HttpGet("{id:int}")]
        public ActionResult<ApiResponse<AuditLogDto>> Get(int id)
            => Ok(ApiResponse<AuditLogDto>.Ok(_service.GetAudit(id)));

        [HttpPost]
        public ActionResult<ApiResponse<AuditLogDto>> Create([FromBody] AuditLogCreateDto dto)
        {
            if (dto is null) return BadRequest(ApiResponse<object>.Fail(new { code = "BAD_REQUEST" }, "Request body is required."));
            var created = _service.CreateAudit(dto);
            return CreatedAtAction(nameof(Get), new { id = created.AuditId }, ApiResponse<AuditLogDto>.Ok(created, "Audit log created."));
        }
    }
}