namespace SWD.SheritonHotel.API.WebSocket
{
    public class ApplicationWorker : BackgroundService
    {
        private readonly ILogger<ApplicationWorker> _logger;
        private readonly SocketIOServer _socketIOServer;

        public ApplicationWorker(ILogger<ApplicationWorker> logger, SocketIOServer socketIOServer)
        {
            _logger = logger;
            _socketIOServer = socketIOServer;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            _socketIOServer.Start();
            await Task.CompletedTask;
        }
    }
}
