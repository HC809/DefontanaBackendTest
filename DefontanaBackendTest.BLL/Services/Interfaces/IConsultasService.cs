using DefontanaBackendTest.BLL.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DefontanaBackendTest.BLL.Services.Interfaces
{
    public interface IConsultasService
    {
        Task<ConsultaDTO> ConsultaVentas();
    }
}
