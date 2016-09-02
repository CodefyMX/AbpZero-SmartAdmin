namespace Cinotam.ModuleZero.MailSender.TemplateManager.Templates
{
    public class SimpleTemplate : Template
    {
        public SimpleTemplate(string user, string content, string route = "/Content/MailTemplates/Simple.html")
        {
            TemplateRoute = route;
            User = user;
            Content = content;
        }
    }
}
