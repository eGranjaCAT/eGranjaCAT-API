using Asp.Versioning;
using eGranjaCAT.Api.Extensions;
using eGranjaCAT.Application.DTOs.Lot;
using eGranjaCAT.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace eGranjaCAT.Api.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Authorize(Policy = "Lots")]
    [Route("api/v{version:apiVersion}/lots")]
    public class LotsController : ControllerBase
    {
        private readonly ILotService _service;

        public LotsController(ILotService service)
        {
            _service = service;
        }


        [HttpPost("farm-{farmId:int}")]
        public async Task<IActionResult> CreateLot(int farmId, [FromBody] CreateLotDTO dto)
        {
            var userGuid = User.GetUserId();
            var result = await _service.CreateLotAsync(farmId, userGuid, dto);
            if (!result.Success) return StatusCode(result.StatusCode, new { result.Errors });

            return CreatedAtAction("GetLotById", new { farmId, id = result.Data }, null);
        }

        [HttpGet("farm-{farmId:int}/active")]
        public async Task<IActionResult> GetActiveLotsByFarm(int farmId, [FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 20)
        {
            var result = await _service.GetActiveLotsByFarmAsync(farmId, pageIndex, pageSize);
            if (!result.Success) return StatusCode(result.StatusCode, new { result.Errors });

            return StatusCode(result.StatusCode, result.Data);
        }

        [HttpGet("farm-{farmId:int}")]
        public async Task<IActionResult> GetLotsByFarm(int farmId, [FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 20)
        {
            var result = await _service.GetLotsByFarmIdAsync(farmId, pageIndex, pageSize);
            if (!result.Success) return StatusCode(result.StatusCode, new { result.Errors });

            return StatusCode(result.StatusCode, result.Data);
        }

        [HttpGet("{id:int}", Name = "GetLotById")]
        public async Task<IActionResult> GetLotById(int id)
        {
            var result = await _service.GetLotByIdAsync(id);
            if (!result.Success) return StatusCode(result.StatusCode, new { result.Errors });

            return StatusCode(result.StatusCode, result.Data);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateLot(int id, [FromBody] UpdateLotDTO dto)
        {
            var result = await _service.UpdateLotAsync(id, dto);
            if (!result.Success) return StatusCode(result.StatusCode, new { result.Errors });

            return CreatedAtAction("GetLotById", new { id }, null);
        }

        [HttpGet("export")]
        public async Task<IActionResult> ExportAllLots([FromQuery] int? pageIndex, [FromQuery] int? pageSize)
        {
            var stream = await _service.ExportLotsAsync(pageIndex, pageSize);
            if (stream == null) return NotFound();
            var fileName = $"lots_{DateTime.Now:yyyyMMdd}.xlsx";

            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }

        [HttpGet("export/active")]
        public async Task<IActionResult> ExportAllActiveLots([FromQuery] int? pageIndex, [FromQuery] int? pageSize)
        {
            var stream = await _service.ExportActiveLotsAsync(pageIndex, pageSize);
            if (stream == null) return NotFound();
            var fileName = $"active_lots_{DateTime.Now:yyyyMMdd}.xlsx";

            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }


        [HttpGet("farm-{farmId:int}/export")]
        public async Task<IActionResult> ExportLotsByFarm(int farmId, [FromQuery] int? pageIndex, [FromQuery] int? pageSize)
        {
            var stream = await _service.ExportLotsByFarmAsync(farmId, pageIndex, pageSize);
            if (stream == null) return NotFound();
            var fileName = $"lots_granja_{farmId}_{DateTime.Now:yyyyMMdd}.xlsx";

            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }

        [HttpGet("farm-{farmId:int}/export/active")]
        public async Task<IActionResult> ExportActiveLots(int farmId, [FromQuery] int? pageIndex, [FromQuery] int? pageSize)
        {
            var stream = await _service.ExportActiveLotsByFarmAsync(farmId, pageIndex, pageSize);
            if (stream == null) return NotFound();
            var fileName = $"active_lots_granja_{farmId}_{DateTime.Now:yyyyMMdd}.xlsx";

            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }

        [HttpGet("{id:int}/export")]
        public async Task<IActionResult> ExportLotById(int id)
        {
            var stream = await _service.ExportLotByIdAsync(id);
            if (stream == null) return NotFound();
            var fileName = $"lots_{id}_{DateTime.Now:yyyyMMdd}.xlsx";

            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }
    }
}