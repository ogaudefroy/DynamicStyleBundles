namespace DynamicStyleBundles
{
    using System.Web;

    /// <summary>
    /// Custom HttpHandlerFactory used to instantiate HttpHander.
    /// </summary>
    public class HttpHandlerFactory : IHttpHandlerFactory
    {
        /// <summary>
        /// Creates the HTTP handler.
        /// </summary>
        /// <param name="context">The HTTP context.</param>
        /// <param name="requestType">The request type.</param>
        /// <param name="url">The target URL.</param>
        /// <param name="pathTranslated">The path to translate.</param>
        /// <returns>The HTTP Handler or null.</returns>
        public IHttpHandler GetHandler(HttpContext context, string requestType, string url, string pathTranslated)
        {
            if (requestType == "GET")
            {
                return new HttpHandler();
            }
            return null;
        }

        /// <summary>
        /// Releases the handler.
        /// </summary>
        /// <param name="handler">The instantiated handler.</param>
        public void ReleaseHandler(IHttpHandler handler)
        {
        }
    }
}
