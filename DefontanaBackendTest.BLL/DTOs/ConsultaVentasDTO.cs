using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DefontanaBackendTest.BLL.DTOs
{
    public class ConsultaVentasDTO
    {
        public string? MontoTotalVentasUltimos30Dias { get; set; }
        public int CantidadTotalVentasUltimos30Dias { get; set; }
    }
}
