using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DefontanaBackendTest.BLL.DTOs
{
    public class VentaDetalleDTO
    {
        public long IdVentaDetalle { get; set; }

        public long IdVenta { get; set; }

        public int PrecioUnitario { get; set; }

        public int Cantidad { get; set; }

        public int TotalLinea { get; set; }

        public long IdProducto { get; set; }

        public string? ProductoNombre { get; set; }
    }
}
