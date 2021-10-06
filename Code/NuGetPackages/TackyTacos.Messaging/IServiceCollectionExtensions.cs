﻿

namespace TackyTacos.Messaging
{
	public static class IServiceCollectionExtensions
	{

		public static IServiceCollection SetUpRabbitMQ(this IServiceCollection services, IConfiguration config)
		{
			var configSection = config.GetSection("RabbitMQSettings");

			services.Configure<RabbitMQSettings>(configSection);

			var settings = new RabbitMQSettings();
			configSection.Bind(settings);

			var factory = new ConnectionFactory() { HostName = settings.HostName };
			var connection = factory.CreateConnection();
			var channel = connection.CreateModel();
			channel.ExchangeDeclare(exchange: settings.ExchangeName,
										type: settings.ExchangeType);

			services.AddSingleton(channel);

			return services;
		}
	}
}
