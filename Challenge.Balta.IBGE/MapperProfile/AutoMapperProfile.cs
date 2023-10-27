using AutoMapper;
using Challenge.Balta.IBGE.Model;
using Chanllenge.Balta.IBGE.Domain.Entities;

namespace Challenge.Balta.IBGE.MapperProfile
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<IbgeModel, Ibge>();
            CreateMap<Ibge, IbgeModel>();
        }
    }
}
