using CareSchedule.API.Contracts;
using CareSchedule.DTOs;
using CareSchedule.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace CareSchedule.API.Controllers
{
    [ApiController]
    [Route("api/masterdata/sites")]
    public class SitesController : ControllerBase
    {
        private readonly ISiteService _service;
        public SitesController(ISiteService service) => _service = service;

        [HttpGet]
        public IActionResult Search([FromQuery] SiteSearchQuery q)
            => Ok(ApiResponse<object>.Ok(_service.SearchSite(q)));

        [HttpGet("{id:int}")]
        public ActionResult<ApiResponse<SiteDto>> Get(int id)
            => Ok(ApiResponse<SiteDto>.Ok(_service.GetSite(id)));

        [HttpPost]
        public ActionResult<ApiResponse<SiteDto>> Create([FromBody] SiteCreateDto dto)
        {
            if (dto is null) return BadRequest(ApiResponse<object>.Fail(new { code = "BAD_REQUEST" }, "Request body is required."));
            var created = _service.CreateSite(dto);
            return CreatedAtAction(nameof(Get), new { id = created.SiteId }, ApiResponse<SiteDto>.Ok(created, "Site created."));
        }

        [HttpPut("{id:int}")]
        public ActionResult<ApiResponse<SiteDto>> Update(int id, [FromBody] SiteUpdateDto dto)
        {
            if (dto is null) return BadRequest(ApiResponse<object>.Fail(new { code = "BAD_REQUEST" }, "Request body is required."));
            return Ok(ApiResponse<SiteDto>.Ok(_service.UpdateSite(id, dto), "Site updated."));
        }

        [HttpDelete("{id:int}")]
        public ActionResult<ApiResponse<object>> Deactivate(int id)
        {
            _service.DeactivateSite(id);
            return Ok(ApiResponse<object>.Ok(new { id }, "Site deactivated."));
        }

        [HttpPost("{id:int}/activate")]
        public ActionResult<ApiResponse<object>> Activate(int id)
        {
            _service.ActivateSite(id);
            return Ok(ApiResponse<object>.Ok(new { id }, "Site activated."));
        }
    }
}