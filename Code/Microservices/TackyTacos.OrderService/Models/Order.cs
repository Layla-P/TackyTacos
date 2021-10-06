
namespace TackyTacos.OrderService.Models
{
	public class Order
	{
		[JsonPropertyName("id")]
		public Guid Id { get; set; }

		[JsonPropertyName("WeekDay")]
		public string Partition { get; set; }

		public Guid UserId { get; set; }

		public DateTime CreatedTime { get; set; }

		//public Address DeliveryAddress { get; set; } = new Address();

		//public LatLong DeliveryLocation { get; set; }

		public decimal TotalPrice { get; set; }
		public DateTime OrderPlaced { get; set; }
		public bool OrderPaid { get; set; }

		public override string ToString() => JsonSerializer.Serialize(this);

		
}
}
