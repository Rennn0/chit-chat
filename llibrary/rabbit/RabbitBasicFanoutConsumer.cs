﻿using llibrary.Guards;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace llibrary.Rabbit;

public class RabbitBasicFanoutConsumer : RabbitRootConsumer
{
    public RabbitBasicFanoutConsumer(
        string host,
        string username,
        string password,
        string exchange = "amq.fanout",
        int port = 5672
    )
        : base(exchange, host, username, password, port)
    {
    }

    public override async Task InitializeAsync()
    {
        await base.InitializeAsync();
        Guard.AgainstNull(Connection);

        _channel = await Connection.CreateChannelAsync();
        QueueDeclareOk q = await _channel.QueueDeclareAsync(
            durable: false,
            exclusive: true,
            autoDelete: true,
            arguments: null
        );
        await _channel.QueueBindAsync(
            queue: q.QueueName,
            exchange: _exchange,
            routingKey: string.Empty
        );

        _consumer = new AsyncEventingBasicConsumer(_channel);
        await _channel.BasicConsumeAsync(queue: q.QueueName, autoAck: true, consumer: _consumer);
    }
}