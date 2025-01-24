using Microsoft.EntityFrameworkCore;
using ShoppingCart.Entities;
using ShoppingCart.Models;

namespace ShoppingCart.Repository
{
	public class ApplicationDbContext :DbContext
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
		{
			
		}

		public DbSet<Category> Category { get; set; }
        public DbSet<Product> Product { get; set; }

		public DbSet<ProductImage> ProductImages { get; set; }
    }
}
