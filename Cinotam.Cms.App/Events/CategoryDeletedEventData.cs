using Abp.Events.Bus;

namespace Cinotam.Cms.App.Events
{
    public class CategoryDeletedEventData : EventData
    {
        public int CategoryId { get; set; }
    }
}
