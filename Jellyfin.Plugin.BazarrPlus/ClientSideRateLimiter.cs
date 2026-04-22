using System;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.RateLimiting;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;

namespace Jellyfin.Plugin.BazarrPlus;

internal sealed class ClientSideRateLimitedHandler : DelegatingHandler
{
    private readonly ILogger<ClientSideRateLimitedHandler> _logger;

    private static readonly RateLimiter _rateLimiter = new FixedWindowRateLimiter(new FixedWindowRateLimiterOptions
    {
        PermitLimit = 5,
        Window = TimeSpan.FromSeconds(1)
    });

    internal ClientSideRateLimitedHandler(ILogger<ClientSideRateLimitedHandler> logger)
        : base(new SocketsHttpHandler
        {
            AutomaticDecompression = DecompressionMethods.All,
            RequestHeaderEncodingSelector = (_, _) => Encoding.UTF8,

            // Bazarr+'s waitress WSGI server closes idle keep-alive connections
            // unilaterally. Without these bounds, HttpClientFactory's long-lived
            // handler reuses half-closed connections and surfaces a spurious
            // "HttpIOException: The response ended prematurely (ResponseEnded)".
            PooledConnectionLifetime = TimeSpan.FromSeconds(60),
            PooledConnectionIdleTimeout = TimeSpan.FromSeconds(5),
        })
    {
        _logger = logger;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        using RateLimitLease lease = await _rateLimiter.AcquireAsync(cancellationToken: cancellationToken).ConfigureAwait(false);

        if (lease.IsAcquired)
        {
            return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
        }

        var response = new HttpResponseMessage(HttpStatusCode.TooManyRequests);
        if (lease.TryGetMetadata(MetadataName.RetryAfter, out TimeSpan retryAfter))
        {
            _logger.LogDebug("Unable to acquire rate limit lease, waiting {RetryAfter}", retryAfter);
            response.Headers.Add(
                HeaderNames.RetryAfter,
                retryAfter.TotalSeconds.ToString("F0", NumberFormatInfo.InvariantInfo));
        }

        return response;
    }
}
