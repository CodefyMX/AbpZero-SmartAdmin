using System;
using System.Collections.Generic;
using System.Web;

namespace Cinotam.FileManager.Contracts
{
    public class FileManagerServiceInput : IFileManagerServiceInput
    {
        public FileManagerServiceInput()
        {
            Properties = new Dictionary<string, object>();
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

        public HttpPostedFileBase File { get; set; }
        public string SpecialFolder { get; set; }
        public string VirtualFolder { get; set; }
        public string FilePath { get; set; }
        public string OverrideFormat { get; set; }
        public bool CreateUniqueName { get; set; }
    }
}
