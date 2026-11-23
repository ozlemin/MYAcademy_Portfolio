using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MYAcademy_Portfolio.Areas.Admin.Models;
using MYAcademy_Portfolio.Context;

namespace MYAcademy_Portfolio.Areas.Admin.Controllers;

[Area("Admin")]
public class DashboardController(PortfolioContext context) : Controller
{
    public IActionResult Index()
    {
        var model = new DashboardViewModel
        {
            // Sayısal İstatistikler
            TotalProjectCount = context.Projects.Count(),
            ActiveProjectCount = context.Projects.Count(x => x.IsActive),
            TotalSkillCount = context.Skills.Count(),
            TotalMessageCount = context.Messages.Count(),
            UnreadMessageCount = context.Messages.Count(x => !x.IsRead),
            ReadMessageCount = context.Messages.Count(x => x.IsRead),
            CategoryCount = context.Categories.Count(),
            ExperienceCount = context.Experiences.Count(),
            EducationCount = context.Educations.Count(),

            // Son 5 Mesaj
            LatestMessages = context.Messages
                .AsNoTracking()
                .OrderByDescending(x => x.SendDate)
                .Take(5)
                .ToList(),

            // Son 4 Proje
            LatestProjects = context.Projects
                .AsNoTracking()
                .Include(x => x.Category)
                .OrderByDescending(x => x.ProjectId)
                .Take(4)
                .ToList(),

            // Kategoriye göre proje dağılımı
            CategoryProjectCounts = context.Categories
                .AsNoTracking()
                .Include(c => c.Projects)
                .ToDictionary(c => c.CategoryName, c => c.Projects.Count)
        };

        // Geriye dönük uyumluluk için ViewBag değerleri
        ViewBag.messageCount = model.TotalMessageCount;
        ViewBag.messageReadByTrueCount = model.ReadMessageCount;
        ViewBag.messageReadByFalseCount = model.UnreadMessageCount;

        return View(model);
    }
}
