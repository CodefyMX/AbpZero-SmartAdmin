using Abp.Events.Bus;

namespace Cinotam.Cms.App.Events
{
    public class PageCategoryChangedData : EventData
    {
        public int PageId { get; set; }
        public int? OldCategoryId { get; set; }
        public int CategoryId { get; set; }
    }
}
