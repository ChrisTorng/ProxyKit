using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace ProxyKit
{
    public delegate Task HandleWebSocketProxyRequest(HttpContext httpContext);
}