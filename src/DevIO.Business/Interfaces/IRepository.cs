using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DevIO.Business.Models;

namespace DevIO.Business.Interfaces
{
	public interface IRepository<TEntity> : IDisposable
		where TEntity : Entity
	{
		Task AddAsync(TEntity entity);

		Task<TEntity> GetByIdAsync(Guid id);

		Task<List<TEntity>> GetAllAsync();

		Task UpdateAsync(TEntity entity);

		Task DeleteAsync(Guid id);

		Task<IEnumerable<TEntity>> SearchAsync(Expression<Func<TEntity, bool>> predicate);

		Task<int> SaveChangesAsync();
	}
}

