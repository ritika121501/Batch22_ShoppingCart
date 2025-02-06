namespace ShoppingCart.Repository
{
    public interface IUnitOfWork
    {
        ICategoryRepository Category { get; }
        IProductRepository Product { get; }
        IProductImageRepository ProductImage { get; }
        IShoppingkartRepository Shoppingkart { get; }
        IApplicationUserRepository ApplicationUser { get; }
        void Save();
    }
}
