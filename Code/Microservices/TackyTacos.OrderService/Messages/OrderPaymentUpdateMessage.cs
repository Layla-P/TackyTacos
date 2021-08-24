using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TackyTacos.OrderService.Messages
{
	public class OrderPaymentUpdateMessage
	{
		public Guid OrderId { get; set; }
		public bool PaymentSuccess { get; set; }
	}
}
