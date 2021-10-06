

namespace TackyTacos.NotificationService.Models
{
	public class NotificationMessage
	{
		public Guid UserId { get; set; }
		public Guid OrderId { get; set; }
		public MessageType MessageType { get; set; }
		public string Message { get; set; }
		public string Email { get; set; }
		public string PhoneNumber { get; set; }
	}
	public enum MessageType
	{
		Default = 0,
		Email = 10,
		Sms = 20
	}
	
}
