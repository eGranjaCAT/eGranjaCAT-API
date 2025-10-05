using eGranjaCAT.Application.Common;
using eGranjaCAT.Application.DTOs.Auth;
using eGranjaCAT.Application.DTOs.User;

namespace eGranjaCAT.Infrastructure.Services
{
    public interface IUserService
    {
        Task<ServiceResult<AuthResponseDTO>> CreateUserAsync(CreateUserDTO userDTO);
        Task<ServiceResult<bool>> DeleteUserById(Guid id);
        Task<ServiceResult<GetUserDTO?>> GetUserByIdAsync(Guid id);
        Task<ServiceResult<PagedResult<GetUserDTO>>> GetUsersAsync(int pageIndex, int pageSize);
        Task<ServiceResult<AuthResponseDTO>> LoginUserAsync(LoginUserDTO loginDTO);
    }
}