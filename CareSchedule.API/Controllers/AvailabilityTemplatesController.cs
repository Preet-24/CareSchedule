using Microsoft.AspNetCore.Mvc;
using CareSchedule.API.Contracts;
using CareSchedule.DTOs;
using CareSchedule.Services.Interface;

namespace CareSchedule.API.Controllers
{
    [ApiController]
    [Route("availability-templates")]
    public class AvailabilityTemplatesController : ControllerBase
    {
        private readonly IAvailabilityService _availability;

        public AvailabilityTemplatesController(IAvailabilityService availability)
        {
            _availability = availability;
        }

        // POST /availability-templates
        [HttpPost]
        public ActionResult<ApiResponse<IdResponseDto>> Create([FromBody] CreateAvailabilityTemplateRequestDto dto)
        {
            var id = _availability.CreateTemplate(dto);
            return ApiResponse<IdResponseDto>.Ok(new IdResponseDto { Id = id }, "Template created.");
        }

        // PUT /availability-templates/{templateId}
        [HttpPut("{templateId:int}")]
        public ActionResult<ApiResponse<object>> Update(int templateId, [FromBody] UpdateAvailabilityTemplateRequestDto dto)
        {
            dto.TemplateId = templateId;
            _availability.UpdateTemplate(dto);
            return ApiResponse<object>.Ok(null, "Template updated.");
        }

        // GET /availability-templates?providerId=&siteId=
        [HttpGet]
        public ActionResult<ApiResponse<IEnumerable<AvailabilityTemplateResponseDto>>> List([FromQuery] int providerId, [FromQuery] int siteId)
        {
            var data = _availability.ListTemplates(providerId, siteId);
            return ApiResponse<IEnumerable<AvailabilityTemplateResponseDto>>.Ok(data, "Templates fetched.");
        }
    }
}