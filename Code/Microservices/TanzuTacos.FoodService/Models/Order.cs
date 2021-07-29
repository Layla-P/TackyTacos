using System;
using System.Collections.Generic;
using System.Linq;

namespace TanzuTacos.FoodService.Models
{
	public class Order
	{
		public int OrderId { get; set; }

		public string UserId { get; set; }

		public DateTime CreatedTime { get; set; }

		//public Address DeliveryAddress { get; set; } = new Address();

		//public LatLong DeliveryLocation { get; set; }

		public List<Taco> Tacos { get; set; } = new();

		public decimal GetTotalPrice() => Tacos.Sum(p => p.GetTotalPrice());

		public string GetFormattedTotalPrice() => GetTotalPrice().ToString("0.00");
	}
}
