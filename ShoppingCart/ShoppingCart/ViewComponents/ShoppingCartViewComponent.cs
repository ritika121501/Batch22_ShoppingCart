using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Repository;
using System.Security.Claims;
namespace ShoppingCart.ViewComponents
{
    public class ShoppingCartViewComponent : ViewComponent
    {

        private readonly IUnitOfWork _unitOfWork;
        public ShoppingCartViewComponent(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            if (claim != null)
            {
                var count = _unitOfWork.Shoppingkart.GetAllExpression(u => u.ApplicationUserId == claim.Value).Count();
                return View(count);
            }
            return View(0);
        }
    }
}
