using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PruebaTecnicaRonal.ViewModels;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PruebaTecnicaRonal.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : ControllerBase
    {
        [HttpPost]
        public ActionResult OnPost(User user)
        {
            ObjectResult result;
            List<User> UsuariosApi = new List<User>();
            try
            {
                RestClient client = new RestClient(Environment.GetEnvironmentVariable("API_PRUEBA") + "api/v1/Users/");
                client.Timeout = 10000;
                RestRequest request = new RestRequest(Method.GET);
                request.AddHeader("Content-Type", "application/json");
                request.AddHeader("Accept", "application/json");
                IRestResponse response = client.Execute(request);

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    UsuariosApi = JsonConvert.DeserializeObject<List<User>>(response.Content);
                }

                if(UsuariosApi.Any(r => r.userName == user.userName && r.password == user.password))
                {
                    result = StatusCode(StatusCodes.Status200OK, "Autenticado!");
                }
                else
                {
                    result = StatusCode(StatusCodes.Status500InternalServerError, response.Content);
                }
            
            }
            catch (Exception e)
            {
                result = StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
            result = StatusCode(StatusCodes.Status200OK, "Autenticado!");
            return result;
        }

        // GET: LoginController
        public ActionResult OnGet()
        {
            return StatusCode(StatusCodes.Status200OK, "");
        }
    }
}
