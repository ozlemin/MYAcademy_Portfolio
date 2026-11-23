using Microsoft.AspNetCore.Mvc;
using MYAcademy_Portfolio.Context;
using MYAcademy_Portfolio.Entities;

namespace MYAcademy_Portfolio.Controllers
{
    public class DefaultController : Controller
    {
        private readonly PortfolioContext _context;

        public DefaultController(PortfolioContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SendMessage(Message message)
        {
            if (ModelState.IsValid)
            {
                message.SendDate = DateTime.Now;
                message.IsRead = false;
                _context.Messages.Add(message);
                _context.SaveChanges();
                TempData["Success"] = "Mesajınız başarıyla iletildi!";
            }
            return RedirectToAction("Index");
        }

        public IActionResult DownloadCv()
        {
            var hero = _context.HeroSections.FirstOrDefault();
            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", (hero?.CvFilePath ?? "cv/cv.pdf").TrimStart('/'));

            if (!System.IO.File.Exists(path))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(path)!);
                System.IO.File.WriteAllText(path, "%PDF-1.4 Poyraz Alsever CV");
            }

            return PhysicalFile(path, "application/pdf", "Poyraz_Alsever_CV.pdf");
        }
    }
}
