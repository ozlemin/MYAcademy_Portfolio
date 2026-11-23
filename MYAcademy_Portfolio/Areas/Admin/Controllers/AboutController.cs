
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MYAcademy_Portfolio.Context;
using MYAcademy_Portfolio.Entities;

namespace MYAcademy_Portfolio.Areas.Admin.Controllers;

[Area("Admin")]
public class AboutController(PortfolioContext context) : Controller
{
    public IActionResult Index()
    {
        return RedirectToAction(nameof(AboutList));
    }

    // Hakkımda bilgilerini listeleme
    public IActionResult AboutList()
    {
        var values = context.Abouts
            .AsNoTracking()
            .OrderBy(item => item.AboutId)
            .ToList();

        return View(values);
    }

    // Yeni hakkımda bilgisi ekleme sayfası
    [HttpGet]
    public IActionResult CreateAbout()
    {
        if (context.Abouts.Any())
            return RedirectToAction(nameof(AboutList));

        return View();
    }

    // Yeni hakkımda bilgisi kaydetme
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult CreateAbout(
        [Bind("NameSurname,JobTitle,Location,Expert,Email,Description,OpenToWork")]
        About item)
    {
        if (context.Abouts.Any())
            return RedirectToAction(nameof(AboutList));

        if (!ModelState.IsValid)
            return View(item);

        context.Abouts.Add(item);
        context.SaveChanges();

        TempData["Success"] = "Hakkımda bilgileri başarıyla eklendi.";

        return RedirectToAction(nameof(AboutList));
    }

    // Hakkımda bilgisi düzenleme sayfası
    [HttpGet]
    public IActionResult EditAbout(int id)
    {
        var item = context.Abouts.Find(id);

        if (item is null)
            return NotFound();

        return View(item);
    }

    // Hakkımda bilgisi güncelleme
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult EditAbout(
        int id,
        [Bind("NameSurname,JobTitle,Location,Expert,Email,Description,OpenToWork")]
        About item)
    {
        if (!ModelState.IsValid)
        {
            item.AboutId = id;
            return View(item);
        }

        var existing = context.Abouts.Find(id);

        if (existing is null)
            return NotFound();

        existing.NameSurname = item.NameSurname;
        existing.JobTitle = item.JobTitle;
        existing.Location = item.Location;
        existing.Expert = item.Expert;
        existing.Email = item.Email;
        existing.Description = item.Description;
        existing.OpenToWork = item.OpenToWork;

        context.SaveChanges();

        TempData["Success"] = "Hakkımda bilgileri başarıyla güncellendi.";

        return RedirectToAction(nameof(AboutList));
    }

    // Hakkımda bilgisi silme
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteAbout(int id)
    {
        var item = context.Abouts.Find(id);

        if (item is null)
            return NotFound();

        context.Abouts.Remove(item);
        context.SaveChanges();

        TempData["Success"] = "Hakkımda bilgileri başarıyla silindi.";

        return RedirectToAction(nameof(AboutList));
    }
}
