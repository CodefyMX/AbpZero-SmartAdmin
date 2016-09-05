namespace Cinotam.MailSender.TemplateManager.Templates
{
    public class ChangePasswordTemplate : Template
    {
        public ChangePasswordTemplate(string user, string content, string templateRoute = "/Content/MailTemplates/PasswordChange.html")
        {
            User = user;
            Content = content;
            TemplateRoute = templateRoute;
        }
    }
}
