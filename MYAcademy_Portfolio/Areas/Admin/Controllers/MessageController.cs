using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MYAcademy_Portfolio.Context;
using MYAcademy_Portfolio.Entities;

namespace MYAcademy_Portfolio.Areas.Admin.Controllers;

[Area("Admin")]
public class MessageController(PortfolioContext context) : Controller
{
    public IActionResult Index() => RedirectToAction(nameof(MessageList));

    // Mesaj listesi (Arama ve filtreleme destekli)
    public IActionResult MessageList(string? search, string? filter)
    {
        var query = context.Messages.AsNoTracking().AsQueryable();

        // Arama filtresi
        if (!string.IsNullOrWhiteSpace(search))
        {
            var s = search.Trim();
            query = query.Where(x =>
                x.NameSurname.Contains(s) ||
                x.Email.Contains(s) ||
                x.Subject.Contains(s) ||
                x.MessageDetail.Contains(s));
            ViewBag.CurrentSearch = s;
        }

        // Okundu / Okunmadı filtreleme
        if (filter == "unread")
        {
            query = query.Where(x => !x.IsRead);
            ViewBag.CurrentFilter = "unread";
        }
        else if (filter == "read")
        {
            query = query.Where(x => x.IsRead);
            ViewBag.CurrentFilter = "read";
        }
        else
        {
            ViewBag.CurrentFilter = "all";
        }

        // İstatistikler
        ViewBag.TotalCount = context.Messages.Count();
        ViewBag.UnreadCount = context.Messages.Count(x => !x.IsRead);
        ViewBag.ReadCount = context.Messages.Count(x => x.IsRead);

        var values = query
            .OrderByDescending(x => x.SendDate)
            .ToList();

        return View(values);
    }

    // Modal için JSON veri döndürme & otomatik okundu işaretleme
    [HttpGet]
    public IActionResult GetMessageDetail(int id)
    {
        var message = context.Messages.Find(id);
        if (message == null)
            return NotFound(new { success = false, message = "Mesaj bulunamadı." });

        if (!message.IsRead)
        {
            message.IsRead = true;
            context.SaveChanges();
        }

        return Json(new
        {
            success = true,
            messageId = message.MessageId,
            nameSurname = message.NameSurname,
            email = message.Email,
            subject = message.Subject,
            detail = message.MessageDetail,
            date = message.SendDate.ToString("dd MMMM yyyy HH:mm"),
            isRead = message.IsRead
        });
    }

    // Doğrudan URL ile erişilirse (fallback)
    [HttpGet]
    public IActionResult MessageDetail(int id)
    {
        var message = context.Messages.Find(id);
        if (message == null)
            return NotFound();

        if (!message.IsRead)
        {
            message.IsRead = true;
            context.SaveChanges();
        }

        return RedirectToAction(nameof(MessageList));
    }

    // Okundu / Okunmadı durumunu tersine çevirme
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult ToggleReadStatus(int id)
    {
        var message = context.Messages.Find(id);

        if (message == null)
        {
            TempData["Error"] = "Mesaj bulunamadı.";
            return RedirectToAction(nameof(MessageList));
        }

        message.IsRead = !message.IsRead;
        context.SaveChanges();

        TempData["Success"] = message.IsRead
            ? "Mesaj okundu olarak işaretlendi."
            : "Mesaj okunmadı olarak işaretlendi.";

        return RedirectToAction(nameof(MessageList));
    }

    // Yeni mesaj ekleme (Admin tarafından test için)
    [HttpGet]
    public IActionResult CreateMessage()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult CreateMessage(Message message)
    {
        if (!ModelState.IsValid)
            return View(message);

        message.SendDate = DateTime.Now;
        message.IsRead = false;

        context.Messages.Add(message);
        context.SaveChanges();

        TempData["Success"] = "Mesaj başarıyla kaydedildi.";
        return RedirectToAction(nameof(MessageList));
    }

    // Mesaj silme
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteMessage(int id)
    {
        var value = context.Messages.Find(id);
        if (value != null)
        {
            context.Messages.Remove(value);
            context.SaveChanges();
            TempData["Success"] = "Mesaj başarıyla silindi.";
        }

        return RedirectToAction(nameof(MessageList));
    }
}
