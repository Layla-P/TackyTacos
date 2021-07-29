using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TanzuTacos.WebApp.Models
{
	public class TacoExtra
	{
		public Extra Extra { get; set; }

		public int ToppingId { get; set; }

		public int TacoId { get; set; }
	}

	public class Extra
	{
		public int Id { get; set; }

		public string Name { get; set; }

		public decimal Price { get; set; }

		public string GetFormattedPrice() => Price.ToString("0.00");
	}
}
