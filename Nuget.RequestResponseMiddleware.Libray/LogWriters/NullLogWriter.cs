﻿namespace Nuget.RequestResponseMiddleware.Libray.LogWriters
{
    internal class NullLogWriter : ILogWriter
    {
        public ILogMessageCreator MessageCreator { get; }

        public Task Write(RequestResponseContext context)
        {
            return Task.CompletedTask;
        }
    }
}
