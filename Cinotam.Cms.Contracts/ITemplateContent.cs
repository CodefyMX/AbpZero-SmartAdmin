﻿using Abp.Domain.Entities;
using System.Collections.Generic;

namespace Cinotam.Cms.Contracts
{
    public interface ITemplateContent : IMayHaveTenant
    {
        string Name { get; set; }
        string FileName { get; set; }
        string Content { get; set; }
        bool IsPartial { get; set; }
        ICollection<IResource> Resources { get; }
    }
}
