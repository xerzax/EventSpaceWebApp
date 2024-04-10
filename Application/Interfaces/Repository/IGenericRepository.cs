using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Repository
{
	public interface IGenericRepository<T> where T: class
	{
		Task<IEnumerable<T>> GetAllAsync();
		Task<T> GetByIdAsync(int id);
		Task<T> AddAsync(T entity);
		Task UpdateAsync(T entity);
		Task DeleteAsync(T entity);

		Task<T> GetFirstOrDefault(Expression<Func<T, bool>> filter);
        Task<List<T>> Where(Expression<Func<T, bool>> filter);
		Task<int> CountAsync(Expression<Func<T, bool>> predicate);
	}
}
