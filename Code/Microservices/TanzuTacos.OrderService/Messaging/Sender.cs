using Microsoft.Extensions.Hosting;
using Steeltoe.Messaging.RabbitMQ.Core;
using Steeltoe.Messaging.RabbitMQ.Extensions;
using System;
using System.Threading;
using System.Threading.Tasks;
using TanzuTacos.OrderService.Models;

namespace TanzuTacos.OrderService.Messaging
{
	public class Sender : IHostedService
	{
		private RabbitTemplate template;

		public Sender(IServiceProvider services)
		{
			template = services.GetRabbitTemplate();
		}

		public Task StartAsync(CancellationToken cancellationToken)
		{
			
			return Task.CompletedTask;
		}

		public Task StopAsync(CancellationToken cancellationToken)
		{			
			return Task.CompletedTask;
		}

		private void Send(Order order)
		{
			template.ConvertAndSend("paymentqueue", order);
		}
	}


}
