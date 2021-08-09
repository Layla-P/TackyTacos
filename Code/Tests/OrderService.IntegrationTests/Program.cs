using Azure.Cosmos;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using TanzuTacos.OrderService.Data;
using TanzuTacos.OrderService.Models;

namespace OrderService.IntegrationTests
{
	class Program
	{
		static async Task Main(string[] args)
		{
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
				OrderPaid = false,
				Partition = DateTime.Now.DayOfWeek.ToString()
			};
			var order2 = new Order
			{
				Id = id2,
				UserId = userId2,
				CreatedTime = DateTime.Now,
				TotalPrice = 20M,
				OrderPlaced = DateTime.Now,
				OrderPaid = false,
				Partition = DateTime.Now.DayOfWeek.ToString()
			};


			var settings = new DbSettings
			{
				DatabaseId = "OrdersTestDB",
				ContainerId = "OrdersTestContainer",
				EndpointUri = "https://localhost:8081",
				AuthKey = "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==", // this key is from a local emulator - not a live instance!!!!
				PartitionKey = "/WeekDay"
			};
			var options = Options.Create<DbSettings>(settings);

			var context = new CosmosDbContext(options);
			var repo = new OrdersRepository(context);

			await Program.AddItemsToContainerAsync(repo, order1, order2);
			await Program.GetAllByIdAsync(repo);
			await Program.ReplaceOrderItemAsync(repo, order1);
			await Program.GetAllByIdAndNotPaidAsync(repo);
			await Program.DeleteOrderItemAsync(repo, order1);
			//await Program.DeleteDatabaseAndCleanupAsync(cosmosClient, settings);


			Console.ReadLine();
		}
		

		/// <summary>
		/// Add Family items to the container
		/// </summary>
		private static async Task AddItemsToContainerAsync(OrdersRepository repo, Order order1, Order order2)
		{
			

			
			try
			{
				// Read the item to see if it exists.  
				var orderResponse1 = await repo.GetByIdAsync(order1);
				Console.WriteLine("Item in database with id: {0} already exists\n", orderResponse1.Id);
			}
			catch (CosmosException ex) when (ex.Status == (int)HttpStatusCode.NotFound)
			{
				// Create an item in the container representing the Andersen family. Note we provide the value of the partition key for this item, which is "Andersen"
				var order1Response = await repo.AddOrUpdateAsync(order1);

				// Note that after creating the item, we can access the body of the item with the Resource property off the ItemResponse.
				Console.WriteLine("Created item in database with id: {0}\n", order1Response.Id);
			}


			// Create an item in the container representing the Wakefield family. Note we provide the value of the partition key for this item, which is "Wakefield"

			try
			{
				// Read the item to see if it exists.  
				var orderResponse2 = await repo.GetByIdAsync(order2);
				Console.WriteLine("Item in database with id: {0} already exists\n", orderResponse2.Id);
			}
			catch (CosmosException ex) when (ex.Status == (int)HttpStatusCode.NotFound)
			{
				// Create an item in the container representing the Andersen family. Note we provide the value of the partition key for this item, which is "Andersen"
				var orderResponse2 = await repo.AddOrUpdateAsync(order2);

				// Note that after creating the item, we can access the body of the item with the Resource property off the ItemResponse.
				Console.WriteLine("Created item in database with id: {0}\n", orderResponse2.Id);
			}
		}

		/// <summary>
		/// Run a query (using Azure Cosmos DB SQL syntax) against the container
		/// </summary>
		private static async Task GetAllByIdAsync(OrdersRepository repo)
		{
			var id = "fe7370fa-c7fc-44c9-9466-11d79a1f3b6d";

			Console.WriteLine("Running query: {0}\n", id);

			List<Order> orders = await repo.GetAllByIdAsync(id);

			foreach (var order in orders)
			{
				Console.WriteLine("\tRead {0}\n", order);
			}
		}

		/// <summary>
		/// Run a query (using Azure Cosmos DB SQL syntax) against the container
		/// </summary>
		private static async Task GetAllNotPaidAsync(OrdersRepository repo)
		{
	
			List<Order> orders = await repo.GetAllNotPaidAsync();

			foreach (var order in orders)
			{
				Console.WriteLine("\tRead Not Paid {0}\n", order);
			}
		}

		/// <summary>
		/// Replace an item in the container
		/// </summary>
		private static async Task ReplaceOrderItemAsync(OrdersRepository repo, Order order)
		{

			var orderResponse = await repo.GetByIdAsync(order);
			Order itemBody = orderResponse;

			// update OrderPaid status from false to true
			itemBody.OrderPaid = true;

			// replace the item with the updated content
			orderResponse = await repo.ReplaceOrderItemAsync(itemBody);
			Console.WriteLine("Updated Order [{0}].\n \tBody is now: {1}\n", itemBody.Id, orderResponse);
		}

		/// <summary>
		/// Run a query (using Azure Cosmos DB SQL syntax) against the container
		/// </summary>
		private static async Task GetAllByIdAndNotPaidAsync(OrdersRepository repo)
		{
			var id = "fe7370fa-c7fc-44c9-9466-11d79a1f3b6d";

			Console.WriteLine("Running query: {0}\n", id);

			List<Order> orders = await repo.GetAllByIdAsync(id, true);

			foreach (var order in orders)
			{
				Console.WriteLine("\tRead Not Paid {0}\n", order);
			}
		}

		/// <summary>
		/// Delete an item in the container
		/// </summary>
		private static async Task DeleteOrderItemAsync(OrdersRepository repo, Order order)
		{

			// Delete an item. Note we must provide the partition key value and id of the item to delete
			await repo.DeleteOrderItemAsync(order);
			Console.WriteLine("Deleted Order [{0},{1}]\n", order.Partition, order.Id);
		}


	}
}
