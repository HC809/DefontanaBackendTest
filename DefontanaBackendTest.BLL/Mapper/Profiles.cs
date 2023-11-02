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
        }
    }
}
