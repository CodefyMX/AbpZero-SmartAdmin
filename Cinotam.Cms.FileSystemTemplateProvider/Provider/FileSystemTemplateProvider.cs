using Cinotam.Cms.Contracts;
using Cinotam.Cms.FileSystemTemplateProvider.Provider.JsonEntities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Cinotam.Cms.FileSystemTemplateProvider.Provider
{
    public class FileSystemTemplateProvider : IFileSystemTemplateProvider
    {
        private const string TemplateFolder = "/Content/Templates/";
        private const string JsonConfigFileName = "TemplateConfig.json";

        public bool IsDatabase => false;

        public async Task<string> GetTemplateContent(string templateName)
        {
            var directories = GetDirectories();
            await Task.FromResult(0);
            foreach (var directory in directories)
            {
                var configuration = GetConfigFile(directory);
                if (configuration == null) continue;
                if (configuration.TemplateName != templateName) continue;
                var content = GetContentFromConfiguration(configuration, directory);
                return content;
            }
            throw new InvalidOperationException(nameof(templateName));
        }

        public async Task<List<string>> GetAvailableTemplates()
        {
            var directories = GetDirectories();
            var templates = new List<string>();
            await Task.FromResult(0);
            foreach (var directory in directories)
            {
                var configuration = GetConfigFile(directory);
                if (configuration == null) continue;
                templates.Add(configuration.TemplateName);
            }
            return templates;
        }

        private string GetContentFromConfiguration(JsonConfigurarion configuration, string directory)
        {
            var file = directory + "/" + configuration.TemplateFullName;
            return GetTextFromFile(file);
        }

        public Task CreateEditTemplate(CTemplate templateContent)
        {
            throw new NotImplementedException("File system template creation is not available");
        }

        public async Task<CTemplate> GetTemplateInfo(string templateName)
        {
            var directories = GetDirectories();
            await Task.FromResult(0);
            foreach (var directory in directories)
            {
                var configuration = GetConfigFile(directory);
                if (configuration == null) continue;
                if (configuration.TemplateName != templateName) continue;
                var content = GetContentFromConfiguration(configuration, directory);
                return new CTemplate()
                {
                    Content = content,
                    FileName = configuration.TemplateFullName,
                    Name = configuration.TemplateName,
                    IsPartial = configuration.IsPartial,
                    ResourcesObj = GetResources(configuration)
                };
            }
            throw new InvalidOperationException(nameof(templateName));
        }

        public async Task<List<CTemplate>> GetTemplatesInfo()
        {
            var directories = GetDirectories();
            await Task.FromResult(0);
            var listOfTemplates = new List<CTemplate>();
            foreach (var directory in directories)
            {
                var configuration = GetConfigFile(directory);
                if (configuration == null) continue;
                var content = GetContentFromConfiguration(configuration, directory);
                listOfTemplates.Add(new CTemplate()
                {
                    Content = content,
                    FileName = configuration.TemplateFullName,
                    Name = configuration.TemplateName,
                    IsPartial = configuration.IsPartial,
                    ResourcesObj = GetResources(configuration)
                });
            }
            return listOfTemplates;
        }

        public Task AddJsResource(string resourceRoute, string templateName, string description)
        {
            throw new NotImplementedException();
        }

        public Task AddCssResource(string resourceRoute, string templateName, string description)
        {
            throw new NotImplementedException();
        }

        public string ServiceName => "Cinotam.FileSystem.Template.Provider";

        private ICollection<CResource> GetResources(JsonConfigurarion configuration)
        {
            var list = new List<CResource>();
            if (configuration.ExternalResources.Css != null && configuration.ExternalResources.Css.Any())
            {
                foreach (var css in configuration.ExternalResources.Css.Where(a => a != null).OrderBy(a => a.Order))
                {
                    list.Add(new CResource()
                    {
                        ResourceType = "css",
                        ResourceUrl = css.Url,
                        IsCdn = css.IsCdn
                    });
                }
            }
            if (configuration.ExternalResources.Js != null && configuration.ExternalResources.Js.Any())
            {
                foreach (var externalResourcesJ in configuration.ExternalResources.Js.Where(a => a != null).OrderBy(a => a.Order))
                {
                    list.Add(new CResource()
                    {
                        ResourceType = "js",
                        ResourceUrl = externalResourcesJ.Url,
                        IsCdn = externalResourcesJ.IsCdn
                    });
                }
            }

            return list;
        }

        private IEnumerable<string> GetDirectories()
        {
            var folder = HttpContext.Current.Server.MapPath(TemplateFolder);
            if (folder != null)
            {
                var directories = Directory.GetDirectories(folder);
                return directories;
            }
            throw new InvalidOperationException(nameof(folder));
        }

        private JsonConfigurarion GetConfigFile(string folder)
        {
            try
            {

                var fullRoute = folder + "/" + JsonConfigFileName;
                var txtFromFile = GetTextFromFile(fullRoute);
                var jsonObject = JsonConvert.DeserializeObject<JsonConfigurarion>(txtFromFile);
                return jsonObject;
            }
            catch (Exception)
            {
                // ignored
                return null;
            }
        }

        private string GetTextFromFile(string fullRoute)
        {
            using (var fileStream = new StreamReader(fullRoute))
            {
                var txt = fileStream.ReadToEnd();
                fileStream.Close();
                return txt;
            }
        }
    }
}
