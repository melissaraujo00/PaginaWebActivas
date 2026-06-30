using Microsoft.AspNetCore.Mvc;
using AppWeb2.Models;

namespace AppWeb2.ViewComponents
{
    public class FiltrosPaginacionViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(FiltrosPaginacionViewModel model)
        {
            if (model == null)
                model = new FiltrosPaginacionViewModel();

            return View(model);
        }
    }
}