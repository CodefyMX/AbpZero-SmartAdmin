using Cinotam.ModuleZero.MailSender.TemplateManager.Templates;
using System;
using System.Web.Hosting;

namespace Cinotam.ModuleZero.MailSender.TemplateManager
{
    public class TemplateManager : ITemplateManager
    {

        public string GetContent(TemplateType type, bool enablePartials, params string[] arguments)
        {
            switch (type)
            {
                case TemplateType.Welcome:
                    return GetTextFromFile(new WelcomeTemplate(enablePartials, arguments));
                case TemplateType.NotificationUserChangePassword:
                    return GetTextFromFile(new ChangePasswordTemplate());
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
        private string GetTextFromFile(Template template)
        {
            var localPath = HostingEnvironment.MapPath(template.TemplateRoute);
            if (localPath != null)
            {
                var text = System.IO.File.ReadAllText(localPath);
                var format = string.Format(text, template.Arguments);
                return format;
            }
            throw new InvalidOperationException(nameof(localPath));
        }
    }
}
