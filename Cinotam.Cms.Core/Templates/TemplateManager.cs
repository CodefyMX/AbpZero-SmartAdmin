﻿using Cinotam.Cms.Core.Templates.Outputs;
using Cinotam.Cms.DatabaseEntities.Templates.Entities;
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
                        Resources = templateContent.Resources
                    };
                }
                catch (Exception)
                {
                    //
                }
            }
            throw new InvalidOperationException(nameof(templateName));
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
                    var template = new Template()
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
    }
}
