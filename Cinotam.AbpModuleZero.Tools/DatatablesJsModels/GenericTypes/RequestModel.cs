using System.Collections.Generic;

namespace Cinotam.AbpModuleZero.Tools.DatatablesJsModels.GenericTypes
{
    /// <summary>
    /// Normal request model, we use the generic to send special parameters to the query,
    /// this will not have negative effects in the reflection extension (i hope so...)
    /// </summary>
    public class RequestModel<T> where T : new()
    {
        public RequestModel()
        {
            SecondarySearch = new List<string>();

        }
        public int start { get; set; }
        public int length { get; set; }
        public int draw { get; set; }
        public Dictionary<string, string> search { get; set; }
        public int PropSort { get; set; }
        public string PropOrd { get; set; }
        public List<string> SecondarySearch { get; set; }
        /// <summary>
        /// For reflection
        /// </summary>
        public string PropToSort { get; set; }

        public string PropToSearch { get; set; }
        public bool SearchInAll { get; set; }
        public string SearchCol { get; set; }
        public T TypeOfRequest { get; set; }
    }

}
