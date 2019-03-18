using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShoppingCart.Data;
using ShoppingCart.Data.Interfaces;
using ShoppingCart.Models;
using ShoppingCart.ViewModels;

namespace ShoppingCart.Controllers
{
    public class ProductController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IProductRepository _productRepository;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly ApplicationDbContext _db;

        [BindProperty]
        public ProductViewModel ProductVM { get; set; }
        public ProductController(ICategoryRepository categoryRepository, IProductRepository productRepository, IHostingEnvironment hostingEnvironment, ApplicationDbContext db)
        {
            _categoryRepository = categoryRepository;
            _productRepository = productRepository;
            _hostingEnvironment = hostingEnvironment;
            ProductVM = new ProductViewModel()
            {
                Categories = _categoryRepository.GetAll(),
                Product = new Models.Product()
            };
        }

        public IActionResult Index()
        {
            return View(_productRepository.GetAllWithCategory());
        }

        public IActionResult Details(int id)
        {
            var product = _productRepository.GetWithCategoryById(id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        public IActionResult Create()
        {
            return View(ProductVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAsync(Product product)
        {
            if (ModelState.IsValid)
            {
                _productRepository.Create(product);
                return RedirectToAction(nameof(Index));
            }

            ViewData["CategoryId"] = new SelectList(_categoryRepository.GetAll(), "Id", "Name", product.CategoryId);
            string webRootPath = _hostingEnvironment.WebRootPath;
            var files = HttpContext.Request.Form.Files;

            var ProductFromDB = _productRepository.GetById(ProductVM.Product.Id);

            if (files.Count > 0)
            {
                var uploads = Path.Combine(webRootPath, "images");
                var extension = Path.GetExtension(files[0].FileName);

                using (var filesStream = new FileStream(Path.Combine(uploads, ProductVM.Product.Id + extension), FileMode.Create))
                {
                    files[0].CopyTo(filesStream);
                }

                ProductFromDB.Image = @"\images\" + ProductVM.Product.Id + extension;
             }

            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int id)
        {
            var product = _productRepository.GetWithCategoryById(id);

            if (product == null)
            {
                return NotFound();
            }

            ProductVM.Product = product;
            ProductVM.Categories = _categoryRepository.GetAll().Where(c =>c.Id.Equals(product.CategoryId));
            return View(ProductVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Product product)
        {
            _productRepository.Update(product);
            ViewData["CategoryId"] = new SelectList(_categoryRepository.GetAll(), "Id", "Name", product.CategoryId);
            return View(product);
        }

        public IActionResult Delete(int id)
        {
            var product = _productRepository.GetWithCategoryById(id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var product = _productRepository.GetById(id);
            _productRepository.Delete(product);
            return RedirectToAction(nameof(Index));
        }
    }
}