using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TanzuTacos.OrderService.Messaging;
using System.Threading;

namespace TanzuTacos.OrderService.Helpers
{
	public static class ApplicationBuilderExtensions
	{
		public static IHostedService RabbitMqConsumer { get; set; }

		public static IApplicationBuilder UseRabbitMqConsumer(this IApplicationBuilder app)
		{
			RabbitMqConsumer = app.ApplicationServices.GetService<Sender>();
			var hostApplicationLifetime = app.ApplicationServices.GetService<IHostApplicationLifetime>();

			hostApplicationLifetime.ApplicationStarted.Register(OnStarted);
			hostApplicationLifetime.ApplicationStopping.Register(OnStopping);

			return app;
		}

		private static void OnStarted()
		{
			var token = new CancellationToken();
			RabbitMqConsumer.StartAsync(token);
		}

		private static void OnStopping()
		{
			var token = new CancellationToken();
			RabbitMqConsumer.StopAsync(token);
		}
	}
}
