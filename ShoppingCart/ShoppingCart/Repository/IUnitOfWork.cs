namespace ShoppingCart.Repository
{
	public interface IUnitOfWork
	{
		ICategoryRepository Category { get; }
		void Save();
	}
}
