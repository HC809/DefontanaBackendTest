using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DefontanaBackendTest.BLL.DTOs
{
    public class LocalDTO
    {
        public long IdLocal { get; set; }

        public string Nombre { get; set; } = null!;

        public string Direccion { get; set; } = null!;
    }
}
