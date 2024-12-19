using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShoppingCart.Models
{
	public class Product
	{
		[Key]
		public int ProductId { get; set; }

		[Required]
		public string Title { get; set; }
		public string Description { get; set; }
		public string ISBN {  get; set; }
		public string Author { get; set; }
		public decimal Price {  get; set; }
		public int CategoryId {  get; set; }
		[ForeignKey("CategoryId")]
		public Category Category { get; set; }
	}
}
