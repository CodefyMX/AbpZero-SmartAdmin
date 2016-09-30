using System.Web;

namespace Cinotam.Cms.App.Pages.Dto
{
    public class AddImageInput
    {
        public HttpPostedFileBase Image { get; set; }
        public string Lang { get; set; }
        public int PageId { get; set; }
    }
}
