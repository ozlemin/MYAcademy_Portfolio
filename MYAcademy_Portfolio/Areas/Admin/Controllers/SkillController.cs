using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MYAcademy_Portfolio.Context;
using MYAcademy_Portfolio.Entities;

namespace MYAcademy_Portfolio.Areas.Admin.Controllers;

[Area("Admin")]
public class SkillController(PortfolioContext context) : Controller
{
    public IActionResult Index()
    {
        return RedirectToAction(nameof(SkillList));
    }

    // Yetenekleri listeleme
    public IActionResult SkillList()
    {
        var values = context.Skills
            .AsNoTracking()
            .OrderByDescending(item => item.SkillId)
            .ToList();

        return View(values);
    }

    // Yeni yetenek ekleme sayfası
    [HttpGet]
    public IActionResult CreateSkill()
    {
        return View();
    }

    // Yeni yetenek kaydetme
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult CreateSkill(
        [Bind("Name,Percentage,Category,IsActive")] Skill item)
    {
        if (!ModelState.IsValid)
            return View(item);

        context.Skills.Add(item);
        context.SaveChanges();

        TempData["Success"] = "Yetenek başarıyla eklendi.";

        return RedirectToAction(nameof(SkillList));
    }

    // Yetenek düzenleme sayfası
    [HttpGet]
    public IActionResult EditSkill(int id)
    {
        var item = context.Skills.Find(id);

        if (item is null)
            return NotFound();

        return View(item);
    }

    // Yetenek güncelleme
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult EditSkill(
        int id,
        [Bind("Name,Percentage,Category,IsActive")] Skill item)
    {
        if (!ModelState.IsValid)
        {
            item.SkillId = id;
            return View(item);
        }

        var existing = context.Skills.Find(id);

        if (existing is null)
            return NotFound();

        existing.Name = item.Name;
        existing.Percentage = item.Percentage;
        existing.Category = item.Category;
        existing.IsActive = item.IsActive;

        context.SaveChanges();

        TempData["Success"] = "Yetenek başarıyla güncellendi.";

        return RedirectToAction(nameof(SkillList));
    }

    // Yetenek silme
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteSkill(int id)
    {
        var item = context.Skills.Find(id);

        if (item is null)
            return NotFound();

        context.Skills.Remove(item);
        context.SaveChanges();

        TempData["Success"] = "Yetenek başarıyla silindi.";

        return RedirectToAction(nameof(SkillList));
    }
}
