//using Azure.Cosmos;
//using Microsoft.Extensions.Options;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net;
//using System.Threading.Tasks;
//using TanzuTacos.OrderService.Data;
//using TanzuTacos.OrderService.Models;

//namespace OrderService.IntegrationTests
//{
//	class Program
//	{
//		static async Task Main(string[] args)
//		{
//			//Task t = SecondInternalHandler();
//			//t.Wait();
//			//Console.ReadLine();
//			//}

//			//private static async Task SecondInternalHandler()
//			//{

//			var settings = new DbSettings
//			{
//				DatabaseId = "OrdersTestDB",
//				ContainerId = "OrdersTestContainer",
//				EndpointUri = "https://localhost:8081",
//				AuthKey = "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==" // this key is from a local emulator - not a live instance!!!!
//			};


//			CosmosClient cosmosClient = new CosmosClient(settings.EndpointUri, settings.AuthKey);
//			await Program.CreateDatabaseAsync(cosmosClient, settings);
//			await Program.CreateContainerAsync(cosmosClient, settings);
//			await Program.AddItemsToContainerAsync(cosmosClient, settings);
//			await Program.QueryItemsAsync(cosmosClient, settings);
//			await Program.ReplaceOrderItemAsync(cosmosClient, settings);
//			await Program.DeleteOrderItemAsync(cosmosClient, settings);
//			//await Program.DeleteDatabaseAndCleanupAsync(cosmosClient, settings);

//			//var options = Options.Create<DbSettings>(settings);
//			//var context = new CosmosDbContext(options);

//			//var repo = new OrdersRepository(context);

//			//var result = await repo.AddOrUpdateAsync(order1);

//			Console.ReadLine();
//		}
//		private static async Task CreateDatabaseAsync(CosmosClient cosmosClient, DbSettings settings)
//		{
//			// Create a new database
//			CosmosDatabase database = await cosmosClient.CreateDatabaseIfNotExistsAsync(settings.DatabaseId);
//			Console.WriteLine("Created Database: {0}\n", database.Id);
//		}

//		/// <summary>
//		/// Create the container if it does not exist. 
//		/// Specify "/LastName" as the partition key since we're storing family information, to ensure good distribution of requests and storage.
//		/// </summary>
//		/// <returns></returns>
//		private static async Task CreateContainerAsync(CosmosClient cosmosClient, DbSettings settings)
//		{
//			// Create a new container
//			CosmosContainer container = await cosmosClient.GetDatabase(settings.DatabaseId).CreateContainerIfNotExistsAsync(settings.ContainerId, "/WeekDay");
//			Console.WriteLine("Created Container: {0}\n", container.Id);
//		}

//		/// <summary>
//		/// Add Family items to the container
//		/// </summary>
//		private static async Task AddItemsToContainerAsync(CosmosClient cosmosClient, DbSettings settings)
//		{
//			var id1 = new Guid("FE7370FA-C7FC-44C9-9466-11D79A1F3B6D");
//			var id2 = new Guid("3FE461AF-C63E-41A5-B454-674000BA80BC");
//			var id3 = new Guid("657B0F94-2850-4DC3-A625-121C7EA16ED7");
//			var userId1 = new Guid("41E20F55-5819-460D-B357-4B12ECCED538");
//			var userId2 = new Guid("B98E2752-E652-42D0-B71B-D9109CCB702F");


//			var order1 = new Order
//			{
//				Id = id1,
//				UserId = userId1,
//				CreatedTime = DateTime.Now,
//				TotalPrice = 10M,
//				OrderPlaced = DateTime.Now,
//				OrderPaid = false,
//				Partition = DateTime.Now.DayOfWeek.ToString()
//			};

//			CosmosContainer container = cosmosClient.GetContainer(settings.DatabaseId, settings.ContainerId);
//			try
//			{
//				// Read the item to see if it exists.  
//				ItemResponse<Order> orderResponse1 = await container.ReadItemAsync<Order>(order1.Id.ToString(), new PartitionKey(order1.Partition));
//				Console.WriteLine("Item in database with id: {0} already exists\n", orderResponse1.Value.Id);
//			}
//			catch (CosmosException ex) when (ex.Status == (int)HttpStatusCode.NotFound)
//			{
//				// Create an item in the container representing the Andersen family. Note we provide the value of the partition key for this item, which is "Andersen"
//				ItemResponse<Order> andersenFamilyResponse = await container.CreateItemAsync<Order>(order1, new PartitionKey(order1.Partition));

//				// Note that after creating the item, we can access the body of the item with the Resource property off the ItemResponse.
//				Console.WriteLine("Created item in database with id: {0}\n", andersenFamilyResponse.Value.Id);
//			}

//			var order2 = new Order
//			{
//				Id = id2,
//				UserId = userId2,
//				CreatedTime = DateTime.Now,
//				TotalPrice = 20M,
//				OrderPlaced = DateTime.Now,
//				OrderPaid = false,
//				Partition = DateTime.Now.DayOfWeek.ToString()
//			};

//			// Create an item in the container representing the Wakefield family. Note we provide the value of the partition key for this item, which is "Wakefield"
//			try
//			{
//				// Read the item to see if it exists.  
//				ItemResponse<Order> orderResponse2 = await container.ReadItemAsync<Order>(order2.Id.ToString(), new PartitionKey(order2.Partition));
//				Console.WriteLine("Item in database with id: {0} already exists\n", orderResponse2.Value.Id);
//			}
//			catch (CosmosException ex) when (ex.Status == (int)HttpStatusCode.NotFound)
//			{
//				// Create an item in the container representing the Andersen family. Note we provide the value of the partition key for this item, which is "Andersen"
//				ItemResponse<Order> order2Response = await container.CreateItemAsync<Order>(order2, new PartitionKey(order2.Partition));

//				// Note that after creating the item, we can access the body of the item with the Resource property off the ItemResponse.
//				Console.WriteLine("Created item in database with id: {0}\n", order2Response.Value.Id);
//			}
//		}

//		/// <summary>
//		/// Run a query (using Azure Cosmos DB SQL syntax) against the container
//		/// </summary>
//		private static async Task QueryItemsAsync(CosmosClient cosmosClient, DbSettings settings)
//		{
//			var sqlQueryText = "SELECT * FROM c WHERE c.id = 'fe7370fa-c7fc-44c9-9466-11d79a1f3b6d'";

//			Console.WriteLine("Running query: {0}\n", sqlQueryText);

//			CosmosContainer container = cosmosClient.GetContainer(settings.DatabaseId, settings.ContainerId);

//			QueryDefinition queryDefinition = new QueryDefinition(sqlQueryText);

//			List<Order> orders = new();

//			await foreach (Order order in container.GetItemQueryIterator<Order>(queryDefinition))
//			{
//				orders.Add(order);
//				Console.WriteLine("\tRead {0}\n", order);
//			}
//		}

//		/// <summary>
//		/// Replace an item in the container
//		/// </summary>
//		private static async Task ReplaceOrderItemAsync(CosmosClient cosmosClient, DbSettings settings)
//		{
//			CosmosContainer container = cosmosClient.GetContainer(settings.DatabaseId, settings.ContainerId);

//			ItemResponse<Order> order2Response = await container.ReadItemAsync<Order>("fe7370fa-c7fc-44c9-9466-11d79a1f3b6d", new PartitionKey("Tuesday"));
//			Order itemBody = order2Response;

//			// update OrderPaid status from false to true
//			itemBody.OrderPaid = true;

//			// replace the item with the updated content
//			order2Response = await container.ReplaceItemAsync<Order>(itemBody, itemBody.Id.ToString(), new PartitionKey(itemBody.Partition));
//			Console.WriteLine("Updated Order [{0}].\n \tBody is now: {1}\n", itemBody.Id, order2Response.Value);
//		}

//		/// <summary>
//		/// Delete an item in the container
//		/// </summary>
//		private static async Task DeleteOrderItemAsync(CosmosClient cosmosClient, DbSettings settings)
//		{
//			CosmosContainer container = cosmosClient.GetContainer(settings.DatabaseId, settings.ContainerId);

//			string partitionKeyValue = "Tuesday";
//			string orderId = "fe7370fa-c7fc-44c9-9466-11d79a1f3b6d";

//			// Delete an item. Note we must provide the partition key value and id of the item to delete
//			ItemResponse<Order> order2Response = await container.DeleteItemAsync<Order>(orderId, new PartitionKey(partitionKeyValue));
//			Console.WriteLine("Deleted Order [{0},{1}]\n", partitionKeyValue, orderId);
//		}

//		//private static async Task InternalHandler()
//		//{
//		//	var settings = new DbSettings
//		//	{
//		//		DatabaseId = "OrdersTestDB",
//		//		EndpointUri = "https://localhost:8081",
//		//		AuthKey = "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw=="
//		//	};
//		//	var options = Options.Create<DbSettings>(settings);

//		//	var context = new DocumentDbContext(options);


//		//	await context.CreateDatabaseAndCollectionAsync();
//		//	var orderRepository = new OrdersRepository(context);

//		//	var id1 = new Guid("FE7370FA-C7FC-44C9-9466-11D79A1F3B6D");
//		//	var id2 = new Guid("3FE461AF-C63E-41A5-B454-674000BA80BC");
//		//	var id3 = new Guid("657B0F94-2850-4DC3-A625-121C7EA16ED7");
//		//	var userId1 = new Guid("41E20F55-5819-460D-B357-4B12ECCED538");
//		//	var userId2 = new Guid("B98E2752-E652-42D0-B71B-D9109CCB702F");


//		//	var order1 = new Order
//		//	{
//		//		Id = id1,
//		//		UserId = userId1,
//		//		CreatedTime = DateTime.Now,
//		//		TotalPrice = 10M,
//		//		OrderPlaced = DateTime.Now,
//		//		OrderPaid = false
//		//	};
//		//	//Create Orders
//		//	var orderOneEntity = await orderRepository.AddOrUpdateAsync(order1);
//		//	Console.WriteLine("=====Create test one=====");
//		//	Console.WriteLine($"Price: {orderOneEntity.TotalPrice} and Order paid: {orderOneEntity.OrderPaid} and value:{orderOneEntity.TotalPrice}");
//		//	Console.WriteLine("==========");
//		//	var order2 = new Order
//		//	{
//		//		Id = id2,
//		//		UserId = userId2,
//		//		CreatedTime = DateTime.Now,
//		//		TotalPrice = 20M,
//		//		OrderPlaced = DateTime.Now,
//		//		OrderPaid = false
//		//	};
//		//	var orderTwoEntity = await orderRepository.AddOrUpdateAsync(order2);
//		//	Console.WriteLine("=====Create test two=====");
//		//	Console.WriteLine($"Price: {orderTwoEntity.Id} and Order paid: {orderTwoEntity.OrderPaid} and value:{orderTwoEntity.TotalPrice}");
//		//	Console.WriteLine("==========");

//		//	var order3 = new Order
//		//	{
//		//		Id = id3,
//		//		UserId = userId2,
//		//		CreatedTime = DateTime.Now,
//		//		TotalPrice = 30M,
//		//		OrderPlaced = DateTime.Now,
//		//		OrderPaid = false
//		//	};
//		//	var orderThreeEntity = await orderRepository.AddOrUpdateAsync(order3);
//		//	Console.WriteLine("=====Create test three=====");
//		//	Console.WriteLine($"Price: {orderThreeEntity.Id} and Order paid: {orderThreeEntity.OrderPaid} and value:{orderThreeEntity.TotalPrice}");
//		//	Console.WriteLine("==========");

//		//	// Update Orders
//		//	order2.OrderPaid = true;			
//		//	orderTwoEntity = await orderRepository.AddOrUpdateAsync(order2);
//		//	Console.WriteLine("=====Update=====");
//		//	Console.WriteLine(order2.Id);
//		//	Console.WriteLine($"ID: {orderTwoEntity.Id} and Order paid: {orderTwoEntity.OrderPaid}");
//		//	Console.WriteLine("==========");

//		//	//Get order by Id
//		//	orderTwoEntity = await orderRepository.GetByIdAsync(id2);
//		//	Console.WriteLine("=====GetByIdAsync=====");
//		//	Console.WriteLine($"ID: {orderTwoEntity.Id} and Order paid: {orderTwoEntity.OrderPaid} and value:{orderTwoEntity.TotalPrice}");
//		//	Console.WriteLine("==========");

//		//	//Where
//		//	var orders = await orderRepository.WhereAsync(order => order.UserId == userId2);
//		//	Console.WriteLine("=====Where=====");
//		//	Console.WriteLine(orders.Count());
//		//	foreach (var order in orders)
//		//	{
//		//		Console.WriteLine($"ID: {order.Id} UserId:{order.UserId} and price: {order.TotalPrice}");
//		//	}
//		//}
//	}
//}
