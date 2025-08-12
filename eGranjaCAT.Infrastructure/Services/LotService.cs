using AutoMapper;
using eGranjaCAT.Application.Common;
using eGranjaCAT.Application.DTOs.Lot;
using eGranjaCAT.Application.DTOs.User;
using eGranjaCAT.Domain.Entities;
using eGranjaCAT.Infrastructure.Data;
using eGranjaCAT.Infrastructure.ExportMappings;
using eGranjaCAT.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


namespace eGranjaCAT.Infrastructure.Services
{
    public class LotService : ILotService
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _context;
        private readonly IExcelService _excelService;

        public LotService(UserManager<User> userManager, IMapper mapper, ApplicationDbContext context, IExcelService excelService)
        {
            _userManager = userManager;
            _mapper = mapper;
            _context = context;
            _excelService = excelService;
        }


        public async Task<ServiceResult<int?>> CreateLotAsync(int farmId, string userId, CreateLotDTO createLotDTO)
        {
            var farmExists = await _context.Farms.AnyAsync(f => f.Id == farmId);
            if (!farmExists) return ServiceResult<int?>.Fail($"La granja {farmId} no existeix.");

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return ServiceResult<int?>.Fail($"Usuari amb ID {userId} no trobat.");

            var lot = _mapper.Map<Lot>(createLotDTO);
            lot.FarmId = farmId;
            lot.UserGuid = user.Id;
            lot.CreatedAt = DateTime.UtcNow;

            await _context.Lots.AddAsync(lot);
            await _context.SaveChangesAsync();

            return ServiceResult<int?>.Ok(lot.Id, 201);
        }


        public async Task<ServiceResult<PagedResult<GetLotDTO>>> GetActiveLotsByFarmAsync(int farmId, int pageIndex, int pageSize)
        {
            var farm = await _context.Farms.FirstOrDefaultAsync(f => f.Id == farmId);
            if (farm == null) return ServiceResult<PagedResult<GetLotDTO>>.Fail($"Farm with ID {farmId} not found");

            var query = _context.Lots.Include(l => l.Farm).Where(l => l.FarmId == farmId && l.Active);
            var totalCount = await query.CountAsync();

            var lots = await query.OrderBy(l => l.Id).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();

            var userGuids = lots.Select(l => l.UserGuid).Distinct().ToList();
            var users = await _userManager.Users.Where(u => userGuids.Contains(u.Id)).ToListAsync();

            var userDtos = _mapper.Map<List<GetUserDTO>>(users);
            var userDict = userDtos.ToDictionary(u => u.Id!, StringComparer.OrdinalIgnoreCase);

            var lotsDTO = _mapper.Map<List<GetLotDTO>>(lots, opt =>
            {
                opt.Items["UserDict"] = userDict;
            });

            var pagedResult = new PagedResult<GetLotDTO>
            {
                Items = lotsDTO,
                TotalCount = totalCount,
                PageIndex = pageIndex,
                PageSize = pageSize
            };

            return ServiceResult<PagedResult<GetLotDTO>>.Ok(pagedResult);
        }


        public async Task<ServiceResult<GetLotDTO>> GetLotByIdAsync(int lotId)
        {
            var lot = await _context.Lots.Include(l => l.Farm).FirstOrDefaultAsync(l => l.Id == lotId);

            var user = await _userManager.FindByIdAsync(lot!.UserGuid);
            var userDTO = _mapper.Map<GetUserDTO>(user);

            if (lot == null) return ServiceResult<GetLotDTO>.Fail($"Lot {lotId} no trobat");

            var lotDTO = _mapper.Map<GetLotDTO>(lot);
            lotDTO.User = userDTO;

            return ServiceResult<GetLotDTO>.Ok(lotDTO);
        }

        public async Task<ServiceResult<PagedResult<GetLotDTO>>> GetLotsByFarmIdAsync(int farmId, int pageIndex, int pageSize)
        {
            var farm = await _context.Farms.FirstOrDefaultAsync(f => f.Id == farmId);
            if (farm == null) return ServiceResult<PagedResult<GetLotDTO>>.Fail($"Farm with ID {farmId} not found");

            var query = _context.Lots.Include(l => l.Farm).Where(l => l.FarmId == farmId);
            var totalCount = await query.CountAsync();

            var lots = await query.OrderBy(l => l.Id).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();

            var userGuids = lots.Select(l => l.UserGuid).Distinct().ToList();
            var users = await _userManager.Users.Where(u => userGuids.Contains(u.Id)).ToListAsync();

            var userDtos = _mapper.Map<List<GetUserDTO>>(users);
            var userDict = userDtos.ToDictionary(u => u.Id!, StringComparer.OrdinalIgnoreCase);

            var lotsDTO = _mapper.Map<List<GetLotDTO>>(lots, opt =>
            {
                opt.Items["UserDict"] = userDict;
            });

            var pagedResult = new PagedResult<GetLotDTO>
            {
                Items = lotsDTO,
                TotalCount = totalCount,
                PageIndex = pageIndex,
                PageSize = pageSize
            };

            return ServiceResult<PagedResult<GetLotDTO>>.Ok(pagedResult);
        }


        public async Task<ServiceResult<bool>> UpdateLotAsync(int farmId, int lotId, UpdateLotDTO dto)
        {
            var result = new ServiceResult<bool>();
            var lot = await _context.Lots.FirstOrDefaultAsync(l => l.Id == lotId && l.FarmId == farmId);
            if (lot == null) return ServiceResult<bool>.Fail($"Lot {lotId} no trobat a la granja {farmId}");

            _mapper.Map(dto, lot);
            lot.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return ServiceResult<bool>.Ok(true, 204);
        }

        public async Task<ServiceResult<bool>> DeleteLotAsync(int lotId)
        {
            var result = new ServiceResult<bool>();
            var lot = await _context.Lots.FirstOrDefaultAsync(l => l.Id == lotId);
            if (lot == null) return ServiceResult<bool>.Fail($"Lot {lotId} no trobat");

            _context.Lots.Remove(lot);
            await _context.SaveChangesAsync();

            return ServiceResult<bool>.Ok(true, 204);
        }

        public async Task<MemoryStream> ExportLotsAsync()
        {
            var lots = await _context.Lots.Include(l => l.Farm).ToListAsync();

            return await _excelService.GenerateExcelAsync(lots, ExcelColumnMappings.LotExcelColumnMappings, $"Lots - {DateTime.Today:yyyyMMdd}");
        }

        public async Task<MemoryStream> ExportActiveLotsAsync()
        {
            var lots = await _context.Lots.Include(l => l.Farm).Where(l => l.Active).ToListAsync();

            return await _excelService.GenerateExcelAsync(lots, ExcelColumnMappings.LotExcelColumnMappings, $"Lots - {DateTime.Today:yyyyMMdd}");
        }

        public async Task<MemoryStream> ExportActiveLotsByFarmAsync(int farmId)
        {
            var lots = await _context.Lots.Include(l => l.Farm).Where(l => l.FarmId == farmId && l.Active).ToListAsync();

            return await _excelService.GenerateExcelAsync(lots, ExcelColumnMappings.LotExcelColumnMappings, $"Lots (Granja {farmId}) - {DateTime.Today:yyyyMMdd}");
        }

        public async Task<MemoryStream> ExportLotsByFarmAsync(int farmId)
        {
            var lots = await _context.Lots.Include(l => l.Farm).Where(l => l.FarmId == farmId).ToListAsync();

            return await _excelService.GenerateExcelAsync(lots, ExcelColumnMappings.LotExcelColumnMappings, $"Lots (Granja {farmId}) - {DateTime.Today:yyyyMMdd}");
        }

        public async Task<MemoryStream> ExportLotByIdAsync(int lotId)
        {
            var lots = await _context.Lots.Include(l => l.Farm).Where(l => l.Id == lotId).ToListAsync();

            return await _excelService.GenerateExcelAsync(lots, ExcelColumnMappings.LotExcelColumnMappings, $"Lot {lotId} - {DateTime.Today:yyyyMMdd}");
        }
    }
}