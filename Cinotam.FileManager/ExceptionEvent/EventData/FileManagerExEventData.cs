using Abp.Events.Bus.Exceptions;
using System;

namespace Cinotam.FileManager.ExceptionEvent.EventData
{
    public class FileManagerExEventData : AbpHandledExceptionData
    {
        /// <summary>
        /// If any exception occurs the manager must delete the temporal file
        /// </summary>
        public string File { get; set; }

        public FileManagerExEventData(Exception exception) : base(exception)
        {
        }
    }
}
