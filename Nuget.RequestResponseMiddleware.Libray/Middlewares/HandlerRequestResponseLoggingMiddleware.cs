namespace Nuget.RequestResponseMiddleware.Libray.Middlewares
{
    public class HandlerRequestResponseLoggingMiddleware : BaseRequestResponseMiddleware
    {
        private readonly Func<RequestResponseContext, Task> reqResHandler;

        public HandlerRequestResponseLoggingMiddleware(RequestDelegate next,
                                                       Func<RequestResponseContext, Task> reqResHandler,
                                                       ILogWriter logWriter)
            : base(next, logWriter)
        {
            this.reqResHandler = reqResHandler;
        }

        public async Task Invoke(HttpContext context)
        {
            var reqResContext = await BaseMiddlewareInvoke(context);

            await reqResHandler.Invoke(reqResContext);

        }
    }
}
