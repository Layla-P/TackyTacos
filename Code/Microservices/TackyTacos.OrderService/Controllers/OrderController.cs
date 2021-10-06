

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


		[HttpPost]
		public async Task<HttpStatusCode> Post([FromBody]Order order)
		{
			
			await _ordersService.AddOrderAsync(order);

			return HttpStatusCode.OK;
		}
	}
}
