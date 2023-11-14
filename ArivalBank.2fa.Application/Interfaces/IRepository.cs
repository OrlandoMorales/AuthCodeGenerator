using ArivalBank._2fa.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ArivalBank._2fa.Application.Interfaces
{
    public interface IRepository<TEntity> where TEntity : class
    {
        public Task Save(TEntity entity);
        public Task Update(TEntity entity);
        public Task Delete(TEntity entity);
        public Task<TEntity> GetById(object id);
        public IQueryable<TEntity> GetAll();
        public List<TEntity> ListAll();

    }
}