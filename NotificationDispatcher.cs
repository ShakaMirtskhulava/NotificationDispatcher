
using DMS.Application.Models.Notification;
using DMS.Application.Utilities;

namespace DMS.API.Infrastructure.Workers;

public class NotificationDispatcher : BackgroundService
{
    private readonly ILogger _logger;
    private readonly INotificationChannel _notificationChannel;

    public NotificationDispatcher(ILogger<NotificationDispatcher> logger, INotificationChannel notificationChannel)
    {
        _logger = logger;
        _notificationChannel = notificationChannel;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                _logger.LogInformation("Worker running at: {Time}", DateTimeOffset.UtcNow);
                
                var notifications = await _notificationChannel.ReadBatchAsync(5,5000);
                
                _logger.LogInformation("Get {Count} notifications to send", notifications.Count);
                
                foreach (var notification in notifications)
                    await SendNotificationAsync(notification, stoppingToken);

                _logger.LogInformation("{Count} Notifications sent successfully",notifications.Count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing notifications");
            }
        }
    }

    private static async Task<bool> SendNotificationAsync(Notification notification,CancellationToken cancellationToken)
    {
        // Send notification
        return true;
    }
}
