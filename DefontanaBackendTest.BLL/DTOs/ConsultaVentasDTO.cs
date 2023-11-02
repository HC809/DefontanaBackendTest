namespace DefontanaBackendTest.BLL.DTOs
{
    public class Consulta1VentasDTO
    {
        public string? MontoTotalVentasUltimos30Dias { get; set; }
        public int CantidadTotalVentasUltimos30Dias { get; set; }
    }

    public class Consulta2VentasDTO
    {
        public string? DiaVentaMasAlta { get; set; }
        public string? HoraVentaMasAlta { get; set; }
        public string? MontoVentaMasAlta { get; set; }
    }

    public class ConsultaVentasDTO
    {
        public Consulta1VentasDTO? Consulta1 { get; set; }
        public Consulta2VentasDTO? Consulta2 { get; set; }
    }
}
