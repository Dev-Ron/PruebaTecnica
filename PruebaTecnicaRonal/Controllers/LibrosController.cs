using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PruebaTecnica.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PruebaTecnica.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class LibrosController : ControllerBase
    {
        
        private readonly ILogger<LibrosController> _logger;
        private readonly IRepositorio _repositorio;
        private readonly IBussinessLogic CapaLogica;

        public LibrosController(ILogger<LibrosController> logger, IRepositorio repositorio, IBussinessLogic _CapaLogica)
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
                Autores = await CapaLogica.ConsultarAutoresFiltro(filtro.Campo, filtro.Busqueda);
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
                CapaLogica.BorrarRegistros();
            }
            catch (Exception e)
            {
                Autores = null;
            }
            return new JsonResult("Eliminados");
        }



    }
}
