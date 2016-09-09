namespace Cinotam.ModuleZero.MailSender.TemplateManager.Templates
{
    public class WelcomeTemplate : Template
    {
        public WelcomeTemplate(bool usePartial, params string[] arguments) : base("Welcome", "/Content/MailTemplates/WelcomeTemplates/{0}.html")
        {
            Arguments = arguments;
            UsePartial = usePartial;
        }
    }

}
