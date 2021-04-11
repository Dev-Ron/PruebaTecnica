using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PruebaTecnicaRonal.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IRepositorio _repositorio;
        private readonly IBussinessLogic CapaLogica;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IRepositorio repositorio, IBussinessLogic _CapaLogica)
        {
            _logger = logger;
            _repositorio = repositorio;
            CapaLogica = _CapaLogica;
        }

        [HttpGet]
        public IEnumerable<Autor> Get()
        {
            IEnumerable<Autor> Autores;
            try
            {
                Autores = _repositorio.FindAllInclude<Autor>(r => r.libro != null, r => r.libro);
            }catch(Exception e)
            {
                Autores = null;
            }
            return Autores;
        }

        [HttpPost("Sincronizar")]
        public IEnumerable<Autor> OnPostSincronizar()
        {
            IEnumerable<Autor> Autores;
            try
            {
                List<Libro> libros = CapaLogica.ConsultarLibrosApi();
                Autores = CapaLogica.ConsultarAutoresApi(libros);
            }
            catch (Exception e)
            {
                Autores = null;
            }
            return Autores;
        }

    }
}
