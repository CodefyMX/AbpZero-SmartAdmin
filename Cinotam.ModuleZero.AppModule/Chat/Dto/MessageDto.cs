using System;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Cinotam.AbpModuleZero.Chat.Entities;

namespace Cinotam.ModuleZero.AppModule.Chat.Dto
{
    [AutoMap(typeof(Message))]
    public class MessageDto : EntityDto
    {
        public long SenderId { get; set; }
        public string MessageText { get; set; }
        public DateTime CreationTime { get; set; }
        public int ConversationId { get; set; }

    }
}