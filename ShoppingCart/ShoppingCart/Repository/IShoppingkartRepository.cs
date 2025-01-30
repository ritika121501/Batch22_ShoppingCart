using ShoppingCart.Entities;

namespace ShoppingCart.Repository
{
    public interface IShoppingkartRepository :IRepository<ShoppingKart>
    {
        void Update(ShoppingKart obj);
    }
}
