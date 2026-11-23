
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MYAcademy_Portfolio.Context;
using MYAcademy_Portfolio.Entities;

namespace MYAcademy_Portfolio.Areas.Admin.Controllers;

[Area("Admin")]
public class ProjectController(
    PortfolioContext context,
    IWebHostEnvironment environment) : Controller
{
    private const string ImageFolder = "images/projects";
    private const long MaxFileSize = 5 * 1024 * 1024;

    private static readonly string[] AllowedExtensions =
        { ".jpg", ".jpeg", ".png", ".webp" };

    public IActionResult Index() => RedirectToAction(nameof(ProjectList));

    // Proje listesi
    public IActionResult ProjectList()
    {
        var values = context.Projects
            .AsNoTracking()
            .Include(x => x.Category)
            .OrderByDescending(x => x.ProjectId)
            .ToList();

        return View(values);
    }

    // Kategorileri dropdown için hazırlama
    private void LoadCategories()
    {
        ViewBag.Categories = new SelectList(
            context.Categories
                .AsNoTracking()
                .OrderBy(x => x.CategoryName)
                .ToList(),
            "CategoryId",
            "CategoryName");
    }

    // Görsel yükleme
    private string? UploadImage(IFormFile? file)
    {
        if (file == null || file.Length == 0)
            return null;

        if (file.Length > MaxFileSize)
            throw new InvalidOperationException("Görsel boyutu en fazla 5 MB olabilir.");

        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

        if (!AllowedExtensions.Contains(extension))
            throw new InvalidOperationException("Sadece JPG, JPEG, PNG ve WEBP dosyaları yüklenebilir.");

        var webRoot = environment.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
        var folderPath = Path.Combine(webRoot, ImageFolder);

        Directory.CreateDirectory(folderPath);

        var fileName = Guid.NewGuid().ToString("N") + extension;

        var filePath = Path.Combine(folderPath, fileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            file.CopyTo(stream);
        }

        return "/" + ImageFolder + "/" + fileName;
    }

    // Yüklenen eski görseli silme
    private void DeleteImage(string? imageUrl)
    {
        if (string.IsNullOrWhiteSpace(imageUrl))
            return;

        var fileName = Path.GetFileName(imageUrl);

        if (string.IsNullOrWhiteSpace(fileName))
            return;

        var webRoot = environment.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
        var filePath = Path.Combine(webRoot, ImageFolder, fileName);

        if (System.IO.File.Exists(filePath))
            System.IO.File.Delete(filePath);
    }

    // Yeni proje ekleme sayfası
    [HttpGet]
    public IActionResult CreateProject()
    {
        LoadCategories();
        return View();
    }

    // Yeni proje kaydetme
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult CreateProject(
        [Bind("Title,Description,GitHubUrl,IsActive,CategoryId")] Project item,
        IFormFile? coverImage)
    {
        if (!context.Categories.Any(x => x.CategoryId == item.CategoryId))
        {
            ModelState.AddModelError("CategoryId", "Lütfen geçerli bir kategori seçiniz.");
        }

        if (!ModelState.IsValid)
        {
            LoadCategories();
            return View(item);
        }

        try
        {
            item.CoverImageUrl = UploadImage(coverImage) ?? "";

            context.Projects.Add(item);
            context.SaveChanges();

            TempData["Success"] = "Proje başarıyla eklendi.";

            return RedirectToAction(nameof(ProjectList));
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError("CoverImageUrl", ex.Message);
            LoadCategories();
            return View(item);
        }
    }

    // Proje güncelleme sayfası
    [HttpGet]
    public IActionResult UpdateProject(int id)
    {
        var item = context.Projects.Find(id);

        if (item == null)
            return NotFound();

        LoadCategories();
        return View(item);
    }

    // Proje güncelleme
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult UpdateProject(
        int id,
        [Bind("ProjectId,Title,Description,GitHubUrl,IsActive,CategoryId")] Project item,
        IFormFile? coverImage)
    {
        if (id != item.ProjectId)
            return BadRequest();

        if (!context.Categories.Any(x => x.CategoryId == item.CategoryId))
        {
            ModelState.AddModelError("CategoryId", "Lütfen geçerli bir kategori seçiniz.");
        }

        if (!ModelState.IsValid)
        {
            LoadCategories();
            return View(item);
        }

        var existing = context.Projects.Find(id);

        if (existing == null)
            return NotFound();

        try
        {
            if (coverImage != null && coverImage.Length > 0)
            {
                var newImage = UploadImage(coverImage);

                DeleteImage(existing.CoverImageUrl);

                existing.CoverImageUrl = newImage!;
            }

            existing.Title = item.Title;
            existing.Description = item.Description;
            existing.GitHubUrl = item.GitHubUrl;
            existing.CategoryId = item.CategoryId;
            existing.IsActive = item.IsActive;

            context.SaveChanges();

            TempData["Success"] = "Proje başarıyla güncellendi.";

            return RedirectToAction(nameof(ProjectList));
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError("CoverImageUrl", ex.Message);
            LoadCategories();
            return View(item);
        }
    }

    // Proje silme
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteProject(int id)
    {
        var item = context.Projects.Find(id);

        if (item == null)
            return NotFound();

        DeleteImage(item.CoverImageUrl);

        context.Projects.Remove(item);
        context.SaveChanges();

        TempData["Success"] = "Proje başarıyla silindi.";

        return RedirectToAction(nameof(ProjectList));
    }
}
