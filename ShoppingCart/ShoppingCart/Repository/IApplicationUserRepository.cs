using ShoppingCart.Entities;

namespace ShoppingCart.Repository
{
    public interface IApplicationUserRepository : IRepository<ApplicationUser>
    {
        void Update(ApplicationUser obj);
    }
    
}
