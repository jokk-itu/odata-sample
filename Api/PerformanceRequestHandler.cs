using System.Diagnostics;

namespace Api;

public class PerformanceRequestHandler : DelegatingHandler
{
    private readonly ILogger<PerformanceRequestHandler> _logger;

    public PerformanceRequestHandler(
        ILogger<PerformanceRequestHandler> logger)
    {
        _logger = logger;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        _logger.LogDebug("Begin request {Uri}", request.RequestUri);
        var watch = Stopwatch.StartNew();
        var response = await base.SendAsync(request, cancellationToken);
        watch.Stop();
        _logger.LogInformation("Request finished with code {HttpResponseCode} in {ElapsedMilliseconds}", response.StatusCode, watch.ElapsedMilliseconds);
        return response;
    }
}