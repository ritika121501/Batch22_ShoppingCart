using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Entities;
using ShoppingCart.Models;
using ShoppingCart.Repository;
using System.Diagnostics;

namespace ShoppingCart.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private readonly IUnitOfWork _unitOfWork;

		public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
		{
			_logger = logger;
			_unitOfWork = unitOfWork;
		}

		public IActionResult Index()
		{
			IEnumerable<Product> productList = _unitOfWork.Product.GetAllExpression(includeProperties: "Category,ProductImages");
			return View(productList);
		}

		public IActionResult Details(int productId)
		{
			ShoppingKart shoppingKart = new ShoppingKart()
			{
				Product = _unitOfWork.Product.Get(u => u.ProductId == productId, includeProperties: "Category,ProductImages"),
				Count = 1,
				ProductId = productId
			};
			return View(shoppingKart);
		}

		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
