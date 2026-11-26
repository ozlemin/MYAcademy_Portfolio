
using Microsoft.AspNetCore.Mvc;
using MYAcademy_Portfolio.Context;

namespace MYAcademy_Portfolio.ViewComponents.DefaultViewComponents
{
    public class _DefaultNavbarComponentPartial(PortfolioContext context) : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            var hero = context.HeroSections.FirstOrDefault();

            return View(hero);
        }
    }
}
