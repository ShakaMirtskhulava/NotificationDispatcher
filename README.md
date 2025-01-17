# NotificationDispatcher
Application might be sending a different types of notifications(Email, Push, Windows), make the consumer wait until notifications are sent in those differnet ways
would not be efficient. To avoid this issue we can use the C# channel, which'll be responsible for hodling the notification temporarily, untill the Wroker Service 
Processes them.
INotificationChannel_ is responsible for allowing the business logic to send the notifications.
NotificationDispatcher is responsible reding the notification stored in the channel and sending them to the respective external services.
Some external services can receive a batch of the notifications(for performace imporvement), that's why NotificationChannel implements ReadBatchAsync, 
which is working in following way:
If there is at least one message in the channel, it'll only wait to the other messages for batchFulifilmentTimeInMileseconds, after that it'll be sending 
the command as batch, this method also takes batchSize, to limit the number of notifications that could be sent in a single batch. If we won't do that 
some external services might limit the number of notifications in a batch.


