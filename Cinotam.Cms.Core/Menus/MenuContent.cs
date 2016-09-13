﻿using Abp.Domain.Entities.Auditing;

namespace Cinotam.Cms.Core.Menus
{
    public class MenuContent : FullAuditedEntity
    {
        public string Text { get; set; }
        public string Lang { get; set; }
        public virtual Menu Menu { get; set; }
    }
}
