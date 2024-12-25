using Microsoft.AspNetCore.Mvc;

using Microsoft.EntityFrameworkCore;
using MVC_TEST.Data;

public class CategoryController : Controller
{
    private readonly AppDbContext _context;

    public CategoryController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var category = await _context.Category.ToListAsync();
        return View(category);
    }
}