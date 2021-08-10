using Azure;
using Azure.Cosmos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using TanzuTacos.OrderService.Models;

namespace TanzuTacos.OrderService.Data
{
	public class OrdersRepository
	{
		private readonly CosmosDbContext _context;
		public OrdersRepository(CosmosDbContext context)
		{
			_context = context;
		}

		public async Task<Order> AddOrUpdateAsync(Order order)
		{
			if (string.IsNullOrEmpty(order.Partition))
			{
				order.Partition = order.CreatedTime.DayOfWeek.ToString();
			}

			CosmosContainer container = await _context.GetContainerAsync();
			ItemResponse<Order> orderResponse;
			try
			{
				// Read the item to see if it exists.  
				orderResponse = await container.ReadItemAsync<Order>(order.Id.ToString(), new PartitionKey(order.Partition));
				Console.WriteLine($"Item in database with id: {orderResponse.Value.Id} already exists\n");
			}
			catch (CosmosException ex) when (ex.Status == (int)HttpStatusCode.NotFound)
			{
				// Create an item in the container representing the Andersen family. Note we provide the value of the partition key for this item, which is "Andersen"
				orderResponse = await container.CreateItemAsync<Order>(order, new PartitionKey(order.Partition));

				// Note that after creating the item, we can access the body of the item with the Resource property off the ItemResponse.
				Console.WriteLine($"Created item in database with id: {orderResponse.Value.Id}\n");
			}

			return orderResponse.Value;
		}

		/// <summary>
		/// Replace an item in the container as an update
		/// </summary>
		public async Task<Order> ReplaceOrderItemAsync(Order order)
		{
			if (string.IsNullOrEmpty(order.Partition))
			{
				order.Partition = order.CreatedTime.DayOfWeek.ToString();
			}

			CosmosContainer container = await _context.GetContainerAsync();

			ItemResponse<Order> orderResponse = await container.ReadItemAsync<Order>(order.Id.ToString(), new PartitionKey(order.Partition));
			Order itemBody = orderResponse;

			// replace the item with the updated content
			orderResponse = await container.ReplaceItemAsync<Order>(itemBody, itemBody.Id.ToString(), new PartitionKey(itemBody.Partition));
			Console.WriteLine($"Updated Order [{itemBody.Id}].\n \tBody is now: {orderResponse.Value}\n");

			return orderResponse.Value;
		}

		public async Task<Order> GetByIdAsync(Order order)
		{
			CosmosContainer container = await _context.GetContainerAsync();

			ItemResponse<Order> orderResponse = await container.ReadItemAsync<Order>(order.Id.ToString(), new PartitionKey(order.Partition));

			return orderResponse != null ? orderResponse.Value : null;
		}


		public async Task<List<Order>> GetAllByIdAsync(string id, bool excludePaid = false)
		{
			id = id.ToLower();
			string sqlQueryText = $"SELECT * FROM c WHERE c.id = '{id}'";
			if (excludePaid)
			{
				sqlQueryText = $"SELECT * FROM c WHERE c.id = '{id}' AND c.OrderPaid = false";
			}

			return await GetAllByIdAsync(sqlQueryText);

		}

		public async Task<List<Order>> GetAllNotPaidAsync()
		{
			var sqlQueryText = $"SELECT * FROM c WHERE c.OrderPaid = false";

			return await GetAllByIdAsync(sqlQueryText);
		}


		public async Task DeleteOrderItemAsync(Order order)
		{
			CosmosContainer container = await _context.GetContainerAsync();
			
			// Delete an item. Note we must provide the partition key value and id of the item to delete
			ItemResponse<Order> order2Response = await container.DeleteItemAsync<Order>(order.Id.ToString(), new PartitionKey(order.Partition));
			Console.WriteLine($"Deleted Order {order.Id}\n");
		}


		private async Task<List<Order>> GetByQueryAsync(string queryText)
		{
			CosmosContainer container = await _context.GetContainerAsync();

			QueryDefinition queryDefinition = new QueryDefinition(queryText);

			List<Order> orders = new();
			await foreach (Order order in container.GetItemQueryIterator<Order>(queryDefinition))
			{
				orders.Add(order);
				Console.WriteLine($"\tRead {order} is unpaid\n");
			}

			return orders != null && orders.Any() ? orders : null;
		}

		// LINQ queries currently not available on Azure.Cosmos
		//public async Task<IQueryable<Order>> WhereAsync(Expression<Func<Order, bool>> predicate)
		//{
		//	//https://docs.microsoft.com/en-us/dotnet/api/microsoft.azure.cosmos.container.getitemlinqqueryable?view=azure-dotnet
		//	CosmosContainer container = await _context.GetContainerAsync();

		//	container
		//	//using (FeedIterator<Order> setIterator = container.GetItemLinqQueryable<Order>()
		//	//		  .Where(b => b.Title == "War and Peace")
		//	//		  .ToFeedIterator<Book>())
		//	//{
		//	//	//Asynchronous query execution
		//	//	while (setIterator.HasMoreResults)
		//	//	{
		//	//		foreach (var item in await setIterator.ReadNextAsync())
		//	//		{
		//	//			{
		//	//				Console.WriteLine(item.cost);
		//	//			}
		//	//		}
		//	//	}
		//	//}
		//	QueryDefinition queryDefinition = new QueryDefinition(predicate);

		//	return await Task.Run(
		//		() => _context.DocumentClient
		//		.CreateDocumentQuery<Order>(
		//			UriFactory.CreateDocumentCollectionUri(_context._databaseId, _documentCollection.Id))
		//		.Where(predicate));
		//}



	}
}
