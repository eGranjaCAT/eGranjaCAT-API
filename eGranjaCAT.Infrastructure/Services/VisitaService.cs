using AutoMapper;
using eGranjaCAT.Application.Common;
using eGranjaCAT.Application.DTOs.Entrada;
using eGranjaCAT.Application.DTOs.User;
using eGranjaCAT.Application.DTOs.Visites;
using eGranjaCAT.Domain.Entities;
using eGranjaCAT.Infrastructure.Data;
using eGranjaCAT.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;


namespace eGranjaCAT.Infrastructure.Services
{
    public class VisitaService : IVisitaService
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _context;
        private readonly ILogger<EntradaService> _logger;
        private readonly IExcelService _excelService;

        public VisitaService(UserManager<User> userManager, IMapper mapper, ApplicationDbContext context, ILogger<EntradaService> logger, IExcelService excelService)
        {
            _userManager = userManager;
            _mapper = mapper;
            _context = context;
            _logger = logger;
            _excelService = excelService;
        }


        public async Task<ServiceResult<int?>> CreateVisitaAsync(int farmId, string userId, CreateVisitaDTO createVisitaDTO)
        {
            try
            {
                var farmExists = await _context.Farms.AnyAsync(f => f.Id == farmId);
                if (!farmExists) return ServiceResult<int?>.Fail($"La granja {farmId} no existeix");

                var user = await _userManager.FindByIdAsync(userId.ToString());
                if (user == null) return ServiceResult<int?>.Fail($"Usuari amb ID {userId} no trobat");

                var visita = _mapper.Map<Visita>(createVisitaDTO);
                visita.FarmId = farmId;
                visita.CreatedAt = DateTime.UtcNow;
                visita.UserGuid = userId;

                await _context.Visites.AddAsync(visita);
                await _context.SaveChangesAsync();

                return ServiceResult<int?>.Ok(visita.Id, 201);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear la visita");
                return ServiceResult<int?>.FromException(ex);
            }
        }


        public async Task<ServiceResult<PagedResult<GetVisitaDTO>>> GetVisitesByFarmAsync(int farmId, int pageIndex, int pageSize)
        {
            try
            {
                var farmExists = await _context.Farms.AnyAsync(f => f.Id == farmId);
                if (!farmExists) return ServiceResult<PagedResult<GetVisitaDTO>>.Fail($"La granja {farmId} no existeix");

                var query = _context.Visites.Include(e => e.Farm).Where(e => e.FarmId == farmId);
                var totalCount = await query.CountAsync();

                var visites = await query.OrderBy(e => e.Id).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();

                var userGuids = visites.Select(l => l.UserGuid).Distinct().ToList();
                var users = await _userManager.Users.Where(u => userGuids.Contains(u.Id)).ToListAsync();

                var userDtos = _mapper.Map<List<GetUserDTO>>(users);
                var userDict = userDtos.ToDictionary(u => u.Id!, StringComparer.OrdinalIgnoreCase);

                var visitesDTOs = _mapper.Map<List<GetVisitaDTO>>(visites, opt =>
                {
                    opt.Items["UserDict"] = userDict;
                });

                var pagedResult = new PagedResult<GetVisitaDTO>
                {
                    Items = visitesDTOs,
                    TotalCount = totalCount,
                    PageIndex = pageIndex,
                    PageSize = pageSize
                };

                return ServiceResult<PagedResult<GetVisitaDTO>>.Ok(pagedResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtenir les visites");
                return ServiceResult<PagedResult<GetVisitaDTO>>.FromException(ex);
            }
        }


        public async Task<ServiceResult<PagedResult<GetVisitaDTO>>> GetVisitesAsync(int pageIndex, int pageSize)
        {
            try
            {
                var query = _context.Visites.Include(e => e.Farm);
                var totalCount = await query.CountAsync();

                var visites = await query.OrderBy(e => e.Id).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();

                var userGuids = visites.Select(l => l.UserGuid).Distinct().ToList();
                var users = await _userManager.Users.Where(u => userGuids.Contains(u.Id)).ToListAsync();

                var userDtos = _mapper.Map<List<GetUserDTO>>(users);
                var userDict = userDtos.ToDictionary(u => u.Id!, StringComparer.OrdinalIgnoreCase);

                var visitesDTOs = _mapper.Map<List<GetVisitaDTO>>(visites, opt =>
                {
                    opt.Items["UserDict"] = userDict;
                });

                var pagedResult = new PagedResult<GetVisitaDTO>
                {
                    Items = visitesDTOs,
                    TotalCount = totalCount,
                    PageIndex = pageIndex,
                    PageSize = pageSize
                };

                return ServiceResult<PagedResult<GetVisitaDTO>>.Ok(pagedResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtenir les visites");
                return ServiceResult<PagedResult<GetVisitaDTO>>.FromException(ex);
            }
        }


        public async Task<ServiceResult<GetVisitaDTO?>> GetVisitaByIdAsync(int visitaId)
        {
            try
            {
                var visita = await _context.Visites.Include(l => l.Farm).FirstOrDefaultAsync(v => v.Id == visitaId);
                if (visita == null) return ServiceResult<GetVisitaDTO?>.Fail($"Visita amb ID {visitaId} no trobada", 404);

                var visitaDTO = _mapper.Map<GetVisitaDTO>(visita);
                return ServiceResult<GetVisitaDTO?>.Ok(visitaDTO, 200);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtenir la visita");
                return ServiceResult<GetVisitaDTO?>.FromException(ex);
            }
        }


        public async Task<ServiceResult<bool>> UpdateVisitaAsync(int visitaId, UpdateVisitaDTO updateVisitaDTO)
        {
            try
            {
                var visita = await _context.Visites.Where(e => e.Id == visitaId).FirstOrDefaultAsync();
                if (visita == null) return ServiceResult<bool>.Fail("Visita no trobada", 404);

                _mapper.Map(updateVisitaDTO, visita);
                visita.UpdatedAt = DateTime.UtcNow;
                visita.Updated = true;

                _context.Visites.Update(visita);
                await _context.SaveChangesAsync();

                return ServiceResult<bool>.Ok(true, 200);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualitzar la visita");
                return ServiceResult<bool>.FromException(ex);
            }
        }


        public async Task<ServiceResult<bool>> DeleteVisitaAsync(int visitaId)
        {
            try
            {
                var visita = await _context.Visites.Where(e => e.Id == visitaId).FirstOrDefaultAsync();
                if (visita == null) return ServiceResult<bool>.Fail("Visita no trobada", 404);

                _context.Visites.Remove(visita);
                await _context.SaveChangesAsync();

                return ServiceResult<bool>.Ok(true, 204);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar la visita");
                return ServiceResult<bool>.FromException(ex);
            }
        }


        public async Task<MemoryStream> ExportVisitesAsync()
        {
            var visites = await _context.Visites.Include(l => l.Farm).ToListAsync();
            // return await _excelService.GenerateExcelAsync(visites, ExcelColumnMappings.EntradaExcelColumnMappings, $"Entrades - {DateTime.Today:yyyyMMdd}");
            return null;
        }

        public async Task<MemoryStream> ExportVisitesByFarmAsync(int farmId)
        {
            var visites = await _context.Visites.Include(l => l.Farm).Where(e => e.FarmId == farmId).ToListAsync();
            // return await _excelService.GenerateExcelAsync(visites, ExcelColumnMappings.EntradaExcelColumnMappings, $"Entrades (Granja {farmId}) - {DateTime.Today:yyyyMMdd}");
            return null;
        }

        public async Task<MemoryStream> ExportVisitesByIdAsync(int visitaId)
        {
            var visites = await _context.Visites.Include(l => l.Farm).Where(e => e.Id == visitaId).ToListAsync();
            // return await _excelService.GenerateExcelAsync(entrades, ExcelColumnMappings.EntradaExcelColumnMappings, $"Entrada {visitaId} - {DateTime.Today:yyyyMMdd}");
            return null;
        }
    }
}
