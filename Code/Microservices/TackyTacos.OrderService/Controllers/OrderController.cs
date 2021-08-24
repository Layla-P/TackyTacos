using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using TackyTacos.OrderService.Models;
using TackyTacos.OrderService.Services;

namespace TackyTacos.OrderService.Controllers
{
	[Route("orders")]
	[ApiController]
	public class OrderController : Controller
	{
		private readonly OrdersService _ordersService;
		public OrderController(OrdersService ordersService)
		{
			_ordersService = ordersService;
		}
		//[HttpGet]
		//public async Task<ActionResult<List<TacoSpecial>>> GetSpecialsAsync()
		//{
		//	return await Task.FromResult(GetSpecialsList());
		//}

		//[HttpGet]
		//public async Task<HttpStatusCode> GetSpecialsAsync()
		//{
		//	var order = new Order
		//	{
		//		Id = Guid.NewGuid(),
		//		CreatedTime = DateTime.Now,
		//		OrderPlaced = DateTime.Now,
		//		OrderPaid = false,
		//		TotalPrice = 40M
		//	};

		//	await _ordersService.AddOrderAsync(order);

		//	return HttpStatusCode.OK;
		//}

		[HttpPost]
		public async Task<HttpStatusCode> Post([FromBody]Order order)
		{
			
			await _ordersService.AddOrderAsync(order);

			return HttpStatusCode.OK;
		}
	}
}
