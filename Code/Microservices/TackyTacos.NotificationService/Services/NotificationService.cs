﻿

namespace TackyTacos.NotificationService.Services
{
	public class NotificationService
	{

		public async Task SendNotification(NotificationMessage message)
		{
			Console.WriteLine($"Sending message: {message.Message} to userId: {message.UserId} via {message.MessageType}");
		}
	}
}
