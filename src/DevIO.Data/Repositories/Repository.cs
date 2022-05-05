using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DevIO.Business.Interfaces;
using DevIO.Business.Models;
using DevIO.Data.Contexts;
using Microsoft.EntityFrameworkCore;

namespace DevIO.Data.Repositories
{
	public abstract class Repository<TEntity> : IRepository<TEntity>
		where TEntity : Entity, new()
	{
        protected readonly MyDbContext Db;
        protected readonly DbSet<TEntity> DbSet;

        public async Task<IEnumerable<TEntity>> SearchAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await DbSet
                    .AsNoTracking()
                    .Where(predicate)
                    .ToListAsync();
        }

        public async Task<TEntity> GetByIdAsync(Guid id)
        {
            return await DbSet.FindAsync(id);
        }

        public async Task<List<TEntity>> GetAllAsync()
        {
            return await DbSet.ToListAsync();
        }

        public async Task AddAsync(TEntity entity)
        {
            await DbSet.AddAsync(entity);
            await SaveChangesAsync();
        }

        public async Task UpdateAsync(TEntity entity)
        {
            DbSet.Update(entity);
            await SaveChangesAsync();

        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = new TEntity { Id = id };
            DbSet.Remove(entity);
            await SaveChangesAsync();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await Db.SaveChangesAsync();
        }

        public void Dispose()
        {
            Db?.Dispose();
        }
    }
}

