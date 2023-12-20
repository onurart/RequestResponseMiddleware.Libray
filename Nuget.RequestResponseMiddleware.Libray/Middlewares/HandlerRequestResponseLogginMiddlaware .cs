using System.Reflection.Metadata;

namespace Nuget.RequestResponseMiddleware.Libray.Middlewares
{
    public class HandlerRequestResponseLogginMiddlaware : BaseRequestResponseMiddleware
    {
        private readonly Func<RequestResponseContext, Task> reqResHandler;

        public HandlerRequestResponseLogginMiddlaware(RequestDelegate next,
                                                       Func<RequestResponseContext, Task> reqResHandler
                                                       )
            : base(next)
        {
            this.reqResHandler = reqResHandler;
        }

        public async Task Invoke(HttpContext context)
        {
            var reqResContext = await BaseMiddewareInvoke(context);
            await reqResHandler.Invoke(reqResContext);

        }
    }
}

