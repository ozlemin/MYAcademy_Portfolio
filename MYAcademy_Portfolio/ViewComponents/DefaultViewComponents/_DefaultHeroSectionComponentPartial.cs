using Microsoft.AspNetCore.Mvc;
using MYAcademy_Portfolio.Context;
using MYAcademy_Portfolio.Entities;

namespace MYAcademy_Portfolio.ViewComponents.DefaultViewComponents
{
    public class _DefaultHeroSectionComponentPartial : ViewComponent
    {
        private readonly PortfolioContext _context;
        public _DefaultHeroSectionComponentPartial(PortfolioContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            var dbRow = _context.HeroSections.FirstOrDefault();

            var values = new HeroSection
            {
                HeroSectionId = dbRow?.HeroSectionId ?? 0,
                Title       = dbRow?.Title       ?? "Yazılım Geliştirici & ASP.NET Core Mimarı",
                SubTitle    = dbRow?.SubTitle    ?? "Özlem Bozkurt",
                Description = dbRow?.Description ?? "Modern, ölçeklenebilir ve kullanıcı odaklı kurumsal web çözümleri geliştiriyorum.",
                ImageUrl    = dbRow?.ImageUrl    ?? "https://images.unsplash.com/photo-1534528741775-53994a69daeb?auto=format&fit=crop&w=600&q=80",
                CvFilePath  = dbRow?.CvFilePath  ?? "/cv/cv.pdf"
            };

            ViewBag.v1 = values.Title;
            ViewBag.v2 = values.SubTitle;
            ViewBag.v3 = values.Description;
            ViewBag.v4 = values.ImageUrl;
            ViewBag.CvFilePath = string.IsNullOrEmpty(values.CvFilePath) ? "/cv/cv.pdf" : values.CvFilePath;

            return View(values);
        }
    }
}
