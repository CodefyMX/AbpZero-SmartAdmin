﻿using Cinotam.Cms.App.Pages;
using Cinotam.Cms.App.Pages.Dto;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Cinotam.AbpModuleZero.Web.Controllers
{
    public class PagesController : AbpModuleZeroControllerBase
    {
        private readonly IPagesService _pages;

        public PagesController(IPagesService pages)
        {
            _pages = pages;
        }
        [Route("Page/{slug}")]
        public ActionResult Index(string slug)
        {
            var page = _pages.GetPageViewBySlug(slug);
            if (page == null) return RedirectToAction("Index", "Home");
            return View(page);
        }
        public async Task<ActionResult> PageEditor(int id, string lang)
        {
            var page = await _pages.GetPageViewById(id, lang);
            return View(page);
        }

        public async Task<ActionResult> SavePage(PageContentInput input)
        {
            await _pages.SavePageContent(input);
            return Json(new { ok = true });
        }
    }
}