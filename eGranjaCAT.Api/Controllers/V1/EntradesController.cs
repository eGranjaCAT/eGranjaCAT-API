using Asp.Versioning;
using eGranjaCAT.Api.Extensions;
using eGranjaCAT.Application.DTOs.Entrada;
using eGranjaCAT.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace eGranjaCAT.Api.Controllers.V1
{
    [Authorize(Policy = "Entrades")]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/{farmId:int}/entrades")]
    public class EntradesController : ControllerBase
    {
        private readonly IEntradaService _service;

        public EntradesController(IEntradaService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetEntrades(int farmId)
        {
            var result = await _service.GetEntradesAsync(farmId);
            if (!result.Success) return StatusCode(result.StatusCode, new { result.Errors });

            return StatusCode(result.StatusCode, result.Data);
        }

        [HttpGet("{id:int}", Name = "GetEntradaById")]
        public async Task<IActionResult> GetEntradaById(int id)
        {
            var result = await _service.GetEntradaByIdAsync(id);
            if (!result.Success) return StatusCode(result.StatusCode, new { result.Errors });

            return StatusCode(result.StatusCode, result.Data);
        }

        [HttpPost]
        public async Task<IActionResult> CreateEntrada(int farmId, [FromBody] CreateEntradaDTO dto)
        {
            var userGuid = User.GetUserId();
            var result = await _service.CreateEntradaAsync(farmId, userGuid, dto);
            if (!result.Success) return StatusCode(result.StatusCode, new { result.Errors });

            return StatusCode(result.StatusCode, result.Data); return CreatedAtAction("GetEntradaById", new { farmId, id = result.Data }, null);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateEntrada(int farmId, int id, [FromBody] UpdateEntradaDTO dto)
        {
            var result = await _service.UpdateEntradaAsync(farmId, id, dto);
            if (!result.Success) return StatusCode(result.StatusCode, new { result.Errors });

            return StatusCode(result.StatusCode, result.Data);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteEntrada(int farmId, int id)
        {
            var result = await _service.DeleteEntradaAsync(farmId, id);
            if (!result.Success) return StatusCode(result.StatusCode, new { result.Errors });

            return StatusCode(result.StatusCode, result.Data);
        }

        [HttpGet("export-all")]
        public async Task<IActionResult> ExportAllEntrades()
        {
            var stream = await _service.ExportEntradesAsync();
            if (stream == null) return NotFound();
            var fileName = $"entrades_{DateTime.Now:yyyyMMdd}.xlsx";

            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }

        [HttpGet("export")]
        public async Task<IActionResult> ExportEntrades(int farmId)
        {
            var stream = await _service.ExportEntradesByFarmAsync(farmId);
            if (stream == null) return NotFound();
            var fileName = $"entrades_granja_{farmId}_{DateTime.Now:yyyyMMdd}.xlsx";

            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }

        [HttpGet("export/{id:int}")]
        public async Task<IActionResult> ExportEntradaById(int id)
        {
            var stream = await _service.ExportEntradaByIdAsync(id);
            if (stream == null) return NotFound();
            var fileName = $"entrada_{id}_{DateTime.Now:yyyyMMdd}.xlsx";

            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }
    }
}