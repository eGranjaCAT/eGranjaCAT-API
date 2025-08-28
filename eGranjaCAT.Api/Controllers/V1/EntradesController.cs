using Asp.Versioning;
using eGranjaCAT.Api.Extensions;
using eGranjaCAT.Application.DTOs.Entrada;
using eGranjaCAT.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace eGranjaCAT.Api.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Authorize(Policy = "Entrades")]
    [Route("api/v{version:apiVersion}/entrades")]
    public class EntradesController : ControllerBase
    {
        private readonly IEntradaService _service;

        public EntradesController(IEntradaService service)
        {
            _service = service;
        }


        [HttpGet]
        public async Task<IActionResult> GetEntrades([FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 20)
        {
            var result = await _service.GetEntradesAsync(pageIndex, pageSize);
            if (!result.Success) return StatusCode(result.StatusCode, new { result.Errors });

            return StatusCode(result.StatusCode, result.Data);
        }

        [HttpGet("farm-{farmId:int}")]
        public async Task<IActionResult> GetEntradesByFarm(int farmId, [FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 20)
        {
            var result = await _service.GetEntradesByFarmAsync(farmId, pageIndex, pageSize);
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

        [HttpPost("farm-{farmId:int}")]
        public async Task<IActionResult> CreateEntrada(int farmId, [FromBody] CreateEntradaDTO dto)
        {
            var userGuid = User.GetUserId();
            var result = await _service.CreateEntradaAsync(farmId, userGuid, dto);
            if (!result.Success) return StatusCode(result.StatusCode, new { result.Errors });

            return CreatedAtAction("GetEntradaById", new { farmId, id = result.Data }, null);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateEntrada(int id, [FromBody] UpdateEntradaDTO dto)
        {
            var result = await _service.UpdateEntradaAsync(id, dto);
            if (!result.Success) return StatusCode(result.StatusCode, new { result.Errors });

            return StatusCode(result.StatusCode, result.Data);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteEntrada(int id)
        {
            var result = await _service.DeleteEntradaAsync(id);
            if (!result.Success) return StatusCode(result.StatusCode, new { result.Errors });

            return StatusCode(result.StatusCode, result.Data);
        }

        [HttpGet("export")]
        public async Task<IActionResult> ExportAllEntrades([FromQuery] int? pageIndex, [FromQuery] int? pageSize)
        {
            var stream = await _service.ExportEntradesAsync(pageIndex, pageSize);
            if (stream == null) return NotFound();
            var fileName = $"entrades_{DateTime.Now:yyyyMMdd}.xlsx";

            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }

        [HttpGet("farm-{farmId:int}/export")]
        public async Task<IActionResult> ExportEntrades(int farmId, [FromQuery] int? pageIndex, [FromQuery] int? pageSize)
        {
            var stream = await _service.ExportEntradesByFarmAsync(farmId, pageIndex, pageSize);
            if (stream == null) return NotFound();
            var fileName = $"entrades_granja_{farmId}_{DateTime.Now:yyyyMMdd}.xlsx";

            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }

        [HttpGet("{id:int}/export")]
        public async Task<IActionResult> ExportEntradaById(int id)
        {
            var stream = await _service.ExportEntradaByIdAsync(id);
            if (stream == null) return NotFound();
            var fileName = $"entrada_{id}_{DateTime.Now:yyyyMMdd}.xlsx";

            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }
    }
}