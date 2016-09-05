using System;

namespace Cinotam.MailSender.TemplateManager.Templates
{
    public abstract class Template
    {
        public string User { get; set; }
        public string Content { get; set; }
        public string TemplateRoute { get; set; }
        public DateTime Date { get; set; }
    }
}
