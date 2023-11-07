namespace DefontanaBackendTest.BLL.DTOs
{
    public class ConsultaVentasUltimos30Dias
    {
        public string? MontoTotalVentasUltimos30Dias { get; set; }
        public int CantidadTotalVentasUltimos30Dias { get; set; }
    }

    public class ConsultaFechaVentaMasAlta
    {
        public string? DiaVentaMasAlta { get; set; }
        public string? HoraVentaMasAlta { get; set; }
        public string? MontoVentaMasAlta { get; set; }
    }

    public class ConsultaProductoMasVendido
    {
        public string? NombreProductoMasVendido { get; set; }
        public string? MontoTotalVentasProductoMasVendido { get; set; }
    }

    public class ConsultaLocalConMayorVentas
    {
        public string? NombreLocalConMasVentas { get; set; }
        public string? MontoTotalVentasLocal { get; set; }
    }

    public class ConsultaMarcaMayorMargeGanancias
    {
        public string? NombreMarca { get; set; }
        public string? MargenGananciasPorcentaje { get; set; }
        public string? TotalIngresos { get; set; }
        public string? TotalCostos { get; set; }
        public string? TotalGananciaNeta { get; set; }
    }

    public class ProductoMasVendidoPorLocalDTO
    {
        public string? NombreLocal { get; set; }
        public string? NombreProductoMasVendido { get; set; }
        public int TotalVendido { get; set; }
    }

    public class ConsultaDTO
    {
        public ConsultaVentasUltimos30Dias? Consulta1 { get; set; }
        public ConsultaFechaVentaMasAlta? Consulta2 { get; set; }
        public ConsultaProductoMasVendido? Consulta3 { get; set; }
        public ConsultaLocalConMayorVentas? Consulta4 { get; set; }
        public ConsultaMarcaMayorMargeGanancias? Consulta5 { get; set; }
        public List<ProductoMasVendidoPorLocalDTO>? Consulta6 { get; set; }
    }
}
