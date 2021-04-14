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
                        var bearer_tokenJWT = GenerarToken(user);
                        string bearer_token = new JwtSecurityTokenHandler().WriteToken(bearer_tokenJWT);
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
                return new JsonResult(e);
            }
        }

        private JwtSecurityToken GenerarToken(UserVM login)
        {
            string ValidIssuer = config.GetValue<string>("Issuer");
            string ValidAudience = config.GetValue<string>("Audience");
            SymmetricSecurityKey IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.GetValue<string>("SecretKey")));

            //La fecha de expiracion sera el mismo dia a las 12 de la noche
            DateTime dtFechaExpiraToken;
            DateTime now = DateTime.Now;
            dtFechaExpiraToken = new DateTime(now.Year, now.Month, now.Day, 23, 59, 59, 999);

            //Agregamos los claim nuestros
            var claims = new[]
            {
                new Claim(Constantes.JWT_CLAIM_USUARIO, login.userName)
            };

            return new JwtSecurityToken
            (
                issuer: ValidIssuer,
                audience: ValidAudience,
                claims: claims,
                expires: dtFechaExpiraToken,
                notBefore: now,
                signingCredentials: new SigningCredentials(IssuerSigningKey, SecurityAlgorithms.HmacSha256)
            );
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
