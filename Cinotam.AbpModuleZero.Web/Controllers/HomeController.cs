using Abp.Threading;
using Abp.Web.Security.AntiForgery;
using Cinotam.FileManager.Service.AppService;
using Cinotam.FileManager.Service.AppService.Dto;
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
        private readonly IFileManagerAppService _fileManagerAppService;
        public HomeController(IPostAppService postAppService, IFileManagerAppService fileManagerAppService)
        {
            _postAppService = postAppService;
            _fileManagerAppService = fileManagerAppService;
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

        public ActionResult AddAttachment(int id)
        {
            ViewBag.Id = id;
            return View();
        }

        [HttpPost]
        [DisableAbpAntiForgeryTokenValidation]
        public async Task<ActionResult> AddAttachment(int id, FormCollection forms)
        {

            var file = Request.Files[0];

            var fileInfo = await _fileManagerAppService.SaveFile(new SaveFileInput(file));

            await _postAppService.AddAttachment(new PostAttachmentInput()
            {

                FileUrl = fileInfo.Url,
                Id = id,
                StoredInCdn = fileInfo.StoredInCloud
            });

            return RedirectToAction("AddAttachment", new { id });

        }
    }
}