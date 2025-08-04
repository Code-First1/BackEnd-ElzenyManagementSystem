using Domain.Contracts;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using System;
using System.Collections.Generic;
using System.Formats.Tar;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class GenericRepository<TEntity, TKey> : IGenericRepository<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        private ElzenyDbContext _context;
        public GenericRepository(ElzenyDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<TEntity>> GetAllAsync(bool trackChages = false)
        {
            if(typeof(TEntity) == typeof(Product))
            {
                if (trackChages)
                    return await _context.Products.Include(P => P.Category).ToListAsync() as IEnumerable<TEntity>;
                return await _context.Products.AsNoTracking().Include(P => P.Category).ToListAsync() as IEnumerable<TEntity>;

            }

            if(trackChages)
                return await _context.Set<TEntity>().ToListAsync();
            return await _context.Set<TEntity>().AsNoTracking().ToListAsync();

        }

        public async Task<TEntity?> GetAsync(TKey id)
        {
            if (typeof(TEntity) == typeof(Product))
            {
                return await _context.Products.Include(P => P.Category).FirstOrDefaultAsync(P => P.Id == id as int?) as TEntity;

            }

            return await _context.Set<TEntity>().FindAsync(id);    
        }

        public async Task AddAsync(TEntity entity)
        {
            await _context.AddAsync(entity);
        }

        public void Update(TEntity entity)
        {
             _context.Update(entity);
        }

        public void Delete(TEntity entity)
        {
            _context.Remove(entity);
            
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(ISpecifications<TEntity, TKey> spec, bool trackChages = false)
        {
            return await ApplySpecifications(spec).ToListAsync();
        }

        public async Task<TEntity?> GetAsync(ISpecifications<TEntity, TKey> spec, TKey id)
        {
            return await ApplySpecifications(spec).FirstOrDefaultAsync();
        }

        private IQueryable<TEntity> ApplySpecifications(ISpecifications<TEntity, TKey> spec)
        {
            return SpecificationEvaluator.GetQuery(_context.Set<TEntity>(), spec);
        }
    }
}
