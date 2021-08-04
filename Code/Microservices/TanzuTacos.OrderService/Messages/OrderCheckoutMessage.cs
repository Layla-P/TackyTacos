using System;

namespace TanzuTacos.OrderService.Messages
{
	public class OrderCheckoutMessage
	{
		public Guid OrderId { get; set; }
		public Guid UserId { get; set; }

		//payment information
		public string CardNumber { get; set; }
		public string CardName { get; set; }
		public string CardExpiration { get; set; }
		public string CvvCode { get; set; }

		public int OrderTotal { get; set; }
	}
}
