
namespace TackyTacos.OrderService.Data
{
	public class CosmosDbContext
	{
		private readonly DbSettings _settings;
		private CosmosClient _cosmosClient;
		private CosmosDatabase _database;
		private CosmosContainer _container;
		
		public CosmosDbContext(IOptions<DbSettings> settings)
		{
			_settings = settings.Value;
		
		}

		public async Task<CosmosContainer> GetContainerAsync()
		{
			if (_container is null)
			{
				await CreateContainerAsync();
			}

			return _container;
		}

		private CosmosClient CreateCosmosClient()
		{
			return new CosmosClient(
			_settings.EndpointUri,
			_settings.AuthKey,
			new CosmosClientOptions()
			{
				ApplicationRegion = Regions.UKSouth,
			});
		}

		private async Task CreateDatabaseAsync()
		{
			if (_cosmosClient is null)
			{
				_cosmosClient = CreateCosmosClient();
			}

			_database = await _cosmosClient.CreateDatabaseIfNotExistsAsync(_settings.DatabaseId);
			Console.WriteLine("Created Database: {0}\n", _database.Id);
			
		}

		private async Task CreateContainerAsync()
		{
			if (_database is null)
			{
				await CreateDatabaseAsync();
			}

			_container = await _cosmosClient.GetDatabase(_settings.DatabaseId)
				.CreateContainerIfNotExistsAsync(_settings.ContainerId, _settings.PartitionKey);

			Console.WriteLine("Created Container: {0}\n", _container.Id);
		}
		
	}
}

