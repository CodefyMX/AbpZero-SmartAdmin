using System;
using System.Collections.Generic;

namespace Cinotam.TwoFactorAuth.Contracts
{
    public class SendMessageResult
    {
        public SendMessageResult()
        {
            Properties = new Dictionary<string, object>();
        }
        public SendStatus SendStatus { get; set; }
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

    public enum SendStatus
    {
        Fail,
        Success,
        Queued,
        NotNecessary
    }
}
