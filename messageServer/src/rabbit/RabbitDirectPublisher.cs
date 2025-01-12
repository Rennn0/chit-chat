using System.Text;
using LLibrary.Guards;
using LLibrary.Logging;
using llibrary.rabbit;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace messageServer.src.rabbit;

public class RabbitDirectPublisher : RabbitBasicObject
{
    private const string _q = "direct-q";

    public RabbitDirectPublisher(string host, string username, string password)
        : base(host, username, password, nameof(RabbitDirectPublisher)) { }

    public override async Task InitializeAsync()
    {
        try
        {
            await base.InitializeAsync();
            Guard.AgainstNull(Connection);
            _channel = await Connection.CreateChannelAsync();

            await _channel.QueueDeclareAsync(queue: _q, durable: true, autoDelete: false);
            AsyncEventingBasicConsumer consumer = new(_channel);
            consumer.ReceivedAsync += Consumer_ReceivedAsync;

            await _channel.BasicConsumeAsync(queue: _q, autoAck: true, consumer: consumer);

            Diagnostics.LOG_INFO(nameof(RabbitDirectPublisher));
        }
        catch (Exception e)
        {
            Diagnostics.LOG_ERROR(e.Message);
        }
    }

    private async Task Consumer_ReceivedAsync(object sender, BasicDeliverEventArgs @event)
    {
        try
        {
            Guard.AgainstNull(_channel);
            Guard.AgainstNull(@event.BasicProperties.ReplyTo);

            string message = Encoding.UTF8.GetString(@event.Body.ToArray());

            //
            // TODO rame logika aq
            //

            BasicProperties replyProps =
                new() { CorrelationId = @event.BasicProperties.CorrelationId };
            await _channel.BasicPublishAsync(
                exchange: string.Empty,
                routingKey: @event.BasicProperties.ReplyTo,
                mandatory: true,
                basicProperties: replyProps,
                body: Encoding.UTF8.GetBytes(message + "Received")
            );
        }
        catch (Exception e)
        {
            Diagnostics.LOG_ERROR(e.Message);
        }
    }
}
