using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using web_api.DAL.Entities;

namespace web_api.DAL.Repositories
{
    public class GenericRepository<TEntity, TId>
        : IGenericRepository<TEntity, TId>
        where TEntity : class, IBaseEntity<TId>
        where TId : notnull
    {
        private readonly AppDbContext _context;

        public GenericRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateAsync(TEntity entity)
        {
            try
            {
                await _context.Set<TEntity>().AddAsync(entity);
                var result = await _context.SaveChangesAsync();
                return result != 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> UpdateAsync(TEntity entity)
        {
            try
            {
                entity.UpdatedDate = DateTime.UtcNow;
                _context.Set<TEntity>().Update(entity);
                var result = await _context.SaveChangesAsync();
                return result != 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> DeleteAsync(TEntity entity)
        {
            try
            {
                _context.Set<TEntity>().Remove(entity);
                var result = await _context.SaveChangesAsync();
                return result != 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<TEntity?> GetByIdAsync(string id)
        {
            try
            {
                var entity = await _context
                    .Set<TEntity>()
                    .FirstOrDefaultAsync(e => e.Id.Equals(id));
                return entity;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public IQueryable<TEntity> GetAll()
        {
            return _context.Set<TEntity>();
        }
    }
}
