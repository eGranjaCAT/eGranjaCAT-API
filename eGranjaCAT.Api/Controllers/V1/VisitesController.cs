using Asp.Versioning;
using eGranjaCAT.Api.Extensions;
using eGranjaCAT.Application.DTOs.Visites;
using eGranjaCAT.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace eGranjaCAT.Api.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Authorize(Policy = "Visites")]
    [Route("api/v{version:apiVersion}/visites")]
    public class VisitesController : ControllerBase
    {
        private readonly IVisitaService _service;

        public VisitesController(IVisitaService service)
        {
            _service = service;
        }


        [HttpGet]
        public async Task<IActionResult> GetVisites([FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 20)
        {
            var result = await _service.GetVisitesAsync(pageIndex, pageSize);
            if (!result.Success) return StatusCode(result.StatusCode, new { result.Errors });

            return StatusCode(result.StatusCode, result.Data);
        }

        [HttpGet("farm-{farmId:int}")]
        public async Task<IActionResult> GetVisitesByFarm(int farmId, [FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 20)
        {
            var result = await _service.GetVisitesByFarmAsync(farmId, pageIndex, pageSize);
            if (!result.Success) return StatusCode(result.StatusCode, new { result.Errors });

            return StatusCode(result.StatusCode, result.Data);
        }

        [HttpGet("{id:int}", Name = "GetVisitaById")]
        public async Task<IActionResult> GetVisitaById(int id)
        {
            var result = await _service.GetVisitaByIdAsync(id);
            if (!result.Success) return StatusCode(result.StatusCode, new { result.Errors });

            return StatusCode(result.StatusCode, result.Data);
        }

        [HttpPost("farm-{farmId:int}")]
        public async Task<IActionResult> CreateVisita(int farmId, [FromBody] CreateVisitaDTO dto)
        {
            var userGuid = User.GetUserId();
            var result = await _service.CreateVisitaAsync(farmId, userGuid, dto);
            if (!result.Success) return StatusCode(result.StatusCode, new { result.Errors });

            return CreatedAtAction("GetVisitaById", new { farmId, id = result.Data }, null);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateVisita(int id, [FromBody] UpdateVisitaDTO dto)
        {
            var result = await _service.UpdateVisitaAsync(id, dto);
            if (!result.Success) return StatusCode(result.StatusCode, new { result.Errors });

            return StatusCode(result.StatusCode, result.Data);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteVisita(int id)
        {
            var result = await _service.DeleteVisitaAsync(id);
            if (!result.Success) return StatusCode(result.StatusCode, new { result.Errors });

            return StatusCode(result.StatusCode, result.Data);
        }

        [HttpGet("export")]
        public async Task<IActionResult> ExportAllVisites([FromQuery] int? pageIndex, [FromQuery] int? pageSize)
        {
            var stream = await _service.ExportVisitesAsync(pageIndex, pageSize);
            if (stream == null) return NotFound();
            var fileName = $"visites_{DateTime.Now:yyyyMMdd}.xlsx";

            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }

        [HttpGet("farm-{farmId:int}/export")]
        public async Task<IActionResult> ExportVisites(int farmId, [FromQuery] int? pageIndex, [FromQuery] int? pageSize)
        {
            var stream = await _service.ExportVisitesByFarmAsync(farmId, pageIndex, pageSize);
            if (stream == null) return NotFound();
            var fileName = $"visites_granja_{farmId}_{DateTime.Now:yyyyMMdd}.xlsx";

            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }

        [HttpGet("{id:int}/export")]
        public async Task<IActionResult> ExportVisitaById(int id)
        {
            var stream = await _service.ExportVisitesByIdAsync(id);
            if (stream == null) return NotFound();
            var fileName = $"visita_{id}_{DateTime.Now:yyyyMMdd}.xlsx";

            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }
    }
}
