using System;
using System.Collections.Generic;

namespace DefontanaBackendTest.DL.Entities;

public partial class Venta
{
    public long IdVenta { get; set; }

    public int Total { get; set; }

    public DateTime Fecha { get; set; }

    public long IdLocal { get; set; }

    public virtual Local IdLocalNavigation { get; set; } = null!;

    public virtual ICollection<VentaDetalle> VentaDetalle { get; set; } = new List<VentaDetalle>();
}
