using WorkerService.Helpers;

namespace ManasChatDemon;

public class Worker : BackgroundService
{
    public const int DelayInSeconds = 1800000;
    private readonly ILogger<Worker> _logger;
    private readonly DbHelper _dbHelper = new DbHelper();

    public Worker(ILogger<Worker> logger)
    {
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _dbHelper.ClearNotVerifiedUsers();
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            await Task.Delay(DelayInSeconds, stoppingToken);
        }
    }
}