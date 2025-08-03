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
        private readonly UserManager<User> userManager;
        private readonly IMapper mapper;
        private readonly ApplicationDbContext context;
        private readonly ILogger<EntradaService> logger;
        private readonly IExcelService excelService;

        public EntradaService(UserManager<User> userManager, IMapper mapper, ApplicationDbContext context, ILogger<EntradaService> logger, IExcelService excelService)
        {
            this.userManager = userManager;
            this.mapper = mapper;
            this.context = context;
            this.logger = logger;
            this.excelService = excelService;
        }

        public async Task<ServiceResult<int?>> CreateEntradaAsync(int farmId, string userId, CreateEntradaDTO createEntradaDTO)
        {
            var resultObj = new ServiceResult<int?>();
            try
            {
                var farmExists = await context.Farms.AnyAsync(f => f.Id == farmId);
                if (!farmExists)
                {
                    resultObj.Success = false;
                    resultObj.Errors.Add("La granja no existeix");
                    return resultObj;
                }

                var lotExists = await context.Lots.AnyAsync(l => l.Id == createEntradaDTO.LotId && l.FarmId == farmId);
                if (!lotExists)
                {
                    resultObj.Success = false;
                    resultObj.Errors.Add("El lot especificat no existeix");
                    return resultObj;
                }

                var user = await userManager.FindByIdAsync(userId.ToString());

                var entrada = mapper.Map<Entrada>(createEntradaDTO);
                entrada.FarmId = farmId;
                entrada.CreatedAt = DateTime.UtcNow;
                entrada.UserGuid = userId;
                entrada.User = user!;

                await context.Entrades.AddAsync(entrada);
                await context.SaveChangesAsync();

                resultObj.Success = true;
                resultObj.Data = entrada.Id;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error al crear l'entrada");
                resultObj.Success = false;
                resultObj.Errors.Add("Error inesperat en crear l'entrada");
            }
            return resultObj;
        }

        public async Task<ServiceResult<List<GetEntradaDTO>>> GetEntradesAsync(int farmId)
        {
            var resultObj = new ServiceResult<List<GetEntradaDTO>>();
            try
            {
                var farmExists = await context.Farms.AnyAsync(f => f.Id == farmId);
                if (!farmExists)
                {
                    resultObj.Success = false;
                    resultObj.Errors.Add("La granja no existeix");
                    return resultObj;
                }
                var entrades = await context.Entrades.Include(l => l.User).Include(l => l.Farm).Include(l => l.Lot).Where(e => e.FarmId == farmId).ToListAsync();
                if (entrades == null || !entrades.Any())
                {
                    resultObj.Success = false;
                    resultObj.Errors.Add("No s'han trobat entrades");
                    return resultObj;
                }
                var entradesDTOs = mapper.Map<List<GetEntradaDTO>>(entrades);
                resultObj.Data = entradesDTOs;
                resultObj.Success = true;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error al obtenir les entrades");
                resultObj.Success = false;
                resultObj.Errors.Add("Error inesperat en obtenir les entrades");
            }
            return resultObj;
        }

        public async Task<ServiceResult<GetEntradaDTO?>> GetEntradaByIdAsync(int entradaId)
        {
            var resultObj = new ServiceResult<GetEntradaDTO?>();
            try
            {
                var entrada = await context.Entrades.Include(l => l.User).Include(l => l.Farm).Include(l => l.Lot).FirstOrDefaultAsync(e => e.Id == entradaId);

                if (entrada == null)
                {
                    resultObj.Success = false;
                    resultObj.Errors.Add("Entrada no trobada");
                    return resultObj;
                }
                var entradaDTO = mapper.Map<GetEntradaDTO>(entrada);
                resultObj.Data = entradaDTO;
                resultObj.Success = true;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error al obtenir l'entrada");
                resultObj.Success = false;
                resultObj.Errors.Add("Error inesperat en obtenir l'entrada");
            }
            return resultObj;
        }

        public async Task<ServiceResult<bool>> UpdateEntradaAsync(int farmId, int entradaId, UpdateEntradaDTO updateEntradaDTO)
        {
            var resultObj = new ServiceResult<bool>();
            try
            {
                var farmExists = await context.Farms.AnyAsync(f => f.Id == farmId);
                if (!farmExists)
                {
                    resultObj.Success = false;
                    resultObj.Errors.Add("La granja no existeix");
                    return resultObj;
                }
                var entrada = await context.Entrades.Where(e => e.FarmId == farmId && e.Id == entradaId).FirstOrDefaultAsync();

                if (entrada == null)
                {
                    resultObj.Success = false;
                    resultObj.Errors.Add("Entrada no trobada");
                    return resultObj;
                }

                mapper.Map(updateEntradaDTO, entrada);
                entrada.UpdatedAt = DateTime.UtcNow;
                context.Entrades.Update(entrada);
                await context.SaveChangesAsync();

                resultObj.Success = true;
                resultObj.Data = true;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error al actualitzar l'entrada");
                resultObj.Success = false;
                resultObj.Errors.Add("Error inesperat en actualitzar l'entrada");
            }
            return resultObj;
        }

        public async Task<ServiceResult<bool>> DeleteEntradaAsync(int farmId, int entradaId)
        {
            var resultObj = new ServiceResult<bool>();
            try
            {
                var farmExists = await context.Farms.AnyAsync(f => f.Id == farmId);
                if (!farmExists)
                {
                    resultObj.Success = false;
                    resultObj.Errors.Add("La granja no existeix");
                    return resultObj;
                }
                var entrada = await context.Entrades.Where(e => e.FarmId == farmId && e.Id == entradaId).FirstOrDefaultAsync();

                if (entrada == null)
                {
                    resultObj.Success = false;
                    resultObj.Errors.Add("Entrada no trobada");
                    return resultObj;
                }
                context.Entrades.Remove(entrada);
                await context.SaveChangesAsync();

                resultObj.Success = true;
                resultObj.Data = true;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error al eliminar l'entrada");
                resultObj.Success = false;
                resultObj.Errors.Add("Error inesperat en eliminar l'entrada");
            }
            return resultObj;
        }


        public async Task<MemoryStream> ExportEntradesAsync()
        {
            var entrades = await context.Entrades.Include(l => l.User).Include(l => l.Farm).Include(l => l.Lot).ToListAsync();
            return await excelService.GenerateExcelAsync(entrades, ExcelColumnMappings.EntradaExcelColumnMappings, $"Entrades - {DateTime.Today:yyyyMMdd}");
        }

        public async Task<MemoryStream> ExportEntradesByFarmAsync(int farmId)
        {
            var entrades = await context.Entrades.Include(l => l.User).Include(l => l.Farm).Include(l => l.Lot).Where(e => e.FarmId == farmId).ToListAsync();
            return await excelService.GenerateExcelAsync(entrades, ExcelColumnMappings.EntradaExcelColumnMappings, $"Entrades (Granja {farmId}) - {DateTime.Today:yyyyMMdd}");
        }

        public async Task<MemoryStream> ExportEntradaByIdAsync(int entradaId)
        {
            var entrades = await context.Entrades.Include(l => l.User).Include(l => l.Farm).Include(l => l.Lot).Where(e => e.Id == entradaId).ToListAsync();
            return await excelService.GenerateExcelAsync(entrades, ExcelColumnMappings.EntradaExcelColumnMappings, $"Entrada {entradaId} - {DateTime.Today:yyyyMMdd}");
        }
    }
}