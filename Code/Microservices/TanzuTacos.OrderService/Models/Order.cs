using Newtonsoft.Json;
using System;

namespace TanzuTacos.OrderService.Models
{
	public class Order
	{
		[JsonProperty(PropertyName ="id")]
		public Guid Id { get; set; }

		public Guid UserId { get; set; }

		public DateTime CreatedTime { get; set; }

		//public Address DeliveryAddress { get; set; } = new Address();

		//public LatLong DeliveryLocation { get; set; }

		public decimal TotalPrice { get; set; }
		public DateTime OrderPlaced { get; set; }
		public bool OrderPaid { get; set; }
	}
}
