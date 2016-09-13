using Cinotam.Cms.App.Pages;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Cinotam.AbpModuleZero.Web.Controllers
{

    public class HomeController : AbpModuleZeroControllerBase
    {
        private readonly IPagesService _pages;

        public HomeController(IPagesService pages)
        {
            _pages = pages;
        }

        public async Task<ActionResult> Index()
        {
            var url = await _pages.GetMainPageSlug();
            if (string.IsNullOrEmpty(url)) return View();
            return RedirectToAction("Index", "Pages", new { slug = url });
        }

    }
}