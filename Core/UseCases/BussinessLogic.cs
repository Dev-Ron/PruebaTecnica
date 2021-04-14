using Core.Entities;
using Core.Interfaces;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.UseCases
{
    public class BussinessLogic : IBussinessLogic
    {
        private readonly IRepositorio Repository;
        public BussinessLogic(IRepositorio Repository)
        {
            this.Repository = Repository;
        }

        public User CrearUsuario(User user)
        {
            return Repository.Create<User>(user);
        }

        public User ObtenerUsuario(string username)
        {
            return Repository.Find<User>(r => r.Usuario == username);
        }

        public List<Libro> ConsultarLibrosApi()
        {
            List<Libro> libros = new List<Libro>();

            RestClient client = new RestClient(Environment.GetEnvironmentVariable("API_PRUEBA") + "api/v1/Books/");
            client.Timeout = 10000;
            RestRequest request = new RestRequest(Method.GET);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Accept", "application/json");
            IRestResponse response = client.Execute(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                List<LibroApi> librosApi = JsonConvert.DeserializeObject<List<LibroApi>>(response.Content);

                librosApi.ForEach(r => libros.Add(new Libro
                {
                    IdApi = r.Id,
                    description = r.description,
                    pageCount = r.pageCount,
                    publishDate = r.publishDate,
                    title = r.title,
                    excerpt = r.excerpt
                }));
            }

            return libros;
        }

        public List<Autor> ConsultarAutoresApi(List<Libro> libros)
        {
            List<Autor> autores = new List<Autor>();

            RestClient client = new RestClient(Environment.GetEnvironmentVariable("API_PRUEBA") + "api/v1/Authors");
            client.Timeout = 10000;
            RestRequest request = new RestRequest(Method.GET);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Accept", "application/json");
            IRestResponse response = client.Execute(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                List<AutorApi> autoresApi = JsonConvert.DeserializeObject<List<AutorApi>>(response.Content);

                autoresApi.ForEach(r => autores.Add(new Autor
                {
                    IdApi = r.Id,
                    firstName = r.firstName,
                    lastName = r.lastName,
                    libro = libros.FirstOrDefault(s => s.IdApi == r.idBook)
                }));
            }

            Repository.AddRange<Libro>(libros);
            Repository.AddRange<Autor>(autores);
            return autores;
        }

        public List<User> ConsultarUsuariosApi()
        {
            List<UserApi> userApis = new List<UserApi>();
            List<User> useList = new List<User>();
            RestClient client = new RestClient(Environment.GetEnvironmentVariable("API_PRUEBA") + "api/v1/Users/");
            client.Timeout = 10000;
            RestRequest request = new RestRequest(Method.GET);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Accept", "application/json");
            IRestResponse response = client.Execute(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                userApis = JsonConvert.DeserializeObject<List<UserApi>>(response.Content);

                userApis.ForEach(r => useList.Add(new User
                {
                    Usuario = r.userName,
                    Clave = r.password
                }));
            }
            return useList;
        }

        public void BorrarRegistros()
        {
            Repository.DeletaAllRows<Autor>();
            Repository.DeletaAllRows<Libro>();
            //Repository.DeleteRange<Autor>(Repository.FindAll<Autor>(s => s.Id != 0));
            //Repository.DeleteRange<Libro>(Repository.FindAll<Libro>(s => s.Id != 0));
        }

        public async Task<IEnumerable<Autor>> ConsultarAutoresFiltro(string campo, string busqueda)
        {
            IEnumerable<Autor> Autores;
            switch (campo)
            {
                case "Titulo":
                    Autores = await Repository.FindAllIncludeAsync<Autor>(r => r.libro.title.Contains(busqueda), r => r.libro);
                    break;
                case "Descripción":
                    Autores = await Repository.FindAllIncludeAsync<Autor>(r => r.libro.description.Contains(busqueda), r => r.libro);
                    break;
                case "Número de paginas":
                    Autores = await Repository.FindAllIncludeAsync<Autor>(r => r.libro.pageCount == Convert.ToInt32(busqueda), r => r.libro);
                    break;
                case "Nombre autor":
                    Autores = await Repository.FindAllIncludeAsync<Autor>(r => r.firstName.Contains(busqueda), r => r.libro);
                    break;
                case "Apellido autor":
                    Autores = await Repository.FindAllIncludeAsync<Autor>(r => r.lastName.Contains(busqueda), r => r.libro);
                    break;
                case "Fecha de publicación":
                    Autores = await Repository.FindAllIncludeAsync<Autor>(r => r.libro.publishDate.Date == Convert.ToDateTime(busqueda), r => r.libro);
                    break;
                default:
                    Autores = null;
                    break;
            }
            return Autores;

        }

        private class AutorApi
        {
            public int idBook { get; set; }
            public string firstName { get; set; }
            public string lastName { get; set; }
            public int Id { get; set; }
        }

        private class LibroApi
        {
            public string title { get; set; }
            public string description { get; set; }
            public int pageCount { get; set; }
            public string excerpt { get; set; }
            public DateTime publishDate { get; set; }
            public int Id { get; set; }
        }

        private class UserApi
        {
            public string userName { get; set; }
            public string password { get; set; }
        }
    }
}
