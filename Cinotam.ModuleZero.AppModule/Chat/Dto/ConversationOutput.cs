using Abp.Application.Services.Dto;
using System.Collections.Generic;

namespace Cinotam.ModuleZero.AppModule.Chat.Dto
{
    public class ConversationOutput : EntityDto
    {
        /// <summary>
        /// The "to" user
        /// </summary>
        public string User { get; set; }
        public List<MessageDto> Messages { get; set; } = new List<MessageDto>();
    }
}
