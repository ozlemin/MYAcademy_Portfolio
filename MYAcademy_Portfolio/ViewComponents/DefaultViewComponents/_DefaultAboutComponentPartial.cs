using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MYAcademy_Portfolio.Context;
using MYAcademy_Portfolio.Entities;

namespace MYAcademy_Portfolio.ViewComponents.DefaultViewComponents;

public class _DefaultAboutComponentPartial(PortfolioContext context) : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        var about = context.Abouts
            .AsNoTracking()
            .OrderBy(item => item.AboutId)
            .FirstOrDefault() ?? new About();

        return View(about);
    }
}