using Abp.Events.Bus;
using Cinotam.Cms.DatabaseEntities.Pages.Entities;

namespace Cinotam.Cms.App.Events
{
    public class PageStateChangedData : EventData
    {
        public Page Page { get; set; }
    }
}
