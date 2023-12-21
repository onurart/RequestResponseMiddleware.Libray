namespace Nuget.RequestResponseMiddleware.Libray.MessageCreators
{
    internal class LoggerFactoryMessageCreator : BaseLogMessageCreator, ILogMessageCreator
    {
        private readonly LoggingOptions loggingOptions;

        public LoggerFactoryMessageCreator(LoggingOptions loggingOptions)
        {
            this.loggingOptions = loggingOptions;
        }

        public string Create(RequestResponseContext context)
        {
            var sb = new StringBuilder();

            foreach (var field in loggingOptions.LoggingFields)
            {
                var value = GetValuebyField(context, field);
                // Path: /api/user/login

                sb.AppendFormat("{0}: {1}\n", field, value);
            }

            return sb.ToString();
        }


    }
}
