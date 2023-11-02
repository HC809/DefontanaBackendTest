using AutoMapper;
using DefontanaBackendTest.BLL.DTOs;
using DefontanaBackendTest.BLL.Services.Interfaces;
using DefontanaBackendTest.DAL;
using DefontanaBackendTest.DL.Entities;
using System.Globalization;

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

            // Consulta 1
            DateTime fechaInicio = DateTime.Now.AddDays(-30);
            DateTime fechaFin = DateTime.Now;

            List<VentaDTO> ventasUltimos30Dias = ventas.Where(v => v.Fecha >= fechaInicio && v.Fecha <= fechaFin).ToList();

            Consulta1VentasDTO ventasConsulta1 = new Consulta1VentasDTO();

            if (ventasUltimos30Dias.Any())
            {
                ventasConsulta1.MontoTotalVentasUltimos30Dias = ventasUltimos30Dias.Sum(v => v.Total).ToString("N2", CultureInfo.InvariantCulture);
                ventasConsulta1.CantidadTotalVentasUltimos30Dias = ventasUltimos30Dias.Count();
            }
            else
            {
                ventasConsulta1.MontoTotalVentasUltimos30Dias = "0.00";
                ventasConsulta1.CantidadTotalVentasUltimos30Dias = 0;
            }

            // Consulta 2
            VentaDTO ventaMasAlta = _mapper.Map<VentaDTO>(ventas.OrderByDescending(v => v.Total).FirstOrDefault());
            Consulta2VentasDTO ventasConsulta2 = new Consulta2VentasDTO();
            if (ventaMasAlta != null)
            {
                ventasConsulta2.DiaVentaMasAlta = ventaMasAlta?.Fecha.ToString("dd 'de' MMMM 'del' yyyy", new CultureInfo("es-ES"));
                ventasConsulta2.HoraVentaMasAlta = ventaMasAlta?.Fecha.ToString("HH:mm:ss");
                ventasConsulta2.MontoVentaMasAlta = ventaMasAlta?.Total.ToString("N2", CultureInfo.InvariantCulture);
            }

            var resultado = new ConsultaVentasDTO
            {
                Consulta1 = ventasConsulta1,
                Consulta2 = ventasConsulta2

            };

            return resultado;
        }
    }
}
