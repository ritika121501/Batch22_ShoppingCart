using System.Linq.Expressions;

namespace ShoppingCart.Repository
{
	public interface IRepository<T> where T : class
	{
		IEnumerable<T> GetAllExpression(Expression<Func<T, bool>> filter=null, string? includeProperties= null);
		T Get(Expression<Func<T, bool>> filter = null, string? includeProperties = null, bool tracked = false);
		void Add(T entity);
		void Remove(T entity);
		void RemoveRange(IEnumerable<T> entity);
	}
}
