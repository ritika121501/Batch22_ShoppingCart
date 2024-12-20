using ShoppingCart.Models;

namespace ShoppingCart.Repository
{
    public interface ICategoryRepository: IRepository<Category>
    {
        void Update(Category obj); 
    }
}
