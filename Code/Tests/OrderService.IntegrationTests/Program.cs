using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using TanzuTacos.OrderService.Data;
using TanzuTacos.OrderService.Models;

namespace OrderService.IntegrationTests
{
	class Program
	{
		static void Main(string[] args)
		{
			Task t = InternalHandler();
			t.Wait();
			Console.ReadLine();
		}

		private static async Task InternalHandler()
		{
			var settings = new DbSettings
			{
				DatabaseId = "OrdersTestDB",
				EndpointUri = "",
				AuthKey = ""
			};
			var options = Options.Create<DbSettings>(settings);

			var context = new DocumentDbContext(options);


			await context.CreateDatabaseAndCollectionAsync();
			var orderRepository = new OrdersRepository(context);

			var id1 = Guid.NewGuid();
			var id2 = Guid.NewGuid();
			var id3 = Guid.NewGuid();
			var userId1 = Guid.NewGuid();
			var userId2 = Guid.NewGuid();


			var order1 = new Order
			{
				Id = id1,
				UserId = userId1,
				CreatedTime = DateTime.Now,
				TotalPrice = 10M,
				OrderPlaced = DateTime.Now,
				OrderPaid = false
			};
			//Create Orders
			var orderOneEntity = await orderRepository.AddOrUpdateAsync(order1);
			Console.WriteLine("=====Create test one=====");
			Console.WriteLine($"Price: {orderOneEntity.TotalPrice} and Order paid: {orderOneEntity.OrderPaid}");
			Console.WriteLine("==========");
			var order2 = new Order
			{
				Id = id2,
				UserId = userId2,
				CreatedTime = DateTime.Now,
				TotalPrice = 20M,
				OrderPlaced = DateTime.Now,
				OrderPaid = false
			};
			var orderTwoEntity = await orderRepository.AddOrUpdateAsync(order2);
			Console.WriteLine("=====Create test three=====");
			Console.WriteLine($"Price: {orderTwoEntity.Id} and Order paid: {orderTwoEntity.OrderPaid}");
			Console.WriteLine("==========");

			var order3 = new Order
			{
				Id = id3,
				UserId = userId2,
				CreatedTime = DateTime.Now,
				TotalPrice = 30M,
				OrderPlaced = DateTime.Now,
				OrderPaid = false
			};
			var orderThreeEntity = await orderRepository.AddOrUpdateAsync(order2);
			Console.WriteLine("=====Create test two=====");
			Console.WriteLine($"Price: {orderThreeEntity.Id} and Order paid: {orderThreeEntity.OrderPaid}");
			Console.WriteLine("==========");

			// Update Orders
			order2.OrderPaid = true;
			orderTwoEntity = await orderRepository.AddOrUpdateAsync(order2);
			Console.WriteLine("=====Update=====");
			Console.WriteLine($"Price: {orderTwoEntity.Id} and Order paid: {orderTwoEntity.OrderPaid}");
			Console.WriteLine("==========");

			//Get order by Id
			orderTwoEntity = await orderRepository.GetByIdAsync(id2);
			Console.WriteLine("=====GetByIdAsync=====");
			Console.WriteLine($"Price: {orderTwoEntity.Id} and Order paid: {orderTwoEntity.OrderPaid}");
			Console.WriteLine("==========");

			//Where
			var orders = await orderRepository.WhereAsync(order => order.UserId == userId2);
			Console.WriteLine("=====Where=====");

			foreach(var order in orders)
			{
				Console.WriteLine($"UserId:{order.UserId} and price: {order.TotalPrice}");
			}
		}
	}
}
