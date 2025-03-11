using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Entities;
using ShoppingCart.Entities.ViewModel;
using ShoppingCart.Repository;
using ShoppingCart.Utility;
using System.Security.Claims;

namespace ShoppingCart.Controllers
{
    public class CartController : Controller
    {
        private readonly ILogger<CartController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailSender _emailSender;

        public CartController(IUnitOfWork unitOfWork, ILogger<CartController> logger, IEmailSender emailSender)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _emailSender = emailSender;
        }
        public IActionResult Index()
        {
            
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            if (!claimsIdentity.IsAuthenticated) {
                return RedirectToPage("/Account/Login");
            }
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
                cart.Price = cart.Price;
                shoppingCartVM.OrderHeader.OrderTotal += (cart.Product.Price * cart.Count);
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
                finalPrice= shoppingKart.Count * shoppingKart.Product.Price;
            }
            return finalPrice;
        }

        [HttpGet]
        public IActionResult Summary()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            ShoppingCartVM shoppingCartVM = new ShoppingCartVM()
            {
                ShoppingKartsList = _unitOfWork.Shoppingkart.GetAllExpression(u => u.ApplicationUserId == userId,
                includeProperties: "Product").ToList(),
                OrderHeader = new()
            };

            shoppingCartVM.OrderHeader.ApplicationUser = _unitOfWork.ApplicationUser.Get(u=>u.Id == userId);

            shoppingCartVM.OrderHeader.Name = shoppingCartVM.OrderHeader.ApplicationUser.Name;
            shoppingCartVM.OrderHeader.PhoneNumber = shoppingCartVM.OrderHeader.ApplicationUser.PhoneNumber;
            shoppingCartVM.OrderHeader.StreetAddress = shoppingCartVM.OrderHeader.ApplicationUser.StreetAddress;
            shoppingCartVM.OrderHeader.City = shoppingCartVM.OrderHeader.ApplicationUser.City;
            shoppingCartVM.OrderHeader.Carrier = "Indigo";
            IEnumerable<ProductImage> productImages =
                _unitOfWork.ProductImage.GetAllExpression();

            foreach (var cart in shoppingCartVM.ShoppingKartsList)
            {
                cart.Product.ProductImages = productImages.Where(x => x.ProductId == cart.Product.ProductId).ToList();
                cart.Price = GetPriceBasedOnQuantity(cart);
                shoppingCartVM.OrderHeader.OrderTotal += (cart.Price);
            }
            return View(shoppingCartVM);
        }

        [HttpPost]
        [ActionName("Summary")]
        public IActionResult SummaryPOST()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            ShoppingCartVM shoppingCartVM = new ShoppingCartVM()
            {
                ShoppingKartsList = _unitOfWork.Shoppingkart.GetAllExpression(u => u.ApplicationUserId == userId,
                includeProperties: "Product").ToList(),
                OrderHeader = new()
            };

            var applicationUser = _unitOfWork.ApplicationUser.Get(u => u.Id == userId);
            shoppingCartVM.OrderHeader.ApplicationUserId = applicationUser.Id;
            shoppingCartVM.OrderHeader.Name = applicationUser.Name;
            shoppingCartVM.OrderHeader.PhoneNumber = applicationUser.PhoneNumber;
            shoppingCartVM.OrderHeader.StreetAddress = applicationUser.StreetAddress;
            shoppingCartVM.OrderHeader.City = applicationUser.City;
            shoppingCartVM.OrderHeader.Carrier = ShoppingCartUtility.CarrierIndigo;
          

            foreach (var cart in shoppingCartVM.ShoppingKartsList)
            {
                cart.Price = GetPriceBasedOnQuantity(cart);
                shoppingCartVM.OrderHeader.OrderTotal += (cart.Price);
                shoppingCartVM.OrderHeader.PaymentStatus = ShoppingCartUtility.PaymentStatusApproved;
                shoppingCartVM.OrderHeader.OrderStatus = ShoppingCartUtility.OrderStatusCompleted;
                shoppingCartVM.OrderHeader.OrderDate = DateTime.Now;
                shoppingCartVM.OrderHeader.PaymentDate = DateTime.Now;
                shoppingCartVM.OrderHeader.ShippingDate = DateTime.Now;
            }


            _unitOfWork.OrderHeader.Add(shoppingCartVM.OrderHeader);
            _unitOfWork.Save();

            foreach (var cart in shoppingCartVM.ShoppingKartsList)
            {
                OrderDetail orderDetail = new OrderDetail()
                {
                    ProductId = cart.ProductId,
                    OrderHeaderId = shoppingCartVM.OrderHeader.OrderHeaderId,
                    Price = cart.Price,
                    Count = cart.Count
                };
                _unitOfWork.OrderDetail.Add(orderDetail);
                _unitOfWork.Save();
            }

            return RedirectToAction(nameof(OrderConfirmation), new { id = shoppingCartVM.OrderHeader.OrderHeaderId });
        }

        public IActionResult OrderConfirmation(int id)
        {
            OrderHeader orderHeader = _unitOfWork.OrderHeader.Get(u=>u.OrderHeaderId == id,includeProperties:"ApplicationUser");
            if (orderHeader != null)
            {
                if(orderHeader.PaymentStatus == ShoppingCartUtility.PaymentStatusApproved)
                {
                    _emailSender.SendEmailAsync(orderHeader.ApplicationUser.Email, "", "");
                    List<ShoppingKart> carts = _unitOfWork.Shoppingkart.GetAllExpression(u=> u.ApplicationUserId == orderHeader.ApplicationUserId).ToList();
                    _unitOfWork.Shoppingkart.RemoveRange(carts);
                    _unitOfWork.Save();
                }
            }

            return View();
        }
    }
}
