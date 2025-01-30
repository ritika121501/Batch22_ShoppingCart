using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Entities;
using ShoppingCart.Repository;

namespace ShoppingCart.Controllers
{
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        // GET: CategoryController
        public ActionResult Index()
        {
            List<Category> categoryList = _unitOfWork.Category.GetAllExpression().ToList();
            return View(categoryList);
        }

        // GET: CategoryController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CategoryController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CategoryController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Category category)
        {
            try
            {
                if (category == null)
                {
                    return RedirectToAction(nameof(Index));
                }

                if (ModelState.IsValid)
                {
                    _unitOfWork.Category.Add(category);
                    _unitOfWork.Save();
                    TempData["Success"] = "Category created successfully";
                    return RedirectToAction("Index");
                }
                return View();
            }
            catch
            {
                return View();
            }
        }

        // GET: CategoryController/Edit/5
        public ActionResult Edit(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }
            var checkCategory = _unitOfWork.Category.Get(x => x.CategoryId == id);
            if (checkCategory == null)
            {
                return NotFound();
            }
            return View(checkCategory);
        }

        // POST: CategoryController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Category category)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _unitOfWork.Category.Update(category);
                    _unitOfWork.Save();
                    TempData["Success"] = "Category updated successfully";
                    return RedirectToAction("Index");
                }
                return View();
            }
            catch
            {
                return View();
            }
        }

        // GET: CategoryController/Delete/5
        public ActionResult Delete(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }
            var checkCategory = _unitOfWork.Category.Get(x => x.CategoryId == id);
            if (checkCategory == null)
            {
                return NotFound();
            }
            return View(checkCategory);
        }

        // POST: CategoryController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeletePost(int id)
        {
            try
            {
                var checkCategory = _unitOfWork.Category.Get(x => x.CategoryId == id);
                if (checkCategory == null)
                {
                    return NotFound();
                }
                _unitOfWork.Category.Remove(checkCategory);
                _unitOfWork.Save();
                TempData["Success"] = "Category delete successully";
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
