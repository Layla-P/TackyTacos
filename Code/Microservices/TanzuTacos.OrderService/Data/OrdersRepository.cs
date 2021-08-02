using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Threading.Tasks;
using TanzuTacos.OrderService.Models;

namespace TanzuTacos.OrderService.Data
{
	public class OrdersRepository : IDocumentDbRepository<Order>
	{
		private readonly IDocumentDbContext _context;
		private DocumentCollection _documentCollection;
		public OrdersRepository(IDocumentDbContext context)
		{
			_context = context;
			var entityMetadata = _context.EntityCollection.FirstOrDefault(p => p.EntityType == typeof(Order));
			Task.Run(async () => _documentCollection
			= await _context.DocumentClient.ReadDocumentCollectionAsync(
				UriFactory.CreateDocumentCollectionUri(_context._databaseId, entityMetadata.Name))).Wait();
		}

		public async Task<Order> AddOrUpdateAsync(Order entity)
		{
			var upsertedDoc = await _context
				.DocumentClient.UpsertDocumentAsync(
				_documentCollection.SelfLink, entity);

			return JsonConvert.DeserializeObject<Order>(upsertedDoc.Resource.ToString());
		}

		public async Task<Order> GetByIdAsync(Guid id)
		{
			var result = await Task.Run(
				() => _context.DocumentClient.CreateDocumentQuery<Order>(
					UriFactory.CreateDocumentCollectionUri(_context._databaseId, _documentCollection.Id))
				.Where(p => p.Id == id).ToList());

			return result != null && result.Any() ? result.FirstOrDefault() : null;
		}

		public async Task<bool> RemoveAsync(Guid id)
		{
			var result = await _context.DocumentClient.DeleteDocumentAsync(
				UriFactory.CreateDocumentUri(
					_context._databaseId, _documentCollection.Id, id.ToString()));

			return result.StatusCode == HttpStatusCode.NoContent;
		}

		public async Task<IQueryable<Order>> WhereAsync(Expression<Func<Order, bool>> predicate)
		{
			return await Task.Run(
				() => _context.DocumentClient
				.CreateDocumentQuery<Order>(
					UriFactory.CreateDocumentCollectionUri(_context._databaseId, _documentCollection.Id))
				.Where(predicate));
		}
	}
}
