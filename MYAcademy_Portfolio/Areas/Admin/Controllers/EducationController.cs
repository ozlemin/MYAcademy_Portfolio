
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MYAcademy_Portfolio.Context;
using MYAcademy_Portfolio.Entities;

namespace MYAcademy_Portfolio.Areas.Admin.Controllers;

[Area("Admin")]
public class EducationController(PortfolioContext context) : Controller
{
    // Eğitimleri listeleme
    public IActionResult Index()
    {
        var values = context.Educations
            .AsNoTracking()
            .OrderByDescending(item => item.EducationId)
            .ToList();

        return View(values);
    }

    // Yeni eğitim ekleme sayfası
    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    // Yeni eğitim kaydetme
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create([Bind("Title,SubTitle,Description,Duration,IsActive")] Education item)
    {
        if (!ModelState.IsValid)
            return View(item);

        context.Educations.Add(item);
        context.SaveChanges();

        TempData["Success"] = "Eğitim kaydı başarıyla eklendi.";

        return RedirectToAction(nameof(Index));
    }

    // Eğitim düzenleme sayfası
    [HttpGet]
    public IActionResult Edit(int id)
    {
        var item = context.Educations.Find(id);

        if (item is null)
            return NotFound();

        return View(item);
    }

    // Eğitim güncelleme
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, [Bind("Title,SubTitle,Description,Duration,IsActive")] Education item)
    {
        if (!ModelState.IsValid)
        {
            item.EducationId = id;
            return View(item);
        }

        var existing = context.Educations.Find(id);

        if (existing is null)
            return NotFound();

        existing.Title = item.Title;
        existing.SubTitle = item.SubTitle;
        existing.Description = item.Description;
        existing.Duration = item.Duration;
        existing.IsActive = item.IsActive;

        context.SaveChanges();

        TempData["Success"] = "Eğitim kaydı başarıyla güncellendi.";

        return RedirectToAction(nameof(Index));
    }

    // Eğitim silme
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Delete(int id)
    {
        var item = context.Educations.Find(id);

        if (item is null)
            return NotFound();

        context.Educations.Remove(item);
        context.SaveChanges();

        TempData["Success"] = "Eğitim kaydı başarıyla silindi.";

        return RedirectToAction(nameof(Index));
    }
}
