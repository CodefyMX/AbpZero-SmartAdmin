using System.Collections.Generic;

namespace Cinotam.AbpModuleZero.Tools.DatatablesJsModels.GenericTypes
{
    /// <summary>
    /// Normal request model
    /// </summary>
    public class RequestModel : RequestModel<int>
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
        public dynamic GenericProperty { get; set; }
    }

    /// <summary>
    /// Abstract class for the request model
    /// </summary>
    /// <typeparam name="TParameterType"></typeparam>
    public abstract class RequestModel<TParameterType>
    {
        public TParameterType RequestParameter { get; set; }
    }
}
