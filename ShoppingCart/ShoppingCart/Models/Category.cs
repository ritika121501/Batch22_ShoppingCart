using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ShoppingCart.Models
{
	public class Category
	{
		[Key]
		public int CategoryId { get; set; }

		[MaxLength(30)]
		[Required]
		[DisplayName("Category name")]
		public string Name { get; set; }
	}
}
