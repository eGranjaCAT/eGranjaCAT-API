using AutoMapper;
using eGranjaCAT.Application.Common;
using eGranjaCAT.Application.DTOs.Entrada;
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
                entrada.User = user!;

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

        public async Task<ServiceResult<List<GetEntradaDTO>>> GetEntradesAsync(int farmId)
        {
            try
            {
                var farmExists = await _context.Farms.AnyAsync(f => f.Id == farmId);
                if (!farmExists) return ServiceResult<List<GetEntradaDTO>>.Fail($"La granja {farmId} no existeix");

                var entrades = await _context.Entrades.Include(l => l.User).Include(l => l.Farm).Include(l => l.Lot).Where(e => e.FarmId == farmId).ToListAsync();
                var entradesDTOs = _mapper.Map<List<GetEntradaDTO>>(entrades);

                return ServiceResult<List<GetEntradaDTO>>.Ok(entradesDTOs, 200);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtenir les entrades");
                return ServiceResult<List<GetEntradaDTO>>.FromException(ex);
            }
        }

        public async Task<ServiceResult<GetEntradaDTO?>> GetEntradaByIdAsync(int entradaId)
        {
            try
            {
                var entrada = await _context.Entrades.Include(l => l.User).Include(l => l.Farm).Include(l => l.Lot).FirstOrDefaultAsync(e => e.Id == entradaId);

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

        public async Task<ServiceResult<bool>> UpdateEntradaAsync(int farmId, int entradaId, UpdateEntradaDTO updateEntradaDTO)
        {
            try
            {
                var farmExists = await _context.Farms.AnyAsync(f => f.Id == farmId);
                if (!farmExists) return ServiceResult<bool>.Fail("La granja no existeix");

                var entrada = await _context.Entrades.Where(e => e.FarmId == farmId && e.Id == entradaId).FirstOrDefaultAsync();
                if (entrada == null) return ServiceResult<bool>.Fail("Entrada no trobada", 404);

                _mapper.Map(updateEntradaDTO, entrada);
                entrada.UpdatedAt = DateTime.UtcNow;

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

        public async Task<ServiceResult<bool>> DeleteEntradaAsync(int farmId, int entradaId)
        {
            try
            {
                var farmExists = await _context.Farms.AnyAsync(f => f.Id == farmId);
                if (!farmExists) return ServiceResult<bool>.Fail("La granja no existeix");

                var entrada = await _context.Entrades.Where(e => e.FarmId == farmId && e.Id == entradaId).FirstOrDefaultAsync();
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


        public async Task<MemoryStream> ExportEntradesAsync()
        {
            var entrades = await _context.Entrades.Include(l => l.User).Include(l => l.Farm).Include(l => l.Lot).ToListAsync();
            return await _excelService.GenerateExcelAsync(entrades, ExcelColumnMappings.EntradaExcelColumnMappings, $"Entrades - {DateTime.Today:yyyyMMdd}");
        }

        public async Task<MemoryStream> ExportEntradesByFarmAsync(int farmId)
        {
            var entrades = await _context.Entrades.Include(l => l.User).Include(l => l.Farm).Include(l => l.Lot).Where(e => e.FarmId == farmId).ToListAsync();
            return await _excelService.GenerateExcelAsync(entrades, ExcelColumnMappings.EntradaExcelColumnMappings, $"Entrades (Granja {farmId}) - {DateTime.Today:yyyyMMdd}");
        }

        public async Task<MemoryStream> ExportEntradaByIdAsync(int entradaId)
        {
            var entrades = await _context.Entrades.Include(l => l.User).Include(l => l.Farm).Include(l => l.Lot).Where(e => e.Id == entradaId).ToListAsync();
            return await _excelService.GenerateExcelAsync(entrades, ExcelColumnMappings.EntradaExcelColumnMappings, $"Entrada {entradaId} - {DateTime.Today:yyyyMMdd}");
        }
    }
}