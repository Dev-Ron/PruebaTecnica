using Core.Entities;
using Core.Interfaces;
using System;
using System.Collections.Generic;
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
    }
}
