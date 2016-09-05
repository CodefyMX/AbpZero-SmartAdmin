namespace Cinotam.ModuleZero.MailSender.TemplateManager.Templates
{
    public class WelcomeTemplate : Template
    {
        public WelcomeTemplate(string user, string content, bool usePartial) : base("Welcome", "/Content/MailTemplates/WelcomeTemplates/{0}.html")
        {
            User = user;
            Content = content;
            UsePartial = usePartial;
        }
    }

}
