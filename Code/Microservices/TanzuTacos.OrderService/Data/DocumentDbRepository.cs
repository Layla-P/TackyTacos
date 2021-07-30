using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TanzuTacos.OrderService.Models;

namespace TanzuTacos.OrderService.Data
{
	public class DocumentDbRepository : IDocumentDbRepository<Order>
	{
		private readonly IDocumentDbContext _context;
		private DocumentCollection _documentCollection;
		public DocumentDbRepository(IDocumentDbContext context)
		{
			_context = context;
			var entityMetadata = _context.EntityCollection.FirstOrDefault(p => p.EntityType == typeof(Order));
			Task.Run(async () => _documentCollection
			= await _context.DocumentClient.ReadDocumentCollectionAsync(
				UriFactory.CreateDocumentCollectionUri(_context.DatabaseId, entityMetadata.Name))).Wait();
		}

		public async Task<Order> AddOrUpdateAsync(Order entity)
		{
			var upsertedDoc = await _context
				.DocumentClient.UpsertDocumentAsync(
				_documentCollection.SelfLink, entity);

			return JsonConvert.DeserializeObject <Order>(upsertedDoc.Resource.ToString());
		}

		public Task<Order> GetByIdAsync(string id)
		{
			throw new NotImplementedException();
		}

		public Task<bool> RemoveAsync(string id)
		{
			throw new NotImplementedException();
		}

		public Task<IQueryable<Order>> WhereAsync(Expression<Func<Order, bool>> predicate)
		{
			throw new NotImplementedException();
		}
	}
}
