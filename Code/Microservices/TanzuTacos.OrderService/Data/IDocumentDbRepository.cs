
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace TanzuTacos.OrderService.Data
{
	public interface IDocumentDbRepository<T>
	{
		Task<T> AddOrUpdateAsync(T entity);
		Task<T> GetByIdAsync(Guid id);
		Task<bool> RemoveAsync(Guid id);
		Task<IQueryable<T>> WhereAsync(Expression<Func<T, bool>> predicate);
		//Task<IQueryable<T>> TopAsync(Expression<Func<T, bool>> predicate int n);
	}
}
