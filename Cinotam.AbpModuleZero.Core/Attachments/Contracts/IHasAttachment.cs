namespace Cinotam.AbpModuleZero.Attachments.Contracts
{
    public interface IHasAttachment<TEntity> where TEntity : class
    {
        string Description { get; set; }
        string ContentUrl { get; set; }
        bool Active { get; set; }
        bool StoredInCdn { get; set; }
        TEntity Entity { get; set; }
        string Properties { get; set; }
        string FileName { get; set; }
        string SerializeContent<TProperties>(TProperties contentType);

    }
}
