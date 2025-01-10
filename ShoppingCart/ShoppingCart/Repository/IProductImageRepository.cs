using ShoppingCart.Models;

namespace ShoppingCart.Repository
{
    public interface IProductImageRepository: IRepository<ProductImage>
    {
        void Update(ProductImage obj); 
    }
}
