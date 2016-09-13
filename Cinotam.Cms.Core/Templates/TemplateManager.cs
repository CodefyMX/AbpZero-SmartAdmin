using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cinotam.Cms.Core.Templates
{
    public class TemplateManager : ITemplateManager
    {
        public async Task<string> GetTemplateContentAsync(string templateName)
        {
            foreach (var provider in CinotamCmsCore.TemplateContentProviders)
            {
                var templateContent = await provider.GetTemplateContent(templateName);
                return templateContent;
            }
            throw new InvalidOperationException(nameof(templateName));
        }

        public async Task<List<string>> GetAvailableTemplatesAsync()
        {
            foreach (var provider in CinotamCmsCore.TemplateContentProviders)
            {
                var templateContent = await provider.GetAvailableTemplates();
                return templateContent;
            }
            throw new InvalidOperationException();
        }
    }
}
