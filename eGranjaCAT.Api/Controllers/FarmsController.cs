using eGranjaCAT.Application.DTOs.Farm;
using eGranjaCAT.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace eGranjaCAT.Api.Controllers
{
    [ApiController]
    [Route("api/farms")]
    public class FarmsController : ControllerBase
    {
        private readonly IFarmService farmService;

        public FarmsController(IFarmService farmService)
        {
            this.farmService = farmService;
        }


        [HttpGet]
        [Authorize(Policy = "Farms")]
        public async Task<IActionResult> GetFarmsAsync()
        {
            var result = await farmService.GetFarmsAsync();
            if (!result.Success) return BadRequest(new { result.Errors });
            return Ok(result.Data);
        }

        [HttpGet("{id:int}", Name = "GetFarmById")]
        [Authorize(Policy = "Farms")]
        public async Task<IActionResult> GetFarmByIdAsync(int id)
        {
            var result = await farmService.GetFarmByIdAsync(id);
            if (!result.Success) return BadRequest(new { result.Errors });

            return Ok(result.Data);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateFarmAsync([FromBody] CreateFarmDTO createFarmDTO)
        {
            var result = await farmService.CreateFarmAsync(createFarmDTO);
            if (!result.Success) return BadRequest(new { result.Errors });

            return CreatedAtRoute("GetFarmById", new { id = result.Data }, null);
        }


        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteFarmAsync(int id)
        {
            var result = await farmService.DeleteFarmAsync(id);
            if (!result.Success) return BadRequest(new { result.Errors });

            return NoContent();
        }
    }
}