using AutoMapper;
using DefontanaBackendTest.BLL.DTOs;
using DefontanaBackendTest.DL.Entities;

namespace DefontanaBackendTest.BLL.Mapper
{
    public class Profiles : Profile
    {
        public Profiles()
        {
            CreateMap<Venta, VentaDTO>();
            CreateMap<Local, LocalDTO>();
            CreateMap<Producto, ProductoDTO>();
            CreateMap<Marca, MarcaDTO>();
            CreateMap<VentaDetalle, VentaDetalleDTO>()
                .ForMember(dest => dest.ProductoNombre, opt => opt.MapFrom(src => src.IdProductoNavigation.Nombre));

        }
    }
}
