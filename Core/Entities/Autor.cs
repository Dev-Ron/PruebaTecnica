using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entities
{
    public class Autor : BaseEntity
    {
        public Libro libro { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public int IdApi { get; set; }
    }
}
