using ArivalBank._2fa.Application.Interfaces;
using ArivalBank._2fa.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArivalBank._2fa.Infrastructure.Repositories
{
    public class AuthorizationRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly AuthorizationDbContext _dbContext = null;
        private DbSet<TEntity> _table = null;

        public AuthorizationRepository(AuthorizationDbContext authorizationDbContext)
        {
            _dbContext = authorizationDbContext;
            _table = _dbContext.Set<TEntity>();
        }

        public Task Delete(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public IQueryable<TEntity> GetAll()
        {
            return _dbContext.Set<TEntity>().AsNoTracking();
        }

        public Task<TEntity> GetById(object id)
        {
            throw new NotImplementedException();
        }

        public List<TEntity> ListAll()
        {
            return _dbContext.Set<TEntity>().ToList();
        }

        public async Task Save(TEntity entity)
        {
            await _table.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
        }

        public Task Update(TEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}
