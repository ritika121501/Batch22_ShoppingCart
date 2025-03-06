using ShoppingCart.Entities;

namespace ShoppingCart.Repository
{
    public interface IUnitOfWork
    {
        ICategoryRepository Category { get; }
        IProductRepository Product { get; }
        IProductImageRepository ProductImage { get; }
        IShoppingkartRepository Shoppingkart { get; }
        IApplicationUserRepository ApplicationUser { get; }

        IOrderDetailRepository OrderDetail { get; }
        IOrderHeaderRepository OrderHeader { get; }
        Product GetWithIncludes(int productId);
        void Save();
    }
}
