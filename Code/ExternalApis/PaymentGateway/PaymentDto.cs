using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway
{
	public class PaymentDto
	{
		public Guid OrderId { get; set; }
		public int Total { get; set; }

	}
}
