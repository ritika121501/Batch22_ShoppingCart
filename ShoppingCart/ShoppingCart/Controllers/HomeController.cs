using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Entities;
using ShoppingCart.Models;
using ShoppingCart.Repository;
using System.Diagnostics;
using System.Security.Claims;

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

        [HttpPost]
        public IActionResult Details(ShoppingKart shoppingKart)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            if (!claimsIdentity.IsAuthenticated) 
            {
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            }
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            var productFromDb = _unitOfWork.Product.Get(u => u.ProductId == shoppingKart.ProductId, includeProperties: "Category,ProductImages");

            var cartFromDb = _unitOfWork.Shoppingkart.Get(u => u.ApplicationUserId == userId && u.ProductId == shoppingKart.ProductId);
            shoppingKart.ApplicationUserId = userId;
            shoppingKart.Price = productFromDb.Price;
            if (cartFromDb != null)
            {
                shoppingKart.Id = cartFromDb.Id;
                shoppingKart.Count += cartFromDb.Count;
                _unitOfWork.Shoppingkart.Update(shoppingKart);
                _unitOfWork.Save();
            }
            else
            {
                _unitOfWork.Shoppingkart.Add(shoppingKart);
                _unitOfWork.Save();
            }

            return RedirectToAction("Index");
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
