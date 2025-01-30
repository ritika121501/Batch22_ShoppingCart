using ShoppingCart.Entities;

namespace ShoppingCart.Repository
{
    public class ShoppingKartRepository : Repository<ShoppingKart>, IShoppingkartRepository
    {
        public ApplicationDbContext _db;
        public ShoppingKartRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(ShoppingKart obj)
        {
            _db.ShoppingKarts.Update(obj);
        }
    }
}
