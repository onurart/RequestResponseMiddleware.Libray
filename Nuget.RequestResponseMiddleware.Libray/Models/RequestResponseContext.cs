using System.Text.Json.Serialization;

namespace Nuget.RequestResponseMiddleware.Libray.Models
{
    public class RequestResponseContext
    {
        private readonly HttpContext context;
        public RequestResponseContext(HttpContext httpContext)
        {
            this.context = httpContext;
        }
        public string RequstBody { get; set; }
        public string ResponseBody { get; set; }
        [JsonIgnore]
        public TimeSpan ResponseCreationTime { get; set; }
        public string FormattedCreationTime =>
            FormattedCreationTime is null
                ? "00:00.000" :
                string.Format("{0:mm\\:ss\\.fff", ResponseCreationTime);
        public Uri Url => BuildUrl();
        public int? RequestLength => RequstBody.Length;
        public int? ResponseLength => ResponseBody.Length;
        internal Uri BuildUrl()
        {
            var url = context.Request.GetDisplayUrl();
            return new Uri(url, UriKind.RelativeOrAbsolute);
        }
    }
}
