namespace Nuget.RequestResponseMiddleware.Libray.Models
{
    public class LoggingOptions
    {
        private List<LogFields> loggingFields;
        public LogLevel logLevel { get; set; } = LogLevel.Information;
        public string LoggerCategoryName { get; set; } = "RequstReponseLoggerMiddleware";
        public List<LogFields> LogFields
        {
            get { return loggingFields ??= new List<LogFields>(); }
            set => loggingFields = value;
        }
    }
    public enum LogFields
    {
        Requst,
        Reponse,
        HostName,
        Path,
        QueryString,
        ResponseTiming,
        RequestLength,
        ResponseLength
    }
}
