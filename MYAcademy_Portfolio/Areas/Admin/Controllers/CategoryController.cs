
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MYAcademy_Portfolio.Context;
using MYAcademy_Portfolio.Entities;

namespace MYAcademy_Portfolio.Areas.Admin.Controllers;

[Area("Admin")]
public class CategoryController(PortfolioContext context) : Controller
{
    public IActionResult Index() => RedirectToAction(nameof(CategoryList));

    // Kategori listesi
    public IActionResult CategoryList()
    {
        var values = context.Categories
            .AsNoTracking()
            .OrderByDescending(x => x.CategoryId)
            .ToList();

        return View(values);
    }

    // Yeni kategori ekleme sayfası
    [HttpGet]
    public IActionResult CreateCategory()
    {
        return View();
    }

    // Yeni kategori kaydetme
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult CreateCategory(Category item)
    {
        if (!ModelState.IsValid)
            return View(item);

        context.Categories.Add(item);
        context.SaveChanges();

        TempData["Success"] = "Kategori başarıyla eklendi.";

        return RedirectToAction(nameof(CategoryList));
    }

    // Kategori düzenleme sayfası
    [HttpGet]
    public IActionResult EditCategory(int id)
    {
        var item = context.Categories.Find(id);

        if (item == null)
            return NotFound();

        return View(item);
    }

    // Kategori güncelleme
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult EditCategory(int id, Category item)
    {
        if (!ModelState.IsValid)
        {
            item.CategoryId = id;
            return View(item);
        }

        var existing = context.Categories.Find(id);

        if (existing == null)
            return NotFound();

        existing.CategoryName = item.CategoryName;

        context.SaveChanges();

        TempData["Success"] = "Kategori başarıyla güncellendi.";

        return RedirectToAction(nameof(CategoryList));
    }

    // Kategori silme
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteCategory(int id)
    {
        var item = context.Categories
            .Include(x => x.Projects)
            .FirstOrDefault(x => x.CategoryId == id);

        if (item == null)
            return NotFound();

        if (item.Projects != null && item.Projects.Any())
        {
            TempData["Error"] = "Bu kategoriye bağlı projeler bulunduğu için silinemez!";
            return RedirectToAction(nameof(CategoryList));
        }

        context.Categories.Remove(item);
        context.SaveChanges();

        TempData["Success"] = "Kategori başarıyla silindi.";

        return RedirectToAction(nameof(CategoryList));
    }
}
