using DefontanaBackendTest.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace DefontanaBackendTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConsultasController : ControllerBase
    {
        private IConsultasService _consultaService;

        public ConsultasController(IConsultasService consultaService)
        {
            _consultaService = consultaService;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Obtiene todas las consultas de ventas", Description = "Proporciona información detallada sobre las 6 consultas solicitadas.")]
        public async Task<IActionResult> ObtenerConsultas() => Ok(await _consultaService.ConsultaVentas());
    }
}
