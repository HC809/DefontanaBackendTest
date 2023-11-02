using DefontanaBackendTest.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DefontanaBackendTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VentaController : ControllerBase
    {
        private IVentasService _ventaService;

        public VentaController(IVentasService ventaService)
        {
            _ventaService = ventaService;
        }

        [HttpGet]
        public async Task<IActionResult> ConsultaVentas() => Ok(await _ventaService.ConsultaVentas());
    }
}
