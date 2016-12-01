using Abp.Application.Services.Dto;
using Cinotam.AbpModuleZero.LocalizableContent.Contracts;

namespace Cinotam.SimplePost.Application.Posts.Dto
{
    public class NewPostInput : EntityDto
    {
        public bool Active { get; set; } = true;
        public Content Content { get; set; }
    }

    public class Content : EntityDto
    {
        public string Title { get; set; }
        public string ContentString { get; set; }
        public string Lang { get; set; }

        [IsSharedProperty]
        public string SharedString { get; set; } = "Shared string";
    }



    //public class LocalizableContentDto<T> where T : class
    //{
    //    public T Content { get; set; }
    //    public Dictionary<string, string> SharedProps { get; set; }
    //}
}
