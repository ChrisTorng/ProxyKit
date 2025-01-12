using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace ProxyKit
{
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        ///     Runs proxy forwarding requests to downstream server.
        /// </summary>
        /// <param name="app">
        ///     The application builder.
        /// </param>
        /// <param name="handleProxyRequest">
        ///     A delegate that can resolve the destination Uri.
        /// </param>
        public static void RunProxy(
            this IApplicationBuilder app,
            HandleProxyRequest handleProxyRequest)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            if (handleProxyRequest == null)
            {
                throw new ArgumentNullException(nameof(handleProxyRequest));
            }

            app.UseMiddleware<ProxyMiddleware>(handleProxyRequest);
        }

        /// <summary>
        ///     Runs proxy forwarding requests to downstream server.
        /// </summary>
        /// <param name="app">
        ///     The application builder.
        /// </param>
        /// <param name="pathMatch">
        ///     Branches the request pipeline based on matches of the given
        ///     request path. If the request path starts with the given path,
        ///     the branch is executed.
        /// </param>
        /// <param name="handleProxyRequest">
        ///     A delegate that can resolve the destination Uri.
        /// </param>
        [Obsolete("Use app.Map(\"/path\", appProxy=> { appProxy.UseProxy(...); } instead. " +
                  "This will be removed in a future version", false)]
        public static void RunProxy(
            this IApplicationBuilder app,
            PathString pathMatch,
            HandleProxyRequest handleProxyRequest)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            app.Map(pathMatch, appInner =>
            {
                appInner.UseMiddleware<ProxyMiddleware>(handleProxyRequest);
            });
        }

        /// <summary>
        ///     Adds WebSocket proxy that forwards websocket connections
        ///     to destination Uri based the HttpContext if the request matches
        ///     a given path.
        /// </summary>
        /// <param name="app">
        ///     The application builder.
        /// </param>
        /// <param name="getUpstreamUri">
        ///     A function to get the uri to forward the websocket connection to. The
        ///     result of which must start with ws:// or wss://
        /// </param>
        public static void UseWebSocketProxy(
            this IApplicationBuilder app, 
            Func<HttpContext, UpstreamHost> getUpstreamUri)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            if (getUpstreamUri == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            app.UseMiddleware<WebSocketProxyMiddleware>(getUpstreamUri);
        }

        /// <summary>
        ///     Adds WebSocket proxy that forwards websocket connections
        ///     to destination Uri based the HttpContext if the request matches
        ///     a given path.
        /// </summary>
        /// <param name="app">
        ///     The application builder.
        /// </param>
        /// <param name="getUpstreamUri">
        ///     A function to get the uri to forward the websocket connection to. The
        ///     result of which must start with ws:// or wss://
        /// </param>
        /// <param name="configureClientOptions">
        ///     An action to allow customizing of the websocket client before initial
        ///     connection allowing you to set custom headers or adjust cookies.
        /// </param>
        public static void UseWebSocketProxy(
            this IApplicationBuilder app,
            Func<HttpContext, UpstreamHost> getUpstreamUri,
            Action<WebSocketClientOptions> configureClientOptions)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            if (getUpstreamUri == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            app.UseMiddleware<WebSocketProxyMiddleware>(getUpstreamUri, configureClientOptions);
        }
    }
}