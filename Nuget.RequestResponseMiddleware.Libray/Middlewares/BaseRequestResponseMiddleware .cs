using Microsoft.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nuget.RequestResponseMiddleware.Libray.Middlewares
{
    public abstract class BaseRequestResponseMiddleware
    {
        private readonly RequestDelegate next;
        private readonly RequestResponseOptions reqResOptions;
        private readonly RecyclableMemoryStreamManager _recyclableMemoryStreamManager;



        public BaseRequestResponseMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        protected async Task<RequestResponseContext> BaseMiddewareInvoke(HttpContext context)
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
            return new RequestResponseContext(context)
            {
                ResponseCreationTime = TimeSpan.FromTicks(sw.ElapsedTicks),
                RequstBody = requstbody,
                ResponseBody = responsebodytext
            };

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
}
