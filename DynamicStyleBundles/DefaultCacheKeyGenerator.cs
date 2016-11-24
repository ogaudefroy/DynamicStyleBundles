namespace DynamicStyleBundles
{
    using System;
    using System.Web;

    /// <summary>
    /// Generates a unique cache key based on the following format : {HOST}_{originalKey}.
    /// HOST value is extracted based on HTTP_HOST ASP.Net Server Variable.
    /// </summary>
    public class DefaultCacheKeyGenerator : ICacheKeyGenerator
    {
        /// <inheritdoc />
        public string GetCacheKey(string originalKey)
        {
            if (HttpContext.Current == null)
            {
                throw new NotSupportedException();
            }
            return this.GetCacheKey(new HttpContextWrapper(HttpContext.Current), originalKey);
        }

        /// <summary>
        /// Computes the cache key.
        /// </summary>
        /// <param name="context">Incoming request.</param>
        /// <param name="originalKey">The original key.</param>
        /// <returns>The generated cache key.</returns>
        internal string GetCacheKey(HttpContextBase context, string originalKey)
        {
            return string.Format("{0}:{1}", ExtractKeyFromRequest(context), originalKey);   
        }

        private string ExtractKeyFromRequest(HttpContextBase context)
        {
            if (context.Request.ServerVariables["HTTP_HOST"] != null)
            {
                return context.Request.ServerVariables["HTTP_HOST"];
            }
            return context.Request.Url.DnsSafeHost;
        }
    }
}
