using AutoMapper;
using Baseline.Services.ProductAPI.Models;
using Baseline.Services.ProductAPI.Models.Dto;

namespace Baseline.Services.ProductAPI
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<ProductRequestDto, Product>().ReverseMap();
            });

            return mappingConfig;
        }
    }
}
