using Asp.Versioning;
using eGranjaCAT.Api.Extensions;
using eGranjaCAT.Application.DTOs.Farm;
using eGranjaCAT.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace eGranjaCAT.Api.Controllers.V1
{
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/farms")]
    public class FarmsController : ControllerBase
    {
        private readonly IFarmService _service;

        public FarmsController(IFarmService farmService)
        {
            _service = farmService;
        }


        [HttpGet]
        public async Task<IActionResult> GetFarmsAsync()
        {
            var result = await _service.GetFarmsAsync();
            if (!result.Success) return StatusCode(result.StatusCode, new { result.Errors });

            return StatusCode(result.StatusCode, result.Data);
        }

        [HttpGet("{id:int}", Name = "GetFarmById")]
        public async Task<IActionResult> GetFarmByIdAsync(int id)
        {
            var result = await _service.GetFarmByIdAsync(id);
            if (!result.Success) return StatusCode(result.StatusCode, new { result.Errors });

            return StatusCode(result.StatusCode, result.Data);
        }

        [HttpPost]
        public async Task<IActionResult> CreateFarmAsync([FromBody] CreateFarmDTO createFarmDTO)
        {
            var userGuid = User.GetUserId();
            var result = await _service.CreateFarmAsync(createFarmDTO, userGuid);
            if (!result.Success) return StatusCode(result.StatusCode, new { result.Errors });

            return CreatedAtRoute("GetFarmById", new { id = result.Data }, null);
        }


        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteFarmAsync(int id)
        {
            var result = await _service.DeleteFarmAsync(id);
            if (!result.Success) return StatusCode(result.StatusCode, new { result.Errors });

            return StatusCode(result.StatusCode, result.Data);
        }
    }
}