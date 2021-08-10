using Microsoft.Extensions.DependencyInjection;
using System;
using RabbitMQ.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using TanzuTacos.PaymentService.Models;

namespace TanzuTacos.PaymentService.Helpers
{
	public static class IServiceCollectionExtensions
	{

		public static IServiceCollection SetUpRabbitMQ(this IServiceCollection services, IConfiguration config)
		{
			var settings = new RabbitMQSettings
			{
				ExchangeName = config.GetValue<string>("RabbitMQSettings:ExchangeName"),
				HostName = config.GetValue<string>("RabbitMQSettings:HostName"),
				ExchangeType = config.GetValue<string>("RabbitMQSettings:ExchangeType"),
			};
			
			services.Configure<RabbitMQSettings>(config.GetSection("RabbitMQSettings"));

			var factory = new ConnectionFactory() { HostName = "localhost" };
			var connection = factory.CreateConnection();
			var channel = connection.CreateModel();
			channel.ExchangeDeclare(exchange: settings.ExchangeName,
										type: settings.ExchangeType);

			services.AddSingleton(channel);

			return services;
		}
	}
}
