

namespace Nuget.RequestResponseMiddleware.Libray.Middlewares;
public class RequestResponseLogginMiddleware : BaseRequestResponseMiddleware
{
    public RequestResponseLogginMiddleware(RequestDelegate next) 
    : base(next){}

    public async Task Invoke(HttpContext context)
    {
      await BaseMiddewareInvoke(context);
    }

}

