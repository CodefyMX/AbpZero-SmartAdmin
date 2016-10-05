using System.Collections.Generic;
using System.Web;

namespace Cinotam.FileManager.Contracts
{
    public interface IFileManagerServiceInput
    {
        Dictionary<string, object> Properties { get; }
        object this[string key] { get; }
        HttpPostedFileBase File { get; set; }
        string SpecialFolder { get; set; }
        string VirtualFolder { get; set; }

        string FilePath { get; set; }

        string OverrideFormat { get; set; }
        /// <summary>
        /// Has no effect if the file is going to be stored in the cloud
        /// </summary>
        bool CreateUniqueName { get; set; }
    }
}
