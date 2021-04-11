using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entities
{
    public class Libro : BaseEntity
    {
        public string title { get; set; }
        public string description { get; set; }
        public int pageCount { get; set; }
        public string excerpt { get; set; }
        public DateTime publishDate { get; set; }
        public int IdApi { get; set; }
    }
}
