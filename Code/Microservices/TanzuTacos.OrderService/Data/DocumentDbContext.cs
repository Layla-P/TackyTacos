using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using TanzuTacos.OrderService.Models;

namespace TanzuTacos.OrderService.Data
{
	public class DocumentDbContext : IDocumentDbContext
	{
		private IDocumentClient _documentClient;
		private IList<IDocumentDbEntity> _documentDbEntities;

		public string DatabaseId { get; set; }
		public string EndpointUri { get; set; }
		public string AuthKey { get; set; }

		public IDocumentClient DocumentClient
		{
			get
			{
				if(_documentClient is null)
				{
					_documentClient = GetDocumentClient();
				}
				return _documentClient;
			}
		}

		public ICollection<IDocumentDbEntity> EntityCollection
		{
			get
			{
				if(_documentDbEntities is null)
				{
					_documentDbEntities = GetDocumentEntities();
				}
				return _documentDbEntities;
			}
		}

		public async Task CreateDatabaseAndCollectionAsync()
		{
			await CreateDatabaseAsync(DatabaseId);
			foreach(var entity in EntityCollection)
			{
				await CreateDatabaseAndCollectionAsync(DatabaseId, entity.Name);
			}
		}



		private IDocumentClient GetDocumentClient()
		{
			ConnectionPolicy connectionPolicy = new()
			{
				ConnectionMode = ConnectionMode.Gateway,
				ConnectionProtocol = Protocol.Https,
				MaxConnectionLimit = 1000,
				RetryOptions = new()
				{
					MaxRetryAttemptsOnThrottledRequests = 3,
					MaxRetryWaitTimeInSeconds = 30
				},
				EnableEndpointDiscovery = true,
				EnableReadRequestsFallback = true
			};

			connectionPolicy.PreferredLocations.Add(LocationNames.UKSouth);

			return new DocumentClient(new Uri(EndpointUri), AuthKey, connectionPolicy);
		}


		private List<IDocumentDbEntity> GetDocumentEntities()
		{
			 return new List<IDocumentDbEntity>
			{
				new DocumentDbEntity { EntityType = typeof(Order), Name = "OrderCollection"}
			};

		}

		private async Task<Database> CreateDatabaseAsync(string databaseId)
		{
			var response = await DocumentClient.CreateDatabaseIfNotExistsAsync(
				new Database { Id = databaseId });
			return response.Resource;
		}

		private async Task<DocumentCollection> CreateDatabaseAndCollectionAsync(string databaseId, string collectionName)
		{
			var response = await DocumentClient.CreateDocumentCollectionIfNotExistsAsync(
				UriFactory.CreateDatabaseUri(databaseId),
				new DocumentCollection { Id = collectionName },
				new RequestOptions()
				);

			return response.Resource;
		}
	}
}
