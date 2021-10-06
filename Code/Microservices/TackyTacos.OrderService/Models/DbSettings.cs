

namespace TackyTacos.OrderService.Models
{
	public class DbSettings
	{
		public string DatabaseId { get; set; }
		public string EndpointUri { get; set; }
		public string AuthKey { get; set; }
		public string ContainerId { get; set; }
		public string PartitionKey  { get; set; }
	}
}
