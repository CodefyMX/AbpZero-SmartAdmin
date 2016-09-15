using Cinotam.Cms.Contracts;
using Cinotam.Cms.Core.Templates.Outputs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cinotam.Cms.Core.Templates
{
    public class TemplateManager : ITemplateManager
    {
        public async Task<TemplateInfo> GetTemplateContentAsync(string templateName)
        {
            foreach (var provider in CinotamCmsCore.TemplateContentProviders)
            {
                try
                {

                    var templateContent = await provider.GetTemplateInfo(templateName);
                    return new TemplateInfo()
                    {
                        Content = templateContent.Content,
                        FileName = templateContent.FileName,
                        IsPartial = templateContent.IsPartial,
                        Name = templateContent.Name,
                        Resources = templateContent.Resources,
                        IsDatabaseTemplate = provider.IsDatabase
                    };
                }
                catch (Exception)
                {
                    //
                }
            }
            return null;
        }

        public async Task<List<string>> GetAvailableTemplatesAsync()
        {
            var listString = new List<string>();
            foreach (var provider in CinotamCmsCore.TemplateContentProviders)
            {
                try
                {
                    var templateContent = await provider.GetAvailableTemplates();
                    foreach (var template in templateContent)
                    {
                        if (listString.Any(a => a == template))
                        {
                            continue;
                        }
                        listString.AddRange(templateContent);
                    }
                }
                catch (Exception ex)
                {
                    if (!(ex is NotImplementedException))
                    {
                        throw;
                    }
                }
            }
            return listString;
        }

        public async Task CreateTemplate(string templateName, string copyFrom)
        {
            var templateInfo = new TemplateInfo();
            if (!string.IsNullOrEmpty(copyFrom))
            {
                templateInfo = await GetTemplateContentAsync(copyFrom);
            }
            foreach (var provider in CinotamCmsCore.TemplateContentProviders)
            {
                try
                {
                    var template = new CTemplate()
                    {
                        Name = templateName,
                        Content = templateInfo.Content,
                        FileName = ""
                    };
                    await provider.CreateEditTemplate(template);
                    return;
                }
                catch (Exception ex)
                {
                    if (!(ex is NotImplementedException)) throw;
                }

            }
            throw new InvalidOperationException(nameof(templateName));
        }

        public async Task AddCssResource(string url, string name, string description)
        {
            foreach (var provider in CinotamCmsCore.TemplateContentProviders)
            {
                try
                {

                    await provider.AddCssResource(url, name, description);
                    return;
                }
                catch (Exception ex)
                {
                    if (!(ex is NotImplementedException))
                    {
                        throw;
                    }
                }
            }
            throw new InvalidOperationException();
        }

        public async Task AddJsResource(string url, string name, string description)
        {
            foreach (var provider in CinotamCmsCore.TemplateContentProviders)
            {
                try
                {
                    await provider.AddJsResource(url, name, description);
                    return;
                }
                catch (Exception ex)
                {
                    if (!(ex is NotImplementedException))
                    {
                        throw;
                    }
                }
            }
            throw new InvalidOperationException();
        }

        public async Task<List<TemplateInfo>> GetTemplateContentsAsync()
        {
            var listOfTemplatesInfo = new List<TemplateInfo>();
            foreach (var templateContentProvider in CinotamCmsCore.TemplateContentProviders)
            {
                var templates = await templateContentProvider.GetTemplatesInfo();
                foreach (var cTemplate in templates)
                {
                    listOfTemplatesInfo.Add(new TemplateInfo()
                    {
                        Content = cTemplate.Content,
                        FileName = cTemplate.FileName,
                        IsDatabaseTemplate = templateContentProvider.IsDatabase,
                        IsPartial = cTemplate.IsPartial,
                        Name = cTemplate.Name
                    });
                }
            }
            return listOfTemplatesInfo;
        }

        public async Task<TemplateCreationResult> EditTemplate(TemplateInfo info)
        {
            return (await SaveTemplate(info));
        }
        public async Task<TemplateCreationResult> AddTemplate(TemplateInfo info)
        {

            if ((await Exists(info.Name)))
            {
                return new TemplateCreationResult()
                {
                    ErrorMessage = "TemplateNameAlreadyExsist",
                    HasError = true
                };
            }
            return (await SaveTemplate(info));

        }

        private async Task<TemplateCreationResult> SaveTemplate(ITemplateContent info)
        {
            foreach (var templateContentProvider in CinotamCmsCore.TemplateContentProviders)
            {
                try
                {
                    await templateContentProvider.CreateEditTemplate(new CTemplate()
                    {
                        Content = info.Content,
                        Name = info.Name,
                        IsPartial = info.IsPartial,

                    });
                }
                catch (Exception ex)
                {
                    if (ex is NotImplementedException) continue;
                    throw;
                }
            }
            return new TemplateCreationResult()
            {
                HasError = false
            };
        }

        private async Task<bool> Exists(string name)
        {
            var templates = new List<string>();

            foreach (var templateContentProvider in CinotamCmsCore.TemplateContentProviders)
            {
                templates.AddRange((await templateContentProvider.GetAvailableTemplates()));
            }
            return templates.Any(a => a == name);
        }
    }
}
