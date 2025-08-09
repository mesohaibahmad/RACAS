using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace RACAS.Middleware
{
    public class SecurityHeadersMiddleware
    {
        private readonly RequestDelegate _next;

        public SecurityHeadersMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {

            await _next(context);
        }
    }
}
