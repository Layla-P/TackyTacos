using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TanzuTacos.Messaging;

namespace TanzuTacos.PaymentService.Messaging
{
	public class RabbitReceiver : IHostedService
	{
		private readonly RabbitMQSettings _rabbitSettings;
		private readonly IModel _channel;
		public RabbitReceiver(IOptions<RabbitMQSettings> rabbitSettings, IModel channel)
		{
			_rabbitSettings = rabbitSettings.Value;
			_channel = channel;
		}
		public  Task StartAsync(CancellationToken cancellationToken)
		{
			DoStuff();
			return Task.CompletedTask;
		}

		public Task StopAsync(CancellationToken cancellationToken)
		{
			_channel.Dispose();
			return Task.CompletedTask;
		}

		private void DoStuff()
		{
			_channel.ExchangeDeclare(exchange: _rabbitSettings.ExchangeName,
				type: _rabbitSettings.ExchangeType);	

			var queueName = _channel.QueueDeclare().QueueName;


				_channel.QueueBind(queue: queueName,
								  exchange: _rabbitSettings.ExchangeName,
								  routingKey: "order.requestpayment");
			

			Console.WriteLine(" [*] Waiting for messages. To exit press CTRL+C");

			var consumer = new EventingBasicConsumer(_channel);
			consumer.Received += (model, ea) =>
			{
				var body = ea.Body.ToArray();
				var message = Encoding.UTF8.GetString(body);
				var routingKey = ea.RoutingKey;
				Console.WriteLine(" [x] Received '{0}':'{1}'",
								  routingKey,
								  message);
			};
			_channel.BasicConsume(queue: queueName,
								 autoAck: true,
								 consumer: consumer);

			Console.WriteLine(" Press [enter] to exit.");
			Console.ReadLine();
		}
	}
}
