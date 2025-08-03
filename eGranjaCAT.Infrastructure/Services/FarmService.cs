using AutoMapper;
using eGranjaCAT.Application.Common;
using eGranjaCAT.Application.DTOs.Farm;
using eGranjaCAT.Application.Entities;
using eGranjaCAT.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;


namespace eGranjaCAT.Infrastructure.Services
{
    public class FarmService : IFarmService
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public FarmService(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<ServiceResult<List<GetFarmDTO>>> GetFarmsAsync()
        {
            var resultObj = new ServiceResult<List<GetFarmDTO>>();
            var farms = await context.Farms.ToListAsync();
            var farmsDTOs = mapper.Map<List<GetFarmDTO>>(farms);

            if (farmsDTOs == null || !farmsDTOs.Any())
            {
                resultObj.Success = false;
                resultObj.Errors.Add("No farms found.");
                return resultObj;
            }

            resultObj.Data = farmsDTOs;
            resultObj.Success = true;
            return resultObj;
        }

        public async Task<ServiceResult<int?>> CreateFarmAsync(CreateFarmDTO createFarmDTO)
        {
            var resultObj = new ServiceResult<int?>();
            var farm = mapper.Map<Farm>(createFarmDTO);

            farm.CreatedAt = DateTime.UtcNow;
            farm.UpdatedAt = DateTime.UtcNow;

            await context.Farms.AddAsync(farm);
            await context.SaveChangesAsync();

            resultObj.Success = true;
            resultObj.Data = farm.Id;
            return resultObj;
        }

        public async Task<ServiceResult<GetFarmDTO?>> GetFarmByIdAsync(int id)
        {
            var resultObj = new ServiceResult<GetFarmDTO?>();

            var farm = await context.Farms.FindAsync(id);
            if (farm == null)
            {
                resultObj.Success = false;
                resultObj.Errors.Add($"Farm with ID {id} not found.");
                return resultObj;
            }

            var farmDto = mapper.Map<GetFarmDTO>(farm);
            resultObj.Data = farmDto;
            resultObj.Success = true;
            return resultObj;
        }

        public async Task<ServiceResult<bool>> DeleteFarmAsync(int id)
        {
            var resultObj = new ServiceResult<bool>();

            var farm = await context.Farms.FindAsync(id);
            if (farm == null)
            {
                resultObj.Success = false;
                resultObj.Errors.Add($"Farm with ID {id} not found.");
                return resultObj;
            }

            context.Farms.Remove(farm);
            await context.SaveChangesAsync();

            resultObj.Success = true;
            resultObj.Data = true;
            return resultObj;
        }
    }
}