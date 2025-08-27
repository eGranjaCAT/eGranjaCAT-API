using Asp.Versioning;
using eGranjaCAT.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace eGranjaCAT.Api.Controllers.V1
{
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/stats")]
    public class StatsController : ControllerBase
    {
        private readonly IStatsService _service;

        public StatsController(IStatsService statsService)
        {
            _service = statsService;
        }


        [HttpGet("total")]
        public async Task<IActionResult> GetTotalStatsAsync()
        {
            var stats = await _service.CountTotalRecordsAsync();
            return Ok(stats);
        }

        [HttpGet("recent/{days}")]
        public async Task<IActionResult> GetRecentStatsAsync(int days)
        {
            if (days <= 0) return BadRequest("El nombre de dies ha de ser positiu");
            var stats = await _service.CountRecentRecordsAsync(days);
            return Ok(stats);
        }

        [HttpGet("users")]
        public async Task<IActionResult> GetUserStatsAsync()
        {
            var stats = await _service.CountTotalUsersAsync();
            return Ok(stats);
        }

        [HttpGet("lots")]
        public async Task<IActionResult> GetLotStatsAsync()
        {
            var stats = await _service.CountTotalActiveLotsAsync();
            return Ok(stats);
        }

        [HttpGet("farms")]
        public async Task<IActionResult> GetFarmStatsAsync()
        {
            var stats = await _service.CountTotalFarmsAsync();
            return Ok(stats);
        }
    }
}