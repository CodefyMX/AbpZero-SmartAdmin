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
        public async Task<ActionResult> Index(string slug)
        {
            var page = await _pages.GetPageViewBySlug(slug);
            if (page == null) return RedirectToAction("Index", "Home");
            return View(page);
        }
        public async Task<ActionResult> PageEditor(int id, string lang)
        {
            var page = await _pages.GetPageViewById(id, lang);
            return View(page);
        }

        public async Task<ActionResult> UpdateWithTemplate(int id, string lang, string templateName)
        {
            var page = await _pages.GetTemplateHtml(id, lang, templateName);
            return View(page);
        }
        public async Task<ActionResult> SavePage(PageContentInput input)
        {
            await _pages.SavePageContent(input);
            return Json(new { ok = true });
        }
    }
}