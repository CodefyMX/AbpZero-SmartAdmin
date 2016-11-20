namespace Cinotam.AbpModuleZero.LocalizableContent.Contracts
{
    public interface ILocalizableContent<T, TContentType> where T : class where TContentType : class
    {
        string SerializeContent(TContentType contentType);
        string EntityId { get; set; }
        string EntityName { get; set; }
        string Lang { get; set; }
        string EntityDtoName { get; set; }
        string Properties { get; set; }
    }
}
