namespace Nuget.RequestResponseMiddleware.Libray.Interface
{
    public interface ILogMessageCreator
    {
        string Create(RequestResponseContext context);

    }
}
