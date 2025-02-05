using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Entities;
using ShoppingCart.Entities.ViewModel;
using ShoppingCart.Repository;
using System.Security.Claims;

namespace ShoppingCart.Controllers
{
    public class CartController : Controller
    {
        private readonly ILogger<CartController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public CartController(IUnitOfWork unitOfWork, ILogger<CartController> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            ShoppingCartVM shoppingCartVM = new ShoppingCartVM()
            {
                ShoppingKartsList = _unitOfWork.Shoppingkart.GetAllExpression(u => u.ApplicationUserId == userId,
                includeProperties: "Product").ToList(),
                OrderHeader = new()

            };

            IEnumerable<ProductImage> productImages =
                _unitOfWork.ProductImage.GetAllExpression();

            foreach(var cart in shoppingCartVM.ShoppingKartsList)
            {
                cart.Product.ProductImages = productImages.Where(x => x.ProductId == cart.Product.ProductId).ToList();
                cart.Price = GetPriceBasedOnQuantity(cart);
                shoppingCartVM.OrderHeader.OrderTotal += (cart.Price * cart.Count);
                //shoppingCartVM.OrderHeader.OrderTotal = shoppingCartVM.OrderHeader.OrderTotal + (cart.Price * cart.Count);
            }
            return View(shoppingCartVM);
        }

        public IActionResult Plus(int cartId)
        {
            var cartFromDb = _unitOfWork.Shoppingkart.Get(u => u.Id == cartId);
            cartFromDb.Count = cartFromDb.Count + 1;
            _unitOfWork.Shoppingkart.Update(cartFromDb);
            _unitOfWork.Save();
            return RedirectToAction("Index");
        }

        public IActionResult Minus(int cartId)
        {
            var cartFromDb = _unitOfWork.Shoppingkart.Get(u => u.Id == cartId);
            if (cartFromDb.Count != null && cartFromDb.Count <= 1)
            {
                _unitOfWork.Shoppingkart.Remove(cartFromDb);
                return RedirectToAction("Index");
            }
            else
            {
                cartFromDb.Count = cartFromDb.Count - 1;
            }
            _unitOfWork.Shoppingkart.Update(cartFromDb);
            _unitOfWork.Save();
            return RedirectToAction("Index");
        }

        private decimal GetPriceBasedOnQuantity(ShoppingKart shoppingKart)
        {
            decimal finalPrice = 0;
            if(shoppingKart != null)
            {
                finalPrice= shoppingKart.Count * shoppingKart.Price;
            }
            return finalPrice;
        }
    }
}
