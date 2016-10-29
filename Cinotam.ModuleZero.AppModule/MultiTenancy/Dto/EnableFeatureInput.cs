namespace Cinotam.ModuleZero.AppModule.MultiTenancy.Dto
{
    public class EnableFeatureInput
    {
        public string FeatureName { get; set; }
        public string FeatureStatus { get; set; }
        public int Id { get; set; }
        public int FeatureId { get; set; }
    }
}
