using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PruebaTecnicaRonal.ViewModels;
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
        public async Task<IEnumerable<Autor>> Get()
        {
            IEnumerable<Autor> Autores;
            try
            {
                Autores = await _repositorio.FindAllIncludeAsync<Autor>(s => s.Id != 0, r => r.libro);
            } catch (Exception e)
            {
                Autores = null;
            }
            return Autores;
        }
        [HttpPost("ConsultarFiltro")]
        public async Task<IEnumerable<Autor>> OnPostConsultarFiltro(Filtro filtro)
        {
            IEnumerable<Autor> Autores;
            try
            {
                switch (filtro.Campo)
                {
                    case "Titulo":
                        Autores = await _repositorio.FindAllIncludeAsync<Autor>(r => r.libro.title.Contains(filtro.Busqueda), r => r.libro);
                        break;
                    case "Descripción":
                        Autores = await _repositorio.FindAllIncludeAsync<Autor>(r => r.libro.description.Contains(filtro.Busqueda), r => r.libro);
                        break;
                    case "Número de paginas":
                        Autores = await _repositorio.FindAllIncludeAsync<Autor>(r => r.libro.pageCount == Convert.ToInt32(filtro.Busqueda), r => r.libro);
                        break;
                    case "Nombre autor":
                        Autores = await _repositorio.FindAllIncludeAsync<Autor>(r => r.firstName.Contains(filtro.Busqueda), r => r.libro);
                        break;
                    case "Apellido autor":
                        Autores = await _repositorio.FindAllIncludeAsync<Autor>(r => r.lastName.Contains(filtro.Busqueda), r => r.libro);
                        break;
                    case "Fecha de publicación":
                        Autores = await _repositorio.FindAllIncludeAsync<Autor>(r => r.libro.publishDate.Date == Convert.ToDateTime(filtro.Busqueda), r => r.libro);
                        break;
                    default:
                        Autores = null;
                        break;
                } 

                
            }
            catch (Exception e)
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

        [HttpDelete("Eliminar")]
        public JsonResult OnDeleteEliminar()
        {
            IEnumerable<Autor> Autores;
            try
            {
                _repositorio.DeleteRange<Autor>(_repositorio.FindAll<Autor>(s => s.Id != 0));
                _repositorio.DeleteRange<Libro>(_repositorio.FindAll<Libro>(s => s.Id != 0));
            }
            catch (Exception e)
            {
                Autores = null;
            }
            return new JsonResult("Eliminados");
        }



    }
}
