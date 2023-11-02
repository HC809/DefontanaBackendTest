using AutoMapper;
using DefontanaBackendTest.BLL.DTOs;
using DefontanaBackendTest.BLL.Services.Interfaces;
using DefontanaBackendTest.DAL;
using DefontanaBackendTest.DL.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DefontanaBackendTest.BLL.Services
{
    public class VentasService : IVentasService
    {
        private readonly IGenericRepository<Venta> _ventasRepository;
        private readonly IMapper _mapper;

        public VentasService(IGenericRepository<Venta> ventasRepository, IMapper mapper)
        {
            _ventasRepository = ventasRepository;
            _mapper = mapper;
        }

        public async Task<ConsultaVentasDTO> ConsultaVentas()
        {
            List<VentaDTO> ventas = _mapper.Map<List<VentaDTO>>(await _ventasRepository.GetAllAsync());

            DateTime fechaInicio = DateTime.Now.AddDays(-30);

            var resultado = ventas.Where(v => v.Fecha >= fechaInicio && v.Fecha <= DateTime.Now)
                .GroupBy(v => 1)
                .Select(c => new ConsultaVentasDTO
                {
                    MontoTotalVentasUltimos30Dias = c.Sum(v => v.Total).ToString("N2", CultureInfo.InvariantCulture),
                    CantidadTotalVentasUltimos30Dias = c.Count()
                })
                .FirstOrDefault();

            return resultado ?? new ConsultaVentasDTO();
        }
    }
}
