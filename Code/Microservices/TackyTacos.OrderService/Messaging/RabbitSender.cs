using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System;
using System.Text;
using System.Text.Json;
using TackyTacos.OrderService.Models;

namespace TackyTacos.OrderService.Messaging
{
	public class RabbitSender
	{
		private readonly IModel _channel;
		private readonly RabbitMQSettings _rabbitSettings;
		public RabbitSender(IOptions<RabbitMQSettings> rabbitSettings,IModel channel)
		{
			_channel = channel;
			_rabbitSettings = rabbitSettings.Value;
		}

		public void PublishMessage<T>(T entity, string topic) where T: class
		{
			var message =  JsonSerializer.Serialize(entity);
			//topic should become enum or similar
			var body = Encoding.UTF8.GetBytes(message);
				_channel.BasicPublish(exchange: _rabbitSettings.ExchangeName,
											 routingKey: topic,
											 basicProperties: null,
											 body: body);
				Console.WriteLine(" [x] Sent '{0}':'{1}'", topic, message);
			
		}
	}
}
