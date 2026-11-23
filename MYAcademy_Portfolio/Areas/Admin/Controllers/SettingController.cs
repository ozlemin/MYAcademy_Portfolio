using Microsoft.AspNetCore.Mvc;
using MYAcademy_Portfolio.Context;
using MYAcademy_Portfolio.Entities;

namespace MYAcademy_Portfolio.Areas.Admin.Controllers;

[Area("Admin")]
public class SettingController(PortfolioContext context) : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        var hero = context.HeroSections.FirstOrDefault();
        string relativePath = hero?.CvFilePath ?? "/cv/cv.pdf";
        string fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", relativePath.TrimStart('/'));

        ViewBag.CvExists = System.IO.File.Exists(fullPath);
        ViewBag.CvPath = relativePath;

        if (ViewBag.CvExists)
        {
            var fileInfo = new FileInfo(fullPath);
            ViewBag.CvSize = $"{Math.Round(fileInfo.Length / 1024.0, 1)} KB";
            ViewBag.CvLastModified = fileInfo.LastWriteTime.ToString("dd MMMM yyyy HH:mm");
        }

        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult UploadCv(IFormFile? cvFile)
    {
        if (cvFile == null || cvFile.Length == 0)
        {
            TempData["Error"] = "Lütfen bir PDF dosyası seçiniz.";
            return RedirectToAction(nameof(Index));
        }

        var extension = Path.GetExtension(cvFile.FileName).ToLowerInvariant();
        if (extension != ".pdf")
        {
            TempData["Error"] = "Sadece .pdf uzantılı dosyalar yüklenebilir.";
            return RedirectToAction(nameof(Index));
        }

        if (cvFile.Length > 5 * 1024 * 1024)
        {
            TempData["Error"] = "CV dosya boyutu en fazla 5 MB olabilir.";
            return RedirectToAction(nameof(Index));
        }

        var cvFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "cv");
        if (!Directory.Exists(cvFolder))
        {
            Directory.CreateDirectory(cvFolder);
        }

        var filePath = Path.Combine(cvFolder, "cv.pdf");
        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            cvFile.CopyTo(stream);
        }

        var hero = context.HeroSections.FirstOrDefault();
        if (hero != null)
        {
            hero.CvFilePath = "/cv/cv.pdf";
            context.SaveChanges();
        }

        TempData["Success"] = "CV dosyası başarıyla yüklendi ve güncellendi.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
      public IActionResult DeleteCv()
    {
        var cvFolder = Path.Combine(
            Directory.GetCurrentDirectory(),
            "wwwroot",
            "cv"
        );

        var filePath = Path.Combine(cvFolder, "cv.pdf");

        if (System.IO.File.Exists(filePath))
        {
            System.IO.File.Delete(filePath);

            var hero = context.HeroSections.FirstOrDefault();

            if (hero != null)
            {
                hero.CvFilePath = null;
                context.SaveChanges();
            }

            TempData["Success"] = "CV dosyası başarıyla silindi.";
        }
        else
        {
            TempData["Error"] = "Silinecek CV dosyası bulunamadı.";
        }

        return RedirectToAction(nameof(Index));
    }

}
