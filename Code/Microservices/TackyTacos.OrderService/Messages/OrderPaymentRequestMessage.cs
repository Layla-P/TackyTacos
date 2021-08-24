using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TackyTacos.OrderService.Messages
{
	public class OrderPaymentRequestMessage
	{
		public Guid OrderId { get; set; }
		public int Total { get; set; }
		public string CardNumber { get; set; }
		public string CardName { get; set; }
		public string CardExpiration { get; set; }
	}
}
