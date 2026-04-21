// using RabbitMQ.Client;
// using System.Text;
// using System.Text.Json;
// using Microsoft.Extensions.Options;
// using Finlay.PharmaVigilance.Application.Interfaces;


// namespace Finlay.PharmaVigilance.Infrastructure.Messaging;


// public class RabbitMqMessageBus : IMessageBus
// {
//     private readonly RabbitMqSettings _settings;

//     public RabbitMqMessageBus(IOptions<RabbitMqSettings> options)
//     {
//         _settings = options.Value;
//     }

//     public async Task PublishAsync<T>(T message)
//     {
//         var factory = new ConnectionFactory
//         {
//             HostName = _settings.Host,
//             UserName = _settings.User,
//             Password = _settings.Password
//         };

//         await using var connection = await factory.CreateConnectionAsync();
//         await using var channel = await connection.CreateChannelAsync();

//         await channel.QueueDeclareAsync(
//             queue: _settings.QueueName,
//             durable: true,
//             exclusive: false,
//             autoDelete: false
//         );

//         var json = JsonSerializer.Serialize(message);
//         var body = Encoding.UTF8.GetBytes(json);

//         await channel.BasicPublishAsync(
//             exchange: "",
//             routingKey: _settings.QueueName,
//             body: body
//         );
//     }
// }



using System.Text;
using System.Text.Json;
using Finlay.PharmaVigilance.Application.Interfaces;
using RabbitMQ.Client;

namespace Finlay.PharmaVigilance.Infrastructure.Messaging;

public class RabbitMqEventBus : IEventBus
{
    private readonly ConnectionFactory _factory;

    public RabbitMqEventBus()
    {
        _factory = new ConnectionFactory()
        {
            HostName = "localhost",
            UserName = "guest",
            Password = "guest"
        };
    }

    public async Task PublishAsync<T>(T @event)
    {
        using var connection = await _factory.CreateConnectionAsync();
        using var channel = await connection.CreateChannelAsync();

        var queueName = typeof(T).Name;

        await channel.QueueDeclareAsync(queueName, durable: true, exclusive: false, autoDelete: false);

        var message = JsonSerializer.Serialize(@event);
        var body = Encoding.UTF8.GetBytes(message);

        await channel.BasicPublishAsync("", queueName, body);

        Console.WriteLine($"[x] Event sent: {queueName}");
    }
}