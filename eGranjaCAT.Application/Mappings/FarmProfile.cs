using AutoMapper;
using eGranjaCAT.Application.Entities;

namespace eGranjaCAT.Application.Mappings
{
    public class FarmProfile : Profile
    {
        public FarmProfile()
        {
            CreateMap<CreateFarmDTO, Farm>();
            CreateMap<Farm, GetFarmDTO>();
        }
    }
}
