using System;

namespace Cinotam.ModuleZero.MailSender.TemplateManager.Templates
{
    public abstract class Template
    {
        public string User { get; set; }
        public string Content { get; set; }
        public string TemplateRoute { get; set; }
        public DateTime Date { get; set; }
    }
}
