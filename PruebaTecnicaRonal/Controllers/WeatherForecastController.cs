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
        private readonly IRepositorio<Libro> _repositorio;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IRepositorio<Libro> repositorio)
        {
            _logger = logger;
            _repositorio = repositorio;
        }

        [HttpGet]
        public IEnumerable<Libro> Get()
        {
            IEnumerable<Libro> Libros;
            try
            {
                
                Libros = _repositorio.Read();

            }catch(Exception e)
            {
                Libros = null;
            }
            return Libros;
        }
    }
}
