using Application.Interfaces.Repository;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Implementation.Repository
{
	public class GenericRepository<T> : IGenericRepository<T> where T : class
	{
		private readonly ApplicationDbContext _dbContext;
		private readonly DbSet<T> _dbSet = null;

		public GenericRepository(ApplicationDbContext dbContext)
        {
			_dbContext = dbContext;
			_dbSet = _dbContext.Set<T>();
		}
        public async Task<T> AddAsync(T entity)
		{
			await _dbSet.AddAsync(entity);
			await _dbContext.SaveChangesAsync();
			return entity;
		}

		public async Task DeleteAsync(T entity)
		{
			_dbSet.Remove(entity);
			await _dbContext.SaveChangesAsync();
		}

		public async Task<IEnumerable<T>> GetAllAsync()
		{
			return await _dbSet.ToListAsync();
		}

		public async Task<T> GetByIdAsync(int id)
		{
			return await _dbSet.FindAsync(id);
		}

		public async Task<int> CountAsync(Expression<Func<T, bool>> predicate)
		{
			return await _dbSet.CountAsync(predicate);
		}
		public async Task<T> GetFirstOrDefault(Expression<Func<T, bool>> filter)
		{
			try
			{
				return await _dbSet.FirstOrDefaultAsync(filter);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public async Task UpdateAsync(T entity)
		{
			_dbSet.Attach(entity);
			_dbContext.Entry(entity).State = EntityState.Modified;
			await _dbContext.SaveChangesAsync();	
		}
        public async Task<List<T>> Where(Expression<Func<T, bool>> filter)
        {
            try
            {
                return await _dbSet.Where(filter).ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
