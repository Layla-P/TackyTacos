using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TanzuTacos.OrderService.Models;

namespace TanzuTacos.OrderService.Data
{
	public class DocumentDbContext : IDocumentDbContext
	{
		private IDocumentClient _documentClient;
		private IList<IDocumentDbEntity> _documentDbEntities;

		public readonly DbSettings _settings;


		public DocumentDbContext(IOptions<DbSettings> settings)
		{
			_settings = settings.Value;

			_databaseId = _settings.DatabaseId;
			_endpointUri = _settings.EndpointUri;
			_authKey = _settings.AuthKey;
		}

		public string _databaseId { get; }
		public string _endpointUri { get; }
		public string _authKey { get; }

		public IDocumentClient DocumentClient
		{
			get
			{
				if (_documentClient is null)
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
				if (_documentDbEntities is null)
				{
					_documentDbEntities = GetDocumentEntities();
				}
				return _documentDbEntities;
			}
		}



		public async Task CreateDatabaseAndCollectionAsync()
		{
			await CreateDatabaseAsync(_settings.DatabaseId);
			foreach (var entity in EntityCollection)
			{
				await CreateDatabaseAndCollectionAsync(_settings.DatabaseId, entity.Name);
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

			return new DocumentClient(new Uri(_settings.EndpointUri), _settings.AuthKey, connectionPolicy);
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
			try
			{
				var db = new Database { Id = databaseId };
				var response = await DocumentClient.CreateDatabaseIfNotExistsAsync(db);
				return response.Resource;
			}
			catch(Exception ex)
			{
				Console.WriteLine(ex);
				throw;
			}
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
