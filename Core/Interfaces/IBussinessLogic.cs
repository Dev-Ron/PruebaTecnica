using Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Interfaces
{
    public interface IBussinessLogic
    {
        public User CrearUsuario(User user);

        public User ObtenerUsuario(string username);
        public List<Libro> ConsultarLibrosApi();

        public List<Autor> ConsultarAutoresApi(List<Libro> Libros);
    }
}
