using Microsoft.AspNetCore.Mvc;
using MVC_TEST.Data;
using MVC_TEST.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MVC_TEST.Controllers
{
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;


        public ProductController(AppDbContext context)
        {
            _context = context;
        }

        
        public IActionResult Index()
        {
            var products = _context.Product.Include(p => p.Category).ToList();
            return View(products); 
        }

        

        
        public ActionResult Create()
        {
            ViewBag.Categories = new SelectList(_context.Category, "CategoryId", "CategoryName");
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Product.Add(product);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Categories = new SelectList(_context.Category, "CategoryId", "CategoryName", product.CategoryId);
            return View(product);
        }

       
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _context.Product.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

           
            ViewBag.Categories = new SelectList(_context.Category.ToList(), "CategoryId", "CategoryName", product.CategoryId);
            return View(product);
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Product product)
        {
            if (id != product.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Product.Any(p => p.ProductId == product.ProductId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

           
            ViewBag.Categories = new SelectList(_context.Category.ToList(), "CategoryId", "CategoryName", product.CategoryId);
            return View(product);
        }

       
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _context.Product
                                         .Include(p => p.Category)
                                         .FirstOrDefaultAsync(p => p.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Product.FindAsync(id);
            _context.Product.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
