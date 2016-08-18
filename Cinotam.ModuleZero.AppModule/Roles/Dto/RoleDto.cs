using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Cinotam.AbpModuleZero.Authorization.Roles;
using System;

namespace Cinotam.ModuleZero.AppModule.Roles.Dto
{
    [AutoMapFrom(typeof(Role))]
    public class RoleDto : EntityDto
    {
        public bool IsSelected { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public bool IsStatic { get; set; }
        public DateTime CreationTime { get; set; }

        public string CreationTimeString => CreationTime.ToShortDateString();
    }
}