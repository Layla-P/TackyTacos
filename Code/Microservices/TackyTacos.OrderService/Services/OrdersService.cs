

namespace TackyTacos.OrderService.Services
{
	public class OrdersService
	{
		private readonly OrdersRepository _ordersRepository;
		private readonly RabbitSender _rabbitSender;
		public OrdersService(OrdersRepository ordersRepository, RabbitSender rabbitSender)
		{
			_ordersRepository = ordersRepository;
			_rabbitSender = rabbitSender;
		}

		public async Task AddOrderAsync(Order order)
		{
			var orderEntity = await _ordersRepository.AddOrUpdateAsync(order);
			
			_rabbitSender.PublishMessage<Order>(orderEntity, "order.requestpayment");
		}

		public async Task UpdateOrderPaymentStatusAsync(Order order)
		{
			await _ordersRepository.ReplaceOrderItemAsync(order);
		}
	}
}
