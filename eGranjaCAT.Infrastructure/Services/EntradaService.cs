using AutoMapper;
using eGranjaCAT.Application.Common;
using eGranjaCAT.Application.DTOs.Entrada;
using eGranjaCAT.Application.DTOs.User;
using eGranjaCAT.Domain.Entities;
using eGranjaCAT.Infrastructure.Data;
using eGranjaCAT.Infrastructure.ExportMappings;
using eGranjaCAT.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;


namespace eGranjaCAT.Infrastructure.Services
{
    public class EntradaService : IEntradaService
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _context;
        private readonly ILogger<EntradaService> _logger;
        private readonly IExcelService _excelService;

        public EntradaService(UserManager<User> userManager, IMapper mapper, ApplicationDbContext context, ILogger<EntradaService> logger, IExcelService excelService)
        {
            _userManager = userManager;
            _mapper = mapper;
            _context = context;
            _logger = logger;
            _excelService = excelService;
        }


        public async Task<ServiceResult<int?>> CreateEntradaAsync(int farmId, string userId, CreateEntradaDTO createEntradaDTO)
        {
            try
            {
                var farmExists = await _context.Farms.AnyAsync(f => f.Id == farmId);
                if (!farmExists) return ServiceResult<int?>.Fail($"La granja {farmId} no existeix");

                var lotExists = await _context.Lots.AnyAsync(l => l.Id == createEntradaDTO.LotId && l.FarmId == farmId);
                if (!lotExists) return ServiceResult<int?>.Fail($"Lot {createEntradaDTO.LotId} no existeix");

                var user = await _userManager.FindByIdAsync(userId.ToString());
                if (user == null) return ServiceResult<int?>.Fail($"Usuari amb ID {userId} no trobat");

                var entrada = _mapper.Map<Entrada>(createEntradaDTO);
                entrada.FarmId = farmId;
                entrada.CreatedAt = DateTime.UtcNow;
                entrada.UserGuid = userId;

                await _context.Entrades.AddAsync(entrada);
                await _context.SaveChangesAsync();

                return ServiceResult<int?>.Ok(entrada.Id, 201);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear l'entrada");
                return ServiceResult<int?>.FromException(ex);
            }
        }


        public async Task<ServiceResult<PagedResult<GetEntradaDTO>>> GetEntradesByFarmAsync(int farmId, int pageIndex, int pageSize)
        {
            try
            {
                var farmExists = await _context.Farms.AnyAsync(f => f.Id == farmId);
                if (!farmExists) return ServiceResult<PagedResult<GetEntradaDTO>>.Fail($"La granja {farmId} no existeix");

                var query = _context.Entrades.Include(e => e.Farm).Include(e => e.Lot).Where(e => e.FarmId == farmId);
                var totalCount = await query.CountAsync();

                var entrades = await query.OrderBy(e => e.Id).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();

                var userGuids = entrades.Select(l => l.UserGuid).Distinct().ToList();
                var users = await _userManager.Users.Where(u => userGuids.Contains(u.Id)).ToListAsync();

                var userDtos = _mapper.Map<List<GetUserDTO>>(users);
                var userDict = userDtos.ToDictionary(u => u.Id!, StringComparer.OrdinalIgnoreCase);

                var entradesDTOs = _mapper.Map<List<GetEntradaDTO>>(entrades, opt =>
                {
                    opt.Items["UserDict"] = userDict;
                });

                var pagedResult = new PagedResult<GetEntradaDTO>
                {
                    Items = entradesDTOs,
                    TotalCount = totalCount,
                    PageIndex = pageIndex,
                    PageSize = pageSize
                };

                return ServiceResult<PagedResult<GetEntradaDTO>>.Ok(pagedResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtenir les entrades");
                return ServiceResult<PagedResult<GetEntradaDTO>>.FromException(ex);
            }
        }

        public async Task<ServiceResult<PagedResult<GetEntradaDTO>>> GetEntradesAsync(int pageIndex, int pageSize)
        {
            try
            {

                var query = _context.Entrades.Include(e => e.Farm).Include(e => e.Lot);
                var totalCount = await query.CountAsync();

                var entrades = await query.OrderBy(e => e.Id).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();

                var userGuids = entrades.Select(l => l.UserGuid).Distinct().ToList();
                var users = await _userManager.Users.Where(u => userGuids.Contains(u.Id)).ToListAsync();

                var userDtos = _mapper.Map<List<GetUserDTO>>(users);
                var userDict = userDtos.ToDictionary(u => u.Id!, StringComparer.OrdinalIgnoreCase);

                var entradesDTOs = _mapper.Map<List<GetEntradaDTO>>(entrades, opt =>
                {
                    opt.Items["UserDict"] = userDict;
                });

                var pagedResult = new PagedResult<GetEntradaDTO>
                {
                    Items = entradesDTOs,
                    TotalCount = totalCount,
                    PageIndex = pageIndex,
                    PageSize = pageSize
                };

                return ServiceResult<PagedResult<GetEntradaDTO>>.Ok(pagedResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtenir les entrades");
                return ServiceResult<PagedResult<GetEntradaDTO>>.FromException(ex);
            }
        }

        public async Task<ServiceResult<GetEntradaDTO?>> GetEntradaByIdAsync(int entradaId)
        {
            try
            {
                var entrada = await _context.Entrades.Include(l => l.Farm).Include(l => l.Lot).FirstOrDefaultAsync(e => e.Id == entradaId);

                if (entrada == null) return ServiceResult<GetEntradaDTO?>.Fail($"Entrada amb ID {entradaId} no trobada", 404);

                var entradaDTO = _mapper.Map<GetEntradaDTO>(entrada);
                return ServiceResult<GetEntradaDTO?>.Ok(entradaDTO, 200);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtenir l'entrada");
                return ServiceResult<GetEntradaDTO?>.FromException(ex);
            }
        }

        public async Task<ServiceResult<bool>> UpdateEntradaAsync(int entradaId, UpdateEntradaDTO updateEntradaDTO)
        {
            try
            {
                var entrada = await _context.Entrades.Where(e => e.Id == entradaId).FirstOrDefaultAsync();
                if (entrada == null) return ServiceResult<bool>.Fail("Entrada no trobada", 404);

                _mapper.Map(updateEntradaDTO, entrada);
                entrada.UpdatedAt = DateTime.UtcNow;
                entrada.Updated = true;

                _context.Entrades.Update(entrada);
                await _context.SaveChangesAsync();

                return ServiceResult<bool>.Ok(true, 200);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualitzar l'entrada");
                return ServiceResult<bool>.FromException(ex);
            }
        }

        public async Task<ServiceResult<bool>> DeleteEntradaAsync(int entradaId)
        {
            try
            {
                var entrada = await _context.Entrades.Where(e => e.Id == entradaId).FirstOrDefaultAsync();
                if (entrada == null) return ServiceResult<bool>.Fail("Entrada no trobada", 404);

                _context.Entrades.Remove(entrada);
                await _context.SaveChangesAsync();

                return ServiceResult<bool>.Ok(true, 204);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar l'entrada");
                return ServiceResult<bool>.FromException(ex);
            }
        }


        public async Task<MemoryStream> ExportEntradesAsync(int? pageIndex, int? pageSize)
        {
            var entrades = new List<Entrada>();

            if (pageIndex != null && pageSize != null)
            {
                var query = _context.Entrades.Include(l => l.Farm).Include(l => l.Lot);
                var totalCount = await query.CountAsync();

                entrades = await query.OrderBy(e => e.Id).Skip((pageIndex.Value - 1) * pageSize.Value).Take(pageSize.Value).ToListAsync();
            }
            else entrades = await _context.Entrades.Include(l => l.Farm).Include(l => l.Lot).ToListAsync();

            return await _excelService.GenerateExcelAsync(entrades, ExcelColumnMappings.EntradaExcelColumnMappings, $"Entrades - {DateTime.Today:yyyyMMdd}");
        }

        public async Task<MemoryStream> ExportEntradesByFarmAsync(int farmId, int? pageIndex, int? pageSize)
        {
            var entrades = new List<Entrada>();

            if (pageIndex != null && pageSize != null)
            {
                var query = _context.Entrades.Include(l => l.Farm).Include(l => l.Lot);
                var totalCount = await query.CountAsync();
                entrades = await query.OrderBy(e => e.Id).Skip((pageSize.Value - 1) * pageSize.Value).Take(pageSize.Value).ToListAsync();
            }
            else entrades = await _context.Entrades.Include(l => l.Farm).Include(l => l.Lot).Where(e => e.FarmId == farmId).ToListAsync();

            return await _excelService.GenerateExcelAsync(entrades, ExcelColumnMappings.EntradaExcelColumnMappings, $"Entrades (Granja {farmId}) - {DateTime.Today:yyyyMMdd}");
        }

        public async Task<MemoryStream> ExportEntradaByIdAsync(int entradaId)
        {
            var entrades = await _context.Entrades.Include(l => l.Farm).Include(l => l.Lot).Where(e => e.Id == entradaId).ToListAsync();
            return await _excelService.GenerateExcelAsync(entrades, ExcelColumnMappings.EntradaExcelColumnMappings, $"Entrada {entradaId} - {DateTime.Today:yyyyMMdd}");
        }
    }
}