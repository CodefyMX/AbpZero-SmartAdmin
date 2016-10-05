using Abp.Events.Bus;

namespace Cinotam.Cms.App.Events
{
    public class PageDeletedData : EventData
    {
        public int PageId { get; set; }
    }
}
