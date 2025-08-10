using AutoMapper;
using eGranjaCAT.Application.Common;
using eGranjaCAT.Application.DTOs.Farm;
using eGranjaCAT.Application.DTOs.User;
using eGranjaCAT.Application.Entities;
using eGranjaCAT.Infrastructure.Data;
using eGranjaCAT.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


namespace eGranjaCAT.Infrastructure.Services
{
    public class FarmService : IFarmService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;

        public FarmService(ApplicationDbContext context, IMapper mapper, UserManager<User> userManager)
        {
            _context = context;
            _mapper = mapper;
            this._userManager = userManager;
        }

        public async Task<ServiceResult<List<GetFarmDTO>>> GetFarmsAsync()
        {
            var farms = await _context.Farms.ToListAsync();
            var farmsDTOs = _mapper.Map<List<GetFarmDTO>>(farms);

            return ServiceResult<List<GetFarmDTO>>.Ok(farmsDTOs);
        }

        public async Task<ServiceResult<int?>> CreateFarmAsync(CreateFarmDTO createFarmDTO, string userId)
        {
            var farm = _mapper.Map<Farm>(createFarmDTO);
            farm.CreatedAt = DateTime.UtcNow;
            farm.UserGuid = userId;

            await _context.Farms.AddAsync(farm);
            await _context.SaveChangesAsync();

            return ServiceResult<int?>.Ok(farm.Id, 201);
        }

        public async Task<ServiceResult<GetFarmDTO?>> GetFarmByIdAsync(int id)
        {
            var farm = await _context.Farms.FindAsync(id);
            if (farm == null) return ServiceResult<GetFarmDTO?>.Fail($"Granja {id} no trobada");

            var user = await _userManager.FindByIdAsync(farm.UserGuid);
            var userDTO = _mapper.Map<GetUserDTO>(user);

            var farmDto = _mapper.Map<GetFarmDTO>(farm);
            farmDto.User = userDTO;

            return ServiceResult<GetFarmDTO?>.Ok(farmDto);
        }

        public async Task<ServiceResult<bool>> DeleteFarmAsync(int id)
        {
            var farm = await _context.Farms.FindAsync(id);
            if (farm == null) return ServiceResult<bool>.Fail($"Granja {id} no trobada", 404);

            _context.Farms.Remove(farm);
            await _context.SaveChangesAsync();

            return ServiceResult<bool>.Ok(true, 204);
        }
    }
}