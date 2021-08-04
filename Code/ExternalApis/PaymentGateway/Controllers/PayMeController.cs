using Microsoft.AspNetCore.Mvc;
using System;

namespace PaymentGateway.Controllers
{
	[ApiController]
	[Route("payme")]
	public class PayMeController : Controller
	{
		[HttpPost]
		public IActionResult Post([FromBody] PaymentDto payment)
		{
			
			int num = new Random().Next(1000);
			if (num > 500)
			{
				Console.WriteLine($"Order id:{payment.OrderId} for £{payment.Total} was successful!");
				return Ok(true);
			}
			Console.WriteLine($"Order id:{payment.OrderId} for £{payment.Total} was a failure :-(");
			return Ok(false);
		}
	}
}