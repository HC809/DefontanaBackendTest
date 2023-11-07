using AutoMapper;
using DefontanaBackendTest.BLL.DTOs;
using DefontanaBackendTest.BLL.Services.Interfaces;
using DefontanaBackendTest.DAL;
using DefontanaBackendTest.DL.Entities;
using System.Globalization;

namespace DefontanaBackendTest.BLL.Services
{
    public class ConsultasService : IConsultasService
    {
        private readonly IGenericRepository<Venta> _ventasRepository;
        private readonly IGenericRepository<Local> _localRepository;
        private readonly IGenericRepository<VentaDetalle> _ventaDetalleRepository;
        private readonly IGenericRepository<Producto> _productoRepository;
        private readonly IGenericRepository<Marca> _marcaRepository;
        private readonly IMapper _mapper;

        public ConsultasService(
            IGenericRepository<Venta> ventasRepository,
            IGenericRepository<Local> localRepository,
            IGenericRepository<VentaDetalle> ventaDetalleRepository,
            IGenericRepository<Producto> productoRepository,
            IGenericRepository<Marca> marcaRepository,
            IMapper mapper)
        {
            _ventasRepository = ventasRepository;
            _localRepository = localRepository;
            _ventaDetalleRepository = ventaDetalleRepository;
            _productoRepository = productoRepository;
            _marcaRepository = marcaRepository;
            _mapper = mapper;
        }

        public async Task<ConsultaDTO> ConsultaVentas()
        {
            List<VentaDTO> ventas = _mapper.Map<List<VentaDTO>>(await _ventasRepository.GetAllAsync());
            List<VentaDetalleDTO> ventasDetalle = _mapper.Map<List<VentaDetalleDTO>>(await _ventaDetalleRepository.GetAllAsync());
            List<LocalDTO> locales = _mapper.Map<List<LocalDTO>>(await _localRepository.GetAllAsync());
            List<ProductoDTO> productos = _mapper.Map<List<ProductoDTO>>(await _productoRepository.GetAllAsync());
            List<MarcaDTO> marcas = _mapper.Map<List<MarcaDTO>>(await _marcaRepository.GetAllAsync());

            var resultadoConsultas = new ConsultaDTO
            {
                Consulta1 = ObtenerVentasUltimos30Dias(ventas),
                Consulta2 = ObtenerFechaVentaMasAlta(ventas),
                Consulta3 = ObtenerProductoMasVendido(ventasDetalle, productos),
                Consulta4 = ObtenerLocalConMayorVentas(ventas, locales),
                Consulta5 = ObtenerMarcaConMayorMargen(ventasDetalle, productos, marcas),
                Consulta6 = ObtenerProductoMasVendidoPorLocal(ventas, ventasDetalle, locales, productos)
            };

            return resultadoConsultas;
        }

        //Consulta #1
        private ConsultaVentasUltimos30Dias ObtenerVentasUltimos30Dias(List<VentaDTO> ventas)
        {
            ConsultaVentasUltimos30Dias consulta = new ConsultaVentasUltimos30Dias();

            DateTime fechaInicio = DateTime.Now.AddDays(-30);
            DateTime fechaFin = DateTime.Now;

            List<VentaDTO> ventasUltimos30Dias = ventas.Where(v => v.Fecha >= fechaInicio && v.Fecha <= fechaFin).ToList();

            if (ventasUltimos30Dias.Any())
            {
                consulta.MontoTotalVentasUltimos30Dias = ventasUltimos30Dias.Sum(v => v.Total).ToString("N2", CultureInfo.InvariantCulture);
                consulta.CantidadTotalVentasUltimos30Dias = ventasUltimos30Dias.Count();
            }
            else
            {
                consulta.MontoTotalVentasUltimos30Dias = "0.00";
                consulta.CantidadTotalVentasUltimos30Dias = 0;
            }

            return consulta;
        }

        //Consulta #2
        private ConsultaFechaVentaMasAlta ObtenerFechaVentaMasAlta(List<VentaDTO> ventas)
        {
            VentaDTO ventaMasAlta = _mapper.Map<VentaDTO>(ventas.OrderByDescending(v => v.Total).FirstOrDefault());

            ConsultaFechaVentaMasAlta consulta = new ConsultaFechaVentaMasAlta();

            if (ventaMasAlta != null)
            {
                consulta.DiaVentaMasAlta = ventaMasAlta?.Fecha.ToString("dd 'de' MMMM 'del' yyyy", new CultureInfo("es-ES"));
                consulta.HoraVentaMasAlta = ventaMasAlta?.Fecha.ToString("HH:mm:ss");
                consulta.MontoVentaMasAlta = ventaMasAlta?.Total.ToString("N2", CultureInfo.InvariantCulture);
            }
            else
            {
                consulta.DiaVentaMasAlta = "No hay ventas";
                consulta.HoraVentaMasAlta = "N/A";
                consulta.MontoVentaMasAlta = "0.00";
            }

            return consulta;
        }

        //Consulta #3
        private ConsultaProductoMasVendido ObtenerProductoMasVendido(List<VentaDetalleDTO> ventasDetalle, List<ProductoDTO> productos)
        {
            var productosConVentas = from vd in ventasDetalle
                                     join p in productos on vd.IdProducto equals p.IdProducto
                                     select new { vd.TotalLinea, p.Nombre };

            var productoMasVendido = productosConVentas
                .GroupBy(x => x.Nombre)
                .Select(group => new ConsultaProductoMasVendido
                {
                    NombreProductoMasVendido = group.Key,
                    MontoTotalVentasProductoMasVendido = group.Sum(x => x.TotalLinea).ToString("N2", CultureInfo.InvariantCulture)
                })
                .OrderByDescending(dto => int.Parse(dto.MontoTotalVentasProductoMasVendido!, NumberStyles.Currency | NumberStyles.Number, CultureInfo.InvariantCulture))
                .FirstOrDefault();

            return productoMasVendido ?? new ConsultaProductoMasVendido
            {
                NombreProductoMasVendido = "No hay productos vendidos.",
                MontoTotalVentasProductoMasVendido = "0.00"
            };
        }

        //Consulta 4
        private ConsultaLocalConMayorVentas ObtenerLocalConMayorVentas(List<VentaDTO> ventas, List<LocalDTO> locales)
        {
            var localConMasVentas = locales
                    .GroupJoin(
                        ventas,
                        l => l.IdLocal,
                        v => v.IdLocal,
                        (local, ventasPorLocal) => new
                        {
                            NombreLocal = local.Nombre,
                            MontoTotalVentas = ventasPorLocal.Sum(v => v.Total)
                        })
                    .OrderByDescending(x => x.MontoTotalVentas)
                    .FirstOrDefault();

            return localConMasVentas != null ? new ConsultaLocalConMayorVentas
            {
                NombreLocalConMasVentas = localConMasVentas.NombreLocal,
                MontoTotalVentasLocal = localConMasVentas.MontoTotalVentas.ToString("N2", CultureInfo.InvariantCulture)
            } : new ConsultaLocalConMayorVentas
            {
                NombreLocalConMasVentas = "No hay datos disponibles",
                MontoTotalVentasLocal = "0.00"
            };
        }

        //Consulta 5
        private ConsultaMarcaMayorMargeGanancias ObtenerMarcaConMayorMargen(List<VentaDetalleDTO> ventasDetalle, List<ProductoDTO> productos, List<MarcaDTO> marcas)
        {
            var productoTotales = from vd in ventasDetalle
                                  join p in productos on vd.IdProducto equals p.IdProducto
                                  select new
                                  {
                                      p.IdMarca,
                                      vd.Cantidad,
                                      vd.PrecioUnitario,
                                      p.CostoUnitario,
                                      TotalLinea = vd.Cantidad * vd.PrecioUnitario,
                                      CostoTotalLinea = vd.Cantidad * p.CostoUnitario
                                  };

            var marcaTotales = from pt in productoTotales
                               group pt by pt.IdMarca into g
                               join m in marcas on g.Key equals m.IdMarca
                               select new
                               {
                                   NombreMarca = m.Nombre,
                                   TotalIngresos = g.Sum(x => x.TotalLinea),
                                   TotalCostos = g.Sum(x => x.CostoTotalLinea),
                                   TotalGananciaNeta = g.Sum(x => x.TotalLinea - x.CostoTotalLinea),
                                   MargenGanancias = g.Sum(x => x.TotalLinea) != 0
                                       ? ((decimal)g.Sum(x => x.TotalLinea - x.CostoTotalLinea) / (decimal)g.Sum(x => x.TotalLinea)) * 100
                                       : 0
                               };

            var marcaConMayorMargen = marcaTotales.OrderByDescending(x => x.MargenGanancias).FirstOrDefault();

            return marcaConMayorMargen != null ? new ConsultaMarcaMayorMargeGanancias
            {
                NombreMarca = marcaConMayorMargen.NombreMarca,
                TotalIngresos = marcaConMayorMargen.TotalIngresos.ToString("N2", CultureInfo.InvariantCulture),
                TotalCostos = marcaConMayorMargen.TotalCostos.ToString("N2", CultureInfo.InvariantCulture),
                TotalGananciaNeta = marcaConMayorMargen.TotalGananciaNeta.ToString("N2", CultureInfo.InvariantCulture),
                MargenGananciasPorcentaje = marcaConMayorMargen.MargenGanancias.ToString("N2") + " %"
            } : new ConsultaMarcaMayorMargeGanancias
            {
                NombreMarca = "No hay marcas disponibles",
                MargenGananciasPorcentaje = "0.00 %",
                TotalIngresos = "0.00",
                TotalCostos = "0.00",
                TotalGananciaNeta = "0.00"
            };
        }

        //Consulta 6
        private List<ProductoMasVendidoPorLocalDTO> ObtenerProductoMasVendidoPorLocal(
            List<VentaDTO> ventas,
            List<VentaDetalleDTO> detalles,
            List<LocalDTO> locales,
            List<ProductoDTO> productos)
        {
            var productosVendidos = from vd in detalles
                                    join v in ventas on vd.IdVenta equals v.IdVenta
                                    group vd by new { v.IdLocal, vd.IdProducto } into ventaGrupo
                                    select new
                                    {
                                        IdLocal = ventaGrupo.Key.IdLocal,
                                        IdProducto = ventaGrupo.Key.IdProducto,
                                        TotalVendido = ventaGrupo.Sum(x => x.Cantidad)
                                    };

            var maxProductosVendidos = from pv in productosVendidos
                                       group pv by pv.IdLocal into grupoLocal
                                       select new
                                       {
                                           IdLocal = grupoLocal.Key,
                                           MaximoVendido = grupoLocal.Max(x => x.TotalVendido)
                                       };

            List<ProductoMasVendidoPorLocalDTO> productosMasVendidosPorLocal = (from pv in productosVendidos
                                                                                join mpv in maxProductosVendidos on pv.IdLocal equals mpv.IdLocal
                                                                                where pv.TotalVendido == mpv.MaximoVendido
                                                                                join l in locales on pv.IdLocal equals l.IdLocal
                                                                                join p in productos on pv.IdProducto equals p.IdProducto
                                                                                select new ProductoMasVendidoPorLocalDTO
                                                                                {
                                                                                    NombreLocal = l.Nombre,
                                                                                    NombreProductoMasVendido = p.Nombre,
                                                                                    TotalVendido = pv.TotalVendido
                                                                                }).ToList();

            return productosMasVendidosPorLocal;
        }

    }
}
