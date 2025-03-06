using Microsoft.EntityFrameworkCore;
using ShoppingCart.Entities;

namespace ShoppingCart.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext _db;
        public ICategoryRepository Category { get; private set; }
        public IProductRepository Product { get; private set; }
        public IProductImageRepository ProductImage { get; private set; }
        public IShoppingkartRepository Shoppingkart { get; private set; }
        public IApplicationUserRepository ApplicationUser { get; private set; }
        public IOrderHeaderRepository OrderHeader { get; private set; }
        public IOrderDetailRepository OrderDetail { get; private set; }

        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            Category = new CategoryRepository(_db);
            Product = new ProductRepository(_db);
            ProductImage = new ProductImageRepository(_db);
            Shoppingkart = new ShoppingKartRepository(_db);
            ApplicationUser = new ApplicationUserRepository(_db);
            OrderHeader = new OrderHeaderRepository(_db);
            OrderDetail = new OrderDetailRepository(_db);
        }

        public Product GetWithIncludes(int productId)
        {
            return _db.Product
                .Include(p => p.Category)
                .Include(p => p.ProductImages)
                .FirstOrDefault(p => p.ProductId == productId);
        }
        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
