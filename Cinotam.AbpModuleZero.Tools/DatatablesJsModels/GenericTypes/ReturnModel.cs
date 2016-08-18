namespace Cinotam.AbpModuleZero.Tools.DatatablesJsModels.GenericTypes
{
    public class ReturnModel<T>
    {
        public int draw { get; set; }
        public T[] data { get; set; }
        public int recordsTotal { get; set; }
        public int recordsFiltered { get; set; }
        public string error { get; set; }
        public int length { get; set; }
        public int iTotalDisplayRecords { get; set; }
        public int iTotalRecords { get; set; }
    }
}
