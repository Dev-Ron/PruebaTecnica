using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using PruebaTecnica.ViewModels;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static PruebaTecnica.Helper;

namespace PruebaTecnica.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration config;
        private readonly IBussinessLogic CapaLogica;

        public LoginController(IConfiguration _config, IBussinessLogic _CapaLogica)
        {
            config = _config;
            CapaLogica = _CapaLogica;
        }


        [HttpPost]
        public JsonResult OnPost(UserVM user)
        {
            List<UserVM> UsuariosApi = new List<UserVM>();
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
                    UsuariosApi = JsonConvert.DeserializeObject<List<UserVM>>(response.Content);
                }

                if(UsuariosApi.Any(r => r.userName == user.userName))
                {
                    User usuarioBase = new User();
                    if (CapaLogica.ObtenerUsuario(user.userName) == null)
                    {
                        UserVM userAPI = UsuariosApi.FirstOrDefault(r => r.userName == user.userName);
                        usuarioBase = CrearUser(userAPI);
                    }
                    else
                    {
                       usuarioBase = CapaLogica.ObtenerUsuario(user.userName);
                    }
                    

                    if (HashHelper.CheckHash(user.password, usuarioBase.Clave, usuarioBase.Sal))
                    {
                        var secretKey = config.GetValue<string>("SecretKey");
                        var key = Encoding.ASCII.GetBytes(secretKey);

                        var claims = new ClaimsIdentity();
                        claims.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.userName));

                        var tokenDescriptor = new SecurityTokenDescriptor
                        {
                            Subject = claims,
                            Expires = DateTime.UtcNow.AddHours(4),
                            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                        };

                        var tokenHandler = new JwtSecurityTokenHandler();
                        var createdToken = tokenHandler.CreateToken(tokenDescriptor);

                        string bearer_token = tokenHandler.WriteToken(createdToken);
                        return new JsonResult(bearer_token);
                    }
                    else
                    {
                        Response.StatusCode = StatusCodes.Status401Unauthorized;
                        return new JsonResult("Credenciales incorrectas");
                    }
                }
                else
                {
                    Response.StatusCode = StatusCodes.Status404NotFound;
                    return new JsonResult("Usuario no encontrado");
                }
            
            }
            catch (Exception e)
            {
                Response.StatusCode = StatusCodes.Status500InternalServerError;
                return new JsonResult(e.Message);
            }
        }

        // GET: LoginController
        public ActionResult OnGet()
        {
            return StatusCode(StatusCodes.Status200OK, "");
        }

        private User CrearUser(UserVM usuarionuevo)
        {
            HashedPassword Password = HashHelper.Hash(usuarionuevo.password);
            User user = new User();
            user.Usuario = usuarionuevo.userName;
            user.Clave = Password.Password;
            user.Sal = Password.Salt;
            CapaLogica.CrearUsuario(user);
            return user;
        }
    }
}
