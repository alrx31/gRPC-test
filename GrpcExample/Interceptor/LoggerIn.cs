using Grpc.Core;
using Grpc.Core.Interceptors;

public class LoggingInt : Interceptor
{
    private readonly ILogger<LoggingInt> _logger;

    public LoggingInt(ILogger<LoggingInt> logger)
    {
        _logger = logger;
    }

    public override async Task DuplexStreamingServerHandler<TRequest, TResponse>(
        IAsyncStreamReader<TRequest> requestStream,
        IServerStreamWriter<TResponse> responseStream,
        ServerCallContext context,
        DuplexStreamingServerMethod<TRequest, TResponse> continuation)
    {
        _logger.LogInformation("Start Duplex Streaming");

        try
        {
            // Вызов continuation для продолжения обработки запросов
            await continuation(requestStream, responseStream, context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred during duplex streaming.");
            throw;
        }
        finally
        {
            _logger.LogInformation("End Duplex Streaming");
        }
    }
}
