using eGranjaCAT.Api.Extensions;
using eGranjaCAT.Application.DTOs.Entrada;
using eGranjaCAT.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace eGranjaCAT.Api.Controllers
{
    [Authorize(Policy = "Entrades")]
    [ApiController]
    [Route("api/{farmId:int}/entrades")]
    public class EntradesController : ControllerBase
    {
        private readonly IEntradaService service;

        public EntradesController(IEntradaService service)
        {
            this.service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetEntrades(int farmId)
        {
            var result = await service.GetEntradesAsync(farmId);
            if (!result.Success) return BadRequest(new { result.Errors });
            return Ok(result.Data);
        }

        [HttpGet("{id:int}", Name = "GetEntradaById")]
        public async Task<IActionResult> GetEntradaById(int id)
        {
            var result = await service.GetEntradaByIdAsync(id);
            if (!result.Success) return BadRequest(new { result.Errors });
            if (result.Data == null) return NotFound();
            return Ok(result.Data);
        }

        [HttpPost]
        public async Task<IActionResult> CreateEntrada(int farmId, [FromBody] CreateEntradaDTO dto)
        {
            var userGuid = User.GetUserId();
            var result = await service.CreateEntradaAsync(farmId, userGuid, dto);
            if (!result.Success) return BadRequest(new { result.Errors });
            return CreatedAtAction("GetEntradaById", new { farmId, id = result.Data }, null);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateEntrada(int farmId, int id, [FromBody] UpdateEntradaDTO dto)
        {
            var result = await service.UpdateEntradaAsync(farmId, id, dto);
            if (!result.Success) return BadRequest(new { result.Errors });
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteEntrada(int farmId, int id)
        {
            var result = await service.DeleteEntradaAsync(farmId, id);
            if (!result.Success) return BadRequest(new { result.Errors });
            return NoContent();
        }

        [HttpGet("export-all")]
        public async Task<IActionResult> ExportAllEntrades()
        {
            var stream = await service.ExportEntradesAsync();
            if (stream == null) return NotFound();
            var fileName = $"entrades_{DateTime.Now:yyyyMMdd}.xlsx";
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }

        [HttpGet("export")]
        public async Task<IActionResult> ExportEntrades(int farmId)
        {
            var stream = await service.ExportEntradesByFarmAsync(farmId);
            if (stream == null) return NotFound();
            var fileName = $"entrades_granja_{farmId}_{DateTime.Now:yyyyMMdd}.xlsx";
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }

        [HttpGet("export/{id:int}")]
        public async Task<IActionResult> ExportEntradaById(int id)
        {
            var stream = await service.ExportEntradaByIdAsync(id);
            if (stream == null) return NotFound();
            var fileName = $"entrada_{id}_{DateTime.Now:yyyyMMdd}.xlsx";
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }
    }
}