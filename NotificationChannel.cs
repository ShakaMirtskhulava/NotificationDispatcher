public class NotificationChannel : INotificationChannel
{
    private readonly Channel<Notification> _channel;

    public NotificationChannel()
    {
        _channel = Channel.CreateUnbounded<Notification>();
    }

    public async Task SendAsync(Notification notification)
    {
        await _channel.Writer.WriteAsync(notification);
    }

    public async Task<List<Notification>> ReadBatchAsync(int batchSize, int batchFulifilmentTimeInMileseconds)
    {
        var batch = new List<Notification>(batchSize);

        //Thread will be released from here and flow will be paused, untill
        //At least 1 notification is not available in the channel
        while (await _channel.Reader.WaitToReadAsync())
        {
            try
            {
                if (batchFulifilmentTimeInMileseconds > 0)
                {
                    using var cancellationTokenSource = new CancellationTokenSource();
                    cancellationTokenSource.CancelAfter(batchFulifilmentTimeInMileseconds);

                    while (await _channel.Reader.WaitToReadAsync(cancellationTokenSource.Token))
                    {
                        while (_channel.Reader.TryRead(out var notification))
                        {
                            batch.Add(notification);
                            if (batch.Count >= batchSize)
                                return batch;
                        }
                    }
                }
            }
            catch(OperationCanceledException)
            {
                return batch;
            }

        }
        throw new InvalidFlowException();
    }


}
