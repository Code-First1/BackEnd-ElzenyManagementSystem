using Domain.Contracts;
using Domain.Models;
using Persistence.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ElzenyDbContext _context;
        private readonly Dictionary<string, Object> _repositories;
        public UnitOfWork(ElzenyDbContext context)
        {
            _context = context;
            _repositories = new Dictionary<string, Object>();
        }
        public IGenericRepository<TEntity, Tkey> GetRepository<TEntity, Tkey>() where TEntity : BaseEntity<Tkey>
        {
            var type = typeof(TEntity).Name;
            if(!_repositories.ContainsKey(type))
            {
                var repository  = new GenericRepository<TEntity, Tkey>(_context);
                _repositories.Add(type, repository);
            }
            return (IGenericRepository<TEntity,Tkey>) _repositories[type];

        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
