
// using System.Security.Cryptography;
// using System.Text;
// using System.Text.Json;
// using Finlay.PharmaVigilance.Domain.Events;
// using Finlay.PharmaVigilance.Infrastructure.Messaging;
// using Microsoft.Extensions.Hosting;
// using Microsoft.Extensions.Options;
// using RabbitMQ.Client;
// using RabbitMQ.Client.Events;

// namespace Finlay.PharmaVigilance.Infrastructure.Consumers;


// public class RabbitMqListener : BackgroundService
// {
//     private readonly RabbitMqSettings _settings;
//     private readonly ReportCreatedConsumer _consumer;

//     private IConnection _connection = null!;
//     private IChannel _channel = null!;

//     public RabbitMqListener(
//         IOptions<RabbitMqSettings> options,
//         ReportCreatedConsumer consumer)
//     {
//         _settings = options.Value;
//         _consumer = consumer;
//     }

//     protected override async Task ExecuteAsync(CancellationToken stoppingToken)
//     {
//         var factory = new ConnectionFactory
//         {
//             HostName = _settings.Host,
//             UserName = _settings.User,
//             Password = _settings.Password
//         };

//         _connection = await factory.CreateConnectionAsync(stoppingToken);
//         _channel = await _connection.CreateChannelAsync(cancellationToken: stoppingToken);

//         await _channel.QueueDeclareAsync(
//             queue: _settings.QueueName,
//             durable: true,
//             exclusive: false,
//             autoDelete: false,
//             cancellationToken: stoppingToken
//         );

//         var consumer = new AsyncEventingBasicConsumer(_channel);

//         consumer.ReceivedAsync += async (sender, ea) =>
//         {
//             try
//             {
//                 var body = ea.Body.ToArray();
//                 var json = Encoding.UTF8.GetString(body);

//                 var message = JsonSerializer.Deserialize<ReportCreatedEvent>(json);

//                 if (message != null)
//                 {
//                     await _consumer.Handle(message);
//                 }

//                 await _channel.BasicAckAsync(
//                     deliveryTag: ea.DeliveryTag,
//                     multiple: false
//                 );
//             }
//             catch (Exception ex)
//             {

//                 Console.WriteLine(ex);

//                 await _channel.BasicNackAsync(
//                                     deliveryTag: ea.DeliveryTag,
//                                     multiple: false,
//                                     requeue: true
//                                 );
//             }


//         };

//         await _channel.BasicConsumeAsync(
//             queue: _settings.QueueName,
//             autoAck: false,
//             consumer: consumer,
//             cancellationToken: stoppingToken
//         );
//     }

//     public override async Task StopAsync(CancellationToken cancellationToken)
//     {
//         if (_channel != null)
//             await _channel.CloseAsync(cancellationToken);

//         if (_connection != null)
//             await _connection.CloseAsync(cancellationToken);

//         await base.StopAsync(cancellationToken);
//     }


// }