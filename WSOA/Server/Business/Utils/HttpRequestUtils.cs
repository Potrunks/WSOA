namespace WSOA.Server.Business.Utils
{
    public static class HttpRequestUtils
    {
        public static string GeBasetUrl(this HttpRequest request)
        {
            return $"{request.Scheme}://{request.Host.Value}/";
        }
    }
}
