using Microsoft.Extensions.Options;
using System;
using System.Linq;
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

			var id1 = new Guid("FE7370FA-C7FC-44C9-9466-11D79A1F3B6D");
			var id2 = new Guid("3FE461AF-C63E-41A5-B454-674000BA80BC");
			var id3 = new Guid("657B0F94-2850-4DC3-A625-121C7EA16ED7");
			var userId1 = new Guid("41E20F55-5819-460D-B357-4B12ECCED538");
			var userId2 = new Guid("B98E2752-E652-42D0-B71B-D9109CCB702F");


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
			Console.WriteLine($"Price: {orderOneEntity.TotalPrice} and Order paid: {orderOneEntity.OrderPaid} and value:{orderOneEntity.TotalPrice}");
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
			Console.WriteLine("=====Create test two=====");
			Console.WriteLine($"Price: {orderTwoEntity.Id} and Order paid: {orderTwoEntity.OrderPaid} and value:{orderTwoEntity.TotalPrice}");
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
			var orderThreeEntity = await orderRepository.AddOrUpdateAsync(order3);
			Console.WriteLine("=====Create test three=====");
			Console.WriteLine($"Price: {orderThreeEntity.Id} and Order paid: {orderThreeEntity.OrderPaid} and value:{orderThreeEntity.TotalPrice}");
			Console.WriteLine("==========");

			// Update Orders
			order2.OrderPaid = true;			
			orderTwoEntity = await orderRepository.AddOrUpdateAsync(order2);
			Console.WriteLine("=====Update=====");
			Console.WriteLine(order2.Id);
			Console.WriteLine($"ID: {orderTwoEntity.Id} and Order paid: {orderTwoEntity.OrderPaid}");
			Console.WriteLine("==========");

			//Get order by Id
			orderTwoEntity = await orderRepository.GetByIdAsync(id2);
			Console.WriteLine("=====GetByIdAsync=====");
			Console.WriteLine($"ID: {orderTwoEntity.Id} and Order paid: {orderTwoEntity.OrderPaid} and value:{orderTwoEntity.TotalPrice}");
			Console.WriteLine("==========");

			//Where
			var orders = await orderRepository.WhereAsync(order => order.UserId == userId2);
			Console.WriteLine("=====Where=====");
			Console.WriteLine(orders.Count());
			foreach (var order in orders)
			{
				Console.WriteLine($"ID: {order.Id} UserId:{order.UserId} and price: {order.TotalPrice}");
			}
		}
	}
}
