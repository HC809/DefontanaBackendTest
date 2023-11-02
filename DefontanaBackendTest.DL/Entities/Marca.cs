using System;
using System.Collections.Generic;

namespace DefontanaBackendTest.DL.Entities;

public partial class Marca
{
    public long IdMarca { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual ICollection<Producto> Producto { get; set; } = new List<Producto>();
}
