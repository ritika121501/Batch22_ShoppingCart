using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShoppingCart.Entities;
using ShoppingCart.Models;
using ShoppingCart.Repository;
using ShoppingCart.ViewModels;

namespace ShoppingCart.Controllers
{
	public class ProductController : Controller
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IWebHostEnvironment _webHostEnvironment;

		public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
		{
			_unitOfWork = unitOfWork;
			_webHostEnvironment = webHostEnvironment;
		}
		// GET: ProductController
		public ActionResult Index()
		{
            JqueryDataTableParam param = new JqueryDataTableParam();
			param.Search = "256";
			param.DiaplayStart = 2;
			param.DisplayLength =100;

            List<Product> ProductList = _unitOfWork.Product.GetAllExpression().ToList();
			if(!string.IsNullOrEmpty(param.Search))
			{
				ProductList = ProductList.Where(x => x.ISBN.ToLower().Contains(param.Search.ToLower())
				|| x.Author.ToLower().Contains(param.Search.ToLower())
				|| x.Title.ToLower().Contains(param.Search.ToLower())
				|| x.Description.ToLower().Contains(param.Search.ToLower())).ToList();
            }

			ProductList = ProductList.Skip(param.DiaplayStart)
				.Take(param.DisplayLength).ToList();
			var totalRecords = ProductList.Count();
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

		[HttpGet]
		public IActionResult Upsert(int? id)
		{
			ProductVM productVM = new()
			{
				CategoryList = _unitOfWork.Category.GetAllExpression().Select(u => new SelectListItem
				{
					Text = u.Name,
					Value = u.CategoryId.ToString()
				}),
				Product = new Product()

			};

			if(id == null || id ==0)
			{
				//create
				return View(productVM);
			}
			else
			{
				productVM.Product = _unitOfWork.Product.Get(u => u.ProductId == id, includeProperties:"ProductImages");
				return View(productVM);
			}
		}

        [HttpPost]
        public IActionResult Upsert(ProductVM productVM, List<IFormFile> files)
        {
            if(ModelState.IsValid)
			{
				if(productVM.Product.ProductId ==0)
				{
					_unitOfWork.Product.Add(productVM.Product);
				}
				else
				{
					_unitOfWork.Product.Update(productVM.Product);
				}
				_unitOfWork.Save();

				string wwwRootPath = _webHostEnvironment.WebRootPath;
				if (files != null) 
				{
					foreach (IFormFile file in files) 
					{
						string fileName = Guid.NewGuid().ToString()+ Path.GetExtension(file.FileName);
						string productPath = @"images\products\product-" + productVM.Product.ProductId;
						string finalPath = Path.Combine(wwwRootPath, productPath);

						if (!Directory.Exists(finalPath)) { 
							Directory.CreateDirectory(finalPath);
						}
						using (var filestream = new FileStream(Path.Combine(finalPath, fileName), FileMode.Create))
						{
							file.CopyTo(filestream);
						}

						ProductImage productImage = new()
						{
							ImageUrl = @"\" + productPath + @"\" + fileName,
							ProductId = productVM.Product.ProductId,
						};

						if(productVM.Product.ProductImages == null)
						{
							productVM.Product.ProductImages = new List<ProductImage>();
						}

						productVM.Product.ProductImages.Add(productImage);

					}
					_unitOfWork.Product.Update(productVM.Product);
					_unitOfWork.Save();

				}

			}
			TempData["success"] = "Product created/updted successfully";
            return RedirectToAction("Index");
        }

		public IActionResult DeleteImage(int imageId)
		{
			var imageToBeDeleted =_unitOfWork.ProductImage.Get(u=>u.ProductId == imageId);
			if (imageToBeDeleted != null) {
				if (!string.IsNullOrEmpty(imageToBeDeleted.ImageUrl))
				{
					var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, imageToBeDeleted.ImageUrl.TrimStart('\\'));
					if (System.IO.File.Exists(oldImagePath))
					{
						System.IO.File.Delete(oldImagePath);
					}
				}
				_unitOfWork.ProductImage.Remove(imageToBeDeleted);
				_unitOfWork.Save();
			}

			return RedirectToAction("Index");
		}
    }
}
