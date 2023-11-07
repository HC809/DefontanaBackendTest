using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DefontanaBackendTest.BLL.DTOs
{
    public class ProductoDTO
    {
        public long IdProducto { get; set; }

        public string Nombre { get; set; } = null!;

        public string Codigo { get; set; } = null!;

        public long IdMarca { get; set; }

        public string Modelo { get; set; } = null!;

        public int CostoUnitario { get; set; }
    }
}
