using ShoppingCart.Models;

namespace ShoppingCart.Repository
{
    public class ProductImageRepository : Repository<ProductImage>, IProductImageRepository
    {
        public ApplicationDbContext _db;
        public ProductImageRepository(ApplicationDbContext db) : base(db) 
        {
            _db = db;
        }

        public void Update(ProductImage obj)
        {
            _db.ProductImages.Update(obj);
        }

    }
}
