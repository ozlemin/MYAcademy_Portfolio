using Microsoft.AspNetCore.Mvc;
using MYAcademy_Portfolio.Context;
using MYAcademy_Portfolio.Entities;

namespace MYAcademy_Portfolio.Areas.Admin.Controllers;

[Area("Admin")]
public class ProfileController(PortfolioContext context) : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        var about = context.Abouts.FirstOrDefault();
        if (about == null)
        {
            about = new About
            {
                NameSurname = "Özlem Bozkurt",
                JobTitle = "Full Stack .NET Developer",
                Email = "ozlem@bozkurt.com",
                Location = "İstanbul, Türkiye",
                Expert = "C#, ASP.NET Core, MSSQL",
                Description = "Yazılım geliştirici",
                OpenToWork = "Yeni iş fırsatlarına açığım."
            };
            context.Abouts.Add(about);
            context.SaveChanges();
        }

        return View(about);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Index(About model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var existing = context.Abouts.Find(model.AboutId);
        if (existing == null)
            return NotFound();

        existing.NameSurname = model.NameSurname;
        existing.JobTitle = model.JobTitle;
        existing.Location = model.Location;
        existing.Expert = model.Expert;
        existing.Email = model.Email;
        existing.Description = model.Description;
        existing.OpenToWork = model.OpenToWork;

        context.SaveChanges();

        TempData["Success"] = "Profil bilgileriniz başarıyla güncellendi.";
        return RedirectToAction(nameof(Index));
    }
}
