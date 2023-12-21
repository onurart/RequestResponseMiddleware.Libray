namespace Nuget.RequestResponseMiddleware.Libray
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder AddTBRequestResponseMiddleware(this IApplicationBuilder appBuilder,
            Action<RequestResponseOptions> optionAction)
        {
            var opt = new RequestResponseOptions();
            optionAction(opt);
            ILogWriter logWriter = opt.LoggerFactory is null
                ? new NullLogWriter()
                : new LoggerFactoryLogWriter(opt.LoggerFactory, opt.LoggingOptions);
            if (opt.ReqResHandler is not null)
                appBuilder.UseMiddleware<HandlerRequestResponseLoggingMiddleware>(opt.ReqResHandler, logWriter);
            else
                appBuilder.UseMiddleware<RequestResponseLoggingMiddleware>(logWriter);
            return appBuilder;
        }
    }
}
