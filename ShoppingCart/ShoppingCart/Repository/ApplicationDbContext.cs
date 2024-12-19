using Microsoft.EntityFrameworkCore;

namespace ShoppingCart.Repository
{
	public class ApplicationDbContext :DbContext
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
		{

		}
	}
}
