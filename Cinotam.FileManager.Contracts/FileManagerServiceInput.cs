using System;
using System.Collections.Generic;

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
    }
}
