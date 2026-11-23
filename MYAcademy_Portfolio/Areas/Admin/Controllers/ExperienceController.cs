
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MYAcademy_Portfolio.Context;
using MYAcademy_Portfolio.Entities;

namespace MYAcademy_Portfolio.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ExperienceController : Controller
    {
        private readonly PortfolioContext _context;

        public ExperienceController(PortfolioContext context)
        {
            _context = context;
        }

        // Deneyim Listesi
        public IActionResult ExperienceList()
        {
            var values = _context.Experiences
                .OrderByDescending(x => x.ExperienceId)
                .ToList();

            return View(values);
        }

        // Yeni Deneyim Ekleme - GET
        [HttpGet]
        public IActionResult CreateExperience()
        {
            return View();
        }

        // Yeni Deneyim Ekleme - POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateExperience(Experience experience)
        {
            if (!ModelState.IsValid)
            {
                return View(experience);
            }

            _context.Experiences.Add(experience);
            _context.SaveChanges();

            TempData["Success"] = "Deneyim kaydı başarıyla eklendi.";

            return RedirectToAction("ExperienceList");
        }

        // Deneyim Silme
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteExperience(int id)
        {
            var value = _context.Experiences.Find(id);

            if (value == null)
            {
                TempData["Error"] = "Silinecek deneyim kaydı bulunamadı.";
                return RedirectToAction("ExperienceList");
            }

            _context.Experiences.Remove(value);
            _context.SaveChanges();

            TempData["Success"] = "Deneyim kaydı başarıyla silindi.";

            return RedirectToAction("ExperienceList");
        }

        // Deneyim Güncelleme - GET
        [HttpGet]
        public IActionResult UpdateExperience(int id)
        {
            var value = _context.Experiences.Find(id);

            if (value == null)
            {
                return NotFound();
            }

            return View(value);
        }

        // Deneyim Güncelleme - POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateExperience(Experience experience)
        {
            if (!ModelState.IsValid)
            {
                return View(experience);
            }

            var value = _context.Experiences.Find(experience.ExperienceId);

            if (value == null)
            {
                return NotFound();
            }

            value.Title = experience.Title;
            value.CompanyName = experience.CompanyName;
            value.Duration = experience.Duration;
            value.Description = experience.Description;
            value.WorkType = experience.WorkType;
            value.SideTitle = experience.SideTitle;
            value.IsActive = experience.IsActive;

            _context.SaveChanges();

            TempData["Success"] = "Deneyim kaydı başarıyla güncellendi.";

            return RedirectToAction("ExperienceList");
        }
    }
}
