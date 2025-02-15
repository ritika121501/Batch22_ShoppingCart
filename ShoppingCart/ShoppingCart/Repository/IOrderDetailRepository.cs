using ShoppingCart.Entities;

namespace ShoppingCart.Repository
{
    public interface IOrderDetailRepository : IRepository<OrderDetail>
    {
        void Update(OrderDetail obj);
    }
}
