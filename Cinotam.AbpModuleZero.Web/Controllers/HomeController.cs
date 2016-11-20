using Abp.Threading;
using Abp.Web.Security.AntiForgery;
using Cinotam.SimplePost.Application.Posts;
using Cinotam.SimplePost.Application.Posts.Dto;
using System.Globalization;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Cinotam.AbpModuleZero.Web.Controllers
{
    public class HomeController : AbpModuleZeroControllerBase
    {
        private readonly IPostAppService _postAppService;

        public HomeController(IPostAppService postAppService)
        {
            _postAppService = postAppService;
        }

        public ActionResult Index()
        {

            return View();
        }
        [HttpPost]
        [DisableAbpAntiForgeryTokenValidation]
        public async Task<ActionResult> Index(string title, string content, string lang)
        {
            await _postAppService.CreateEditPost(new NewPostInput()
            {
                Active = true,
                Content = new Content()
                {
                    ContentString = content,
                    Title = title,
                    Lang = lang
                }
            });
            return RedirectToAction("Index");
        }

        public ActionResult GetPost()
        {
            var posts = AsyncHelper.RunSync(() => _postAppService.GetPosts(CultureInfo.CurrentUICulture.Name));
            return View(posts);
        }

    }
}