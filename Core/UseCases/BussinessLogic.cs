using Core.Entities;
using Core.Interfaces;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

            return autores;
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
    }
}
