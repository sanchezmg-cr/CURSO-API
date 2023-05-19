using _06_Inventory.Api.DTO;
using _06_Inventory.Api.Models;
using AutoMapper;

namespace _06_Inventory.Api.Mapper
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            CreateMap<ARTICULOS, ArticulosDTO>()
            .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.CODIGO))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.DESCRIPCION))
            .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.CATEGORIA))
            .ForMember(dest => dest.Brand, opt => opt.MapFrom(src => src.MARCA))
            .ForMember(dest => dest.Weight, opt => opt.MapFrom(src => src.PESO))
            .ForMember(dest => dest.BarCode, opt => opt.MapFrom(src => src.CODIGO_BARRAS))
            .ForMember(dest => dest.CreateDate, opt => opt.MapFrom(src => src.CREACION_TSTAMP))
            .ForMember(dest => dest.CreateUser, opt => opt.MapFrom(src => src.CREACION_USUARIO))
            .ForMember(dest => dest.UpdateDate, opt => opt.MapFrom(src => src.ULT_MODIF_TSTAMP))
            .ForMember(dest => dest.UpdateUser, opt => opt.MapFrom(src => src.ULT_MODIF_USUARIO))
            .ReverseMap();

            CreateMap<CATEGORIA, CategoriaDTO>()
            .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.CODIGO))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.DESCRIPCION))
            .ForMember(dest => dest.CreateDate, opt => opt.MapFrom(src => src.CREACION_TSTAMP))
            .ForMember(dest => dest.CreateUser, opt => opt.MapFrom(src => src.CREACION_USUARIO))
            .ForMember(dest => dest.UpdateDate, opt => opt.MapFrom(src => src.ULT_MODIF_TSTAMP))
            .ForMember(dest => dest.UpdateUser, opt => opt.MapFrom(src => src.ULT_MODIF_USUARIO))
            .ReverseMap();
        }
    }
}
