using ShoppingCart.Entities;
using ShoppingCart.Models;

namespace ShoppingCart.Repository
{
    public interface IProductRepository: IRepository<Product>
    {
        void Update(Product obj); 
    }
}
