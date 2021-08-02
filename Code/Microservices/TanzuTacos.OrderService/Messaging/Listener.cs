using Microsoft.Extensions.Logging;
using Steeltoe.Messaging.RabbitMQ.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TanzuTacos.OrderService.Messaging
{
	public class Listener
	{
		private ILogger<Listener> logger;
		public Listener(ILogger<Listener> logger)
		{
			this.logger = logger;
		}

		[RabbitListener("orderqueue")]
		public void Listen(string input)
		{
			// Process message from myqueue
			logger.LogInformation(input);
		}
	}
}
