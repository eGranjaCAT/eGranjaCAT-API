using Asp.Versioning;
using eGranjaCAT.Api.Extensions;
using eGranjaCAT.Application.DTOs.Visites;
using eGranjaCAT.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace eGranjaCAT.Api.Controllers.V1
{
    [Authorize(Policy = "Visites")]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/{farmId:int}/visites")]
    public class VisitesController : ControllerBase
    {
        private readonly IVisitaService _service;

        public VisitesController(IVisitaService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetVisites(int farmId, [FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 20)
        {
            var result = await _service.GetVisitesAsync(farmId, pageIndex, pageSize);
            if (!result.Success) return StatusCode(result.StatusCode, new { result.Errors });

            return StatusCode(result.StatusCode, result.Data);
        }

        [HttpGet("{id:int}", Name = "GetVisitaById")]
        public async Task<IActionResult> GetEntradaById(int id)
        {
            var result = await _service.GetVisitaByIdAsync(id);
            if (!result.Success) return StatusCode(result.StatusCode, new { result.Errors });

            return StatusCode(result.StatusCode, result.Data);
        }

        [HttpPost]
        public async Task<IActionResult> CreateEntrada(int farmId, [FromBody] CreateVisitaDTO dto)
        {
            var userGuid = User.GetUserId();
            var result = await _service.CreateVisitaAsync(farmId, userGuid, dto);
            if (!result.Success) return StatusCode(result.StatusCode, new { result.Errors });

            return CreatedAtAction("GetVisitaById", new { farmId, id = result.Data }, null);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateVisita(int farmId, int id, [FromBody] UpdateVisitaDTO dto)
        {
            var result = await _service.UpdateVisitaAsync(farmId, id, dto);
            if (!result.Success) return StatusCode(result.StatusCode, new { result.Errors });

            return StatusCode(result.StatusCode, result.Data);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteVisita(int farmId, int id)
        {
            var result = await _service.DeleteVisitaAsync(farmId, id);
            if (!result.Success) return StatusCode(result.StatusCode, new { result.Errors });

            return StatusCode(result.StatusCode, result.Data);
        }

        [HttpGet("export-all")]
        public async Task<IActionResult> ExportAllVisites()
        {
            var stream = await _service.ExportVisitesAsync();
            if (stream == null) return NotFound();
            var fileName = $"visites_{DateTime.Now:yyyyMMdd}.xlsx";

            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }

        [HttpGet("export")]
        public async Task<IActionResult> ExportVisites(int farmId)
        {
            var stream = await _service.ExportVisitesByFarmAsync(farmId);
            if (stream == null) return NotFound();
            var fileName = $"visites_granja_{farmId}_{DateTime.Now:yyyyMMdd}.xlsx";

            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }

        [HttpGet("export/{id:int}")]
        public async Task<IActionResult> ExportVisitaById(int id)
        {
            var stream = await _service.ExportVisitesByIdAsync(id);
            if (stream == null) return NotFound();
            var fileName = $"visita_{id}_{DateTime.Now:yyyyMMdd}.xlsx";

            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }
    }
}
