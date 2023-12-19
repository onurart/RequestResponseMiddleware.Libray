

namespace Nuget.RequestResponseMiddleware.Libray.Middlewares;
public class RequestResponseLogginMiddleware
{
    private readonly RequestDelegate next;
    private readonly RequestResponseOptions reqResOptions;
    private readonly RecyclableMemoryStreamManager _recyclableMemoryStreamManager;
    public RequestResponseLogginMiddleware(RequestDelegate next, RequestResponseOptions reqResOptions, RecyclableMemoryStreamManager recyclableMemoryStreamManager = null)
    {
        this.next = next;
        this.reqResOptions = reqResOptions;
        _recyclableMemoryStreamManager = recyclableMemoryStreamManager;
    }
    public async Task Invoke(HttpContext context)
    {
        var requstbody = await GetRequstBody(context);
        var originalBodyStream = context.Request.Body;
        await using var responseBody = _recyclableMemoryStreamManager.GetStream();
        context.Response.Body = responseBody;
        var sw = Stopwatch.StartNew();
        await next(context);
        sw.Stop();
        context.Request.Body.Seek(0, SeekOrigin.Begin);
        string responsebodytext = await new StreamReader(context.Response.Body).ReadToEndAsync();
        context.Request.Body.Seek(0, SeekOrigin.Begin);
        var reqResContext = new RequestResponseContext(context)
        {
            ResponseCreationTime = TimeSpan.FromTicks(sw.ElapsedTicks),
            RequstBody = requstbody,
            ResponseBody = responsebodytext
        };
        reqResOptions.ReqResHandler?.Invoke(reqResContext);
    }
    private static string ReadStreamInChunks(Stream stream)
    {
        const int readChunkbufferLength = 4096;
        stream.Seek(0, SeekOrigin.Begin);
        using var textWriter = new StringWriter();
        using var reader = new StreamReader(stream, Encoding.UTF8);
        var readChunk = new char[readChunkbufferLength];
        int readChunkLength;
        do
        {
            readChunkLength = reader.Read(readChunk,
                0,
                readChunkbufferLength);
            textWriter.Write(readChunk, 0, readChunkLength);

        } while (readChunkLength > 0);
        return textWriter.ToString();
    }
    public async Task<string> GetRequstBody(HttpContext context)
    {
        context.Request.EnableBuffering();
        await using var requstStream = _recyclableMemoryStreamManager.GetStream();
        await context.Request.Body.CopyToAsync(requstStream);

        string reqBody = ReadStreamInChunks(requstStream);
        context.Request.Body.Seek(0, SeekOrigin.Begin);
        return reqBody;
    }
}

