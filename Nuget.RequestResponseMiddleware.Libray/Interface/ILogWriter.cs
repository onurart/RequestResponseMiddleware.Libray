namespace Nuget.RequestResponseMiddleware.Libray.Interface
{
    public interface ILogWriter
    {

        ILogMessageCreator MessageCreator { get; }

        Task Write(RequestResponseContext context);
    }
}
