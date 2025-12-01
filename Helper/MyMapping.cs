using AutoMapper;
using E_Com_Monolithic.Dtos;
using E_Com_Monolithic.Models;

namespace E_Com_Monolithic.Helper
{
    public class MyMapping : Profile
    {
        public MyMapping()
        {
            CreateMap<ProductDto, Product>();
            CreateMap<Product, ProductDto>();
        }
    }
}