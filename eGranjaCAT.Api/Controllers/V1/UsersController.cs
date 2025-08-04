using Asp.Versioning;
using eGranjaCAT.Api.Extensions;
using eGranjaCAT.Application.DTOs.User;
using eGranjaCAT.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace eGranjaCAT.Api.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/users")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _service;

        public UsersController(IUserService userService)
        {
            _service = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> CreateUserAsync(CreateUserDTO userDTO)
        {
            var result = await _service.CreateUserAsync(userDTO);
            if (!result.Success) return StatusCode(result.StatusCode, new { result.Errors });

            return StatusCode(result.StatusCode, result.Data);
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginUserAsync(LoginUserDTO userDTO)
        {
            var result = await _service.LoginUserAsync(userDTO);
            if (!result.Success) return StatusCode(result.StatusCode, new { result.Errors });

            return StatusCode(result.StatusCode, result.Data);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetUsersAsync()
        {
            var result = await _service.GetUsersAsync();
            if (!result.Success) return StatusCode(result.StatusCode, new { result.Errors });

            return StatusCode(result.StatusCode, result.Data);
        }

        [HttpGet("{id:int}")]
        [Authorize]
        public async Task<IActionResult> GetUserByIdAsync(Guid id)
        {
            var userGuid = Guid.Parse(User.GetUserId());

            bool isAdmin = User.IsInRole("Admin");
            if (!isAdmin && id != userGuid) return Forbid("No tens permís per veure aquesta informació");

            var result = await _service.GetUserByIdAsync(id);
            if (!result.Success) return StatusCode(result.StatusCode, new { result.Errors });

            return StatusCode(result.StatusCode, result.Data);
        }


        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUserById(Guid id)
        {
            var result = await _service.DeleteUserById(id);
            if (!result.Success) return StatusCode(result.StatusCode, new { result.Errors });

            return StatusCode(result.StatusCode, result.Data);
        }
    }
}