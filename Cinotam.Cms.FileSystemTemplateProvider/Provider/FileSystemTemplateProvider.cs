using Cinotam.Cms.Contracts;
using Cinotam.Cms.FileSystemTemplateProvider.Provider.JsonEntities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Web.Hosting;

namespace Cinotam.Cms.FileSystemTemplateProvider.Provider
{
    public class FileSystemTemplateProvider : IFileSystemTemplateProvider
    {
        private const string TemplateFolder = "/Content/Templates/";
        private const string JsonConfigFileName = "TemplateConfig.json";

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

        private string GetContentFromConfiguration(JsonConfigurarion configuration, string directory)
        {
            var file = directory+"/"  + configuration.TemplateFullName;
            return GetTextFromFile(file);
        }

        public Task CreateEditTemplate(ITemplateContent templateContent)
        {
            throw new NotImplementedException();
        }

        private IEnumerable<string> GetDirectories()
        {
            var folder = HostingEnvironment.MapPath(TemplateFolder);
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
