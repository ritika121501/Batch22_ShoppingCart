using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShoppingCart.Models;
using ShoppingCart.Repository;
using ShoppingCart.ViewModels;

namespace ShoppingCart.Controllers
{
	public class ProductController : Controller
	{
		private readonly IUnitOfWork _unitOfWork;

		public ProductController(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}
		// GET: ProductController
		public ActionResult Index()
		{
			List<Product> ProductList = _unitOfWork.Product.GetAllExpression().ToList();
			return View(ProductList);
		}

		// GET: ProductController/Details/5
		public ActionResult Details(int id)
		{
			return View();
		}

		// GET: ProductController/Create
		public ActionResult Create()
		{
			ProductVM productVM = new ProductVM()
			{
				CategoryList = _unitOfWork.Category.GetAllExpression().Select(u => new SelectListItem
				{
					Text = u.Name,
					Value = u.CategoryId.ToString()
				}),
				Product =new Product()
			};
			return View(productVM);
		}

		// POST: ProductController/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(Product Product)
		{
			try
			{
				if (Product == null) 
				{
					return RedirectToAction(nameof(Index));
				}

				if(ModelState.IsValid)
				{
					_unitOfWork.Product.Add(Product);
					_unitOfWork.Save();
					TempData["Success"] = "Product created successfully";
					return RedirectToAction("Index");
				}
				return View();
			}
			catch
			{
				return View();
			}
		}

		// GET: ProductController/Edit/5
		public ActionResult Edit(int id)
		{
			if(id==0)
			{
				return NotFound();
			}
			var checkProduct = _unitOfWork.Product.Get(x => x.ProductId == id);
			if(checkProduct == null)
			{
				return NotFound();
			}
			return View(checkProduct);
		}

		// POST: ProductController/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(Product Product)
		{
			try
			{
                if (ModelState.IsValid)
                {
                    _unitOfWork.Product.Update(Product);
                    _unitOfWork.Save();
                    TempData["Success"] = "Product updated successfully";
                    return RedirectToAction("Index");
                }
                return View();
            }
			catch
			{
				return View();
			}
		}

		// GET: ProductController/Delete/5
		public ActionResult Delete(int id)
		{
            if (id == 0)
            {
                return NotFound();
            }
            var checkProduct = _unitOfWork.Product.Get(x => x.ProductId == id);
            if (checkProduct == null)
            {
                return NotFound();
            }
            return View(checkProduct);
        }

		// POST: ProductController/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public ActionResult DeletePost(int id)
		{
			try
			{
                var checkProduct = _unitOfWork.Product.Get(x => x.ProductId == id);
				if (checkProduct == null)
				{
					return NotFound();
				}
				_unitOfWork.Product.Remove(checkProduct);
				_unitOfWork.Save();
				TempData["Success"] = "Product delete successully";
				return RedirectToAction("Index");
            }
			catch
			{
				return View();
			}
		}
	}
}
