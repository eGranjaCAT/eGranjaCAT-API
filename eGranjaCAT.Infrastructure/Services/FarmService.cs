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
            _userManager = userManager;
        }

        public async Task<ServiceResult<PagedResult<GetFarmDTO>>> GetFarmsAsync(int pageIndex, int pageSize)
        {
            try
            {
                var query = _context.Farms;
                var totalCount = await query.CountAsync();

                var farms = await query.OrderBy(e => e.Id).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();

                var userGuids = farms.Select(l => l.UserGuid).Distinct().ToList();
                var users = await _userManager.Users.Where(u => userGuids.Contains(u.Id)).ToListAsync();

                var userDtos = _mapper.Map<List<GetUserDTO>>(users);
                var userDict = userDtos.ToDictionary(u => u.Id!, StringComparer.OrdinalIgnoreCase);

                var farmsDTOs = _mapper.Map<List<GetFarmDTO>>(farms, opt =>
                {
                    opt.Items["UserDict"] = userDict;
                });

                var pagedResult = new PagedResult<GetFarmDTO>
                {
                    Items = farmsDTOs,
                    TotalCount = totalCount,
                    PageIndex = pageIndex,
                    PageSize = pageSize
                };

                return ServiceResult<PagedResult<GetFarmDTO>>.Ok(pagedResult);
            }
            catch (Exception ex)
            {
                return ServiceResult<PagedResult<GetFarmDTO>>.FromException(ex);
            }
        }

        public async Task<ServiceResult<int?>> CreateFarmAsync(CreateFarmDTO createFarmDTO, string userId)
        {
            try
            {
                var farm = _mapper.Map<Farm>(createFarmDTO);
                farm.CreatedAt = DateTime.UtcNow;
                farm.UserGuid = userId;

                await _context.Farms.AddAsync(farm);
                await _context.SaveChangesAsync();

                return ServiceResult<int?>.Ok(farm.Id, 201);
            }
            catch (Exception ex)
            {
                return ServiceResult<int?>.FromException(ex);
            }
        }

        public async Task<ServiceResult<GetFarmDTO?>> GetFarmByIdAsync(int id)
        {
            try
            {
                var farm = await _context.Farms.FindAsync(id);
                if (farm == null) return ServiceResult<GetFarmDTO?>.Fail($"Granja {id} no trobada");

                var user = await _userManager.FindByIdAsync(farm.UserGuid);
                var userDTO = _mapper.Map<GetUserDTO>(user);

                var farmDto = _mapper.Map<GetFarmDTO>(farm);
                farmDto.User = userDTO;

                return ServiceResult<GetFarmDTO?>.Ok(farmDto);
            }
            catch (Exception ex)
            {
                return ServiceResult<GetFarmDTO?>.FromException(ex);
            }
        }

        public async Task<ServiceResult<bool>> DeleteFarmAsync(int id)
        {
            try
            {
                var farm = await _context.Farms.FindAsync(id);
                if (farm == null) return ServiceResult<bool>.Fail($"Granja {id} no trobada", 404);

                _context.Farms.Remove(farm);
                await _context.SaveChangesAsync();

                return ServiceResult<bool>.Ok(true, 204);
            }
            catch (Exception ex)
            {
                return ServiceResult<bool>.FromException(ex);
            }
        }
    }
}