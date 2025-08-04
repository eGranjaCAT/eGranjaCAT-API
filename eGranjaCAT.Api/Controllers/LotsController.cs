using eGranjaCAT.Api.Extensions;
using eGranjaCAT.Application.DTOs.Lot;
using eGranjaCAT.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace eGranjaCAT.Api.Controllers
{
    [Authorize(Policy = "Lots")]
    [ApiController]
    [Route("api/{farmId:int}/lots")]
    public class LotsController : ControllerBase
    {
        private readonly ILotService _service;

        public LotsController(ILotService service)
        {
            _service = service;
        }


        [HttpPost]
        public async Task<IActionResult> CreateLot(int farmId, [FromBody] CreateLotDTO dto)
        {
            var userGuid = User.GetUserId();
            var result = await _service.CreateLotAsync(farmId, userGuid, dto);
            if (!result.Success) return StatusCode(result.StatusCode, new { result.Errors });

            return CreatedAtAction("GetLotById", new { farmId, id = result.Data }, result.Data);
        }

        [HttpGet("active")]
        public async Task<IActionResult> GetActiveLots(int farmId)
        {
            var result = await _service.GetActiveLotsByFarmAsync(farmId);
            if (!result.Success) return StatusCode(result.StatusCode, new { result.Errors });

            return StatusCode(result.StatusCode, result.Data);
        }

        [HttpGet]
        public async Task<IActionResult> GetLots(int farmId)
        {
            var result = await _service.GetLotsByFarmIdAsync(farmId);
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
        public async Task<IActionResult> UpdateLot(int farmId, int id, [FromBody] UpdateLotDTO dto)
        {
            var result = await _service.UpdateLotAsync(farmId, id, dto);
            if (!result.Success) return StatusCode(result.StatusCode, new { result.Errors });

            return CreatedAtAction("GetLotById", new { farmId, id }, result.Data);
        }

        [HttpGet("export-all")]
        public async Task<IActionResult> ExportAllLots()
        {
            var stream = await _service.ExportLotsAsync();
            if (stream == null) return NotFound();
            var fileName = $"lots_{DateTime.Now:yyyyMMdd}.xlsx";

            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }

        [HttpGet("export")]
        public async Task<IActionResult> ExportLots(int farmId)
        {
            var stream = await _service.ExportLotsByFarmAsync(farmId);
            if (stream == null) return NotFound();
            var fileName = $"lots_granja_{farmId}_{DateTime.Now:yyyyMMdd}.xlsx";

            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }

        [HttpGet("export/{id:int}")]
        public async Task<IActionResult> ExportLotById(int id)
        {
            var stream = await _service.ExportLotByIdAsync(id);
            if (stream == null) return NotFound();
            var fileName = $"lots_{id}_{DateTime.Now:yyyyMMdd}.xlsx";

            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }
    }
}