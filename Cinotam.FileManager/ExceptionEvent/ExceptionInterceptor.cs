using Abp.Events.Bus.Handlers;
using Cinotam.FileManager.ExceptionEvent.EventData;

namespace Cinotam.FileManager.ExceptionEvent
{
    public class ExceptionInterceptor : IEventHandler<FileManagerExEventData>
    {
        public void HandleEvent(FileManagerExEventData eventData)
        {

        }
    }
}
