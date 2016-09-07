namespace Cinotam.ModuleZero.AppModule.AuditLogs.Dto
{
    public class LogDto
    {
        public string LogLevel { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        public string ThreadNumber { get; set; }
        public string LoggerName { get; set; }
        public string LogText { get; set; }
        public string LoggerContent { get; set; }
    }
}
