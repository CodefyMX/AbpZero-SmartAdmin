using System;

namespace Cinotam.ModuleZero.MailSender.TemplateManager.Templates
{
    public abstract class Template
    {
        protected Template(string templateName, string route)
        {
            _templateName = templateName;
            _route = route;
        }

        private string _templateName;
        private readonly string _route;
        public object[] Arguments { get; set; }
        public string TemplateRoute => BuildRoute();
        public DateTime Date { get; set; }
        public bool UsePartial { get; set; }

        private string BuildRoute()
        {
            if (UsePartial)
            {
                _templateName = _templateName + "_partial";
            }

            var route = string.Format(_route, _templateName);
            return route;
        }

    }
}
