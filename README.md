# NotificationDispatcher
The application is capable of sending different types of notifications (Email, Push, Windows). Making the consumer wait until notifications are sent in these different ways would not be efficient. To avoid this issue, we can use the C# channel, which will be responsible for temporarily holding the notifications until the Worker Service processes them.

`INotificationChannel` is responsible for allowing the business logic to send the notifications. `NotificationDispatcher` is responsible for reading the notifications stored in the channel and sending them to the respective external services. 

Some external services can receive a batch of notifications for performance improvement, which is why `NotificationChannel` implements `ReadBatchAsync`. This method operates as follows: if there is at least one message in the channel, it will only wait for other messages for `batchFulfilmentTimeInMilliseconds`. After that, it will send the command as a batch. This method also takes `batchSize` to limit the number of notifications that can be sent in a single batch. Without this limit, some external services might restrict the number of notifications in a batch.


Channels Temo Theory
There are bounded and unbonded channels, Bounded channels have a fixed capacity, meaning they can hold only a specified maximum number of items at any given time.
In the DI Container channel should be registred as a sginleton, multiple threads will be using its single instance:

