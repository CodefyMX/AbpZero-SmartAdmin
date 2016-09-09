using Cinotam.AbpModuleZero.Tools.DatatablesJsModels.GenericTypes;
using Cinotam.Cms.App.Pages.Dto;
using Cinotam.Cms.Core.Pages;
using System;
using System.Threading.Tasks;

namespace Cinotam.Cms.App.Pages
{
    public class PagesService : IPagesService
    {
        private readonly IPageManager _pageManager;

        public PagesService(IPageManager pageManager)
        {
            _pageManager = pageManager;
        }

        public Task CreateEditPage(string name, int parent, int templateId)
        {
            throw new NotImplementedException();
        }

        public Task<PageDto> GetPage(int id, string lang)
        {
            throw new NotImplementedException();
        }

        public Task<PageDto> GetPreviewPage(int id, string name)
        {
            throw new NotImplementedException();
        }

        public Task<ReturnModel<PageDto>> GetPageList(RequestModel<object> input)
        {
            throw new NotImplementedException();
        }
    }
}
