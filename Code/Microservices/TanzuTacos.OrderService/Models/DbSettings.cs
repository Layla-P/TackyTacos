using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TanzuTacos.OrderService.Models
{
	public class DbSettings
	{
		public string DatabaseId { get; set; }
		public string EndpointUri { get; set; }
		public string AuthKey { get; set; }
	}
}
