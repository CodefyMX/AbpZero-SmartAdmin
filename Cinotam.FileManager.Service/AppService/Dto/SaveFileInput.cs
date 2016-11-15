using System;
using System.Collections.Generic;
using System.Web;

namespace Cinotam.FileManager.Service.AppService.Dto
{
    public class SaveFileInput
    {
        public SaveFileInput(HttpPostedFileBase file)
        {
            Properties = new Dictionary<string, object>();
            if (file.ContentLength <= 0) throw new InvalidOperationException(nameof(file.ContentLength));
            File = file;
        }

        public SaveFileInput(string base64String)
        {
            Base64String = base64String;
        }
        public Dictionary<string, object> Properties
        {
            get { return _properties; }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value));
                }

                _properties = value;
            }

        }

        private Dictionary<string, object> _properties;
        public object this[string key]
        {
            get { return Properties[key]; }
            set { Properties[key] = value; }
        }
        public string Base64String { get; private set; }

        public HttpPostedFileBase File { get; private set; }
        public string SaveFolder { get; set; } = string.Empty;
    }
}
