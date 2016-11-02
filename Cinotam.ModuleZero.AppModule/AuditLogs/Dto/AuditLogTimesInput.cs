namespace Cinotam.ModuleZero.AppModule.AuditLogs.Dto
{
    public class AuditLogTimesInput
    {

        public int? Count { get; set; } = null;
        public int Code { get; set; } = 0;
        public int? TenantId { get; set; } = null;
    }
}
