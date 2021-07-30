
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace TanzuTacos.OrderService.Data
{
	public interface IDocumentDbRepository<T>
	{
		Task<T> AddOrUpdateAsync(T entity);
		Task<T> GetByIdAsync(string id);
		Task<bool> RemoveAsync(string id);
		Task<IQueryable<T>> WhereAsync(Expression<Func<T, bool>> predicate);
		//Task<IQueryable<T>> TopAsync(Expression<Func<T, bool>> predicate int n);
	}
}
