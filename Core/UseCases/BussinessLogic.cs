using Core.Entities;
using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.UseCases
{
    public class BussinessLogic<T> : IBussinessLogic<T> where T : BaseEntity
    {
        private readonly IRepositorio<T> Repository;
        public BussinessLogic(IRepositorio<T> Repository)
        {
            this.Repository = Repository;
        }
    }
}
