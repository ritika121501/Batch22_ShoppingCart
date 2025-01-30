using ShoppingCart.Entities;

namespace ShoppingCart.Repository
{
    public interface IProductRepository : IRepository<Product>
    {
        void Update(Product obj);
    }
}
