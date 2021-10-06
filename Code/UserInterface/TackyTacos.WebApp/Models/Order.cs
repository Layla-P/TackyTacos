using System;
using System.Collections.Generic;
using System.Linq;

namespace TackyTacos.WebApp.Models
{
	public class Order
	{
		public Order()
		{
			UserId = Guid.Empty;
		}
		public Order(Guid userId)
		{
			UserId = userId;
		}
		public Guid Id { get; set; } = Guid.NewGuid();

		public Guid UserId { get; set; }

		public DateTime CreatedTime { get; set; } = DateTime.Now;

		public decimal TotalPrice { get; set; }

		//public Address DeliveryAddress { get; set; } = new Address();

		//public LatLong DeliveryLocation { get; set; }

		public List<Taco> Tacos { get; set; } = new();

		public decimal GetTotalPrice()
		{

			TotalPrice = Tacos.Sum(p => p.GetTotalPrice());
			return TotalPrice;
		}


		public string GetFormattedTotalPrice() => GetTotalPrice().ToString("0.00");
	}
}
