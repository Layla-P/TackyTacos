using Microsoft.Azure.Documents;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TanzuTacos.OrderService.Data
{
	public interface IDocumentDbContext
	{
		string _databaseId { get; }
		string _endpointUri { get; }
		string _authKey { get; }
		IDocumentClient DocumentClient { get; }
		ICollection<IDocumentDbEntity> EntityCollection { get; }
		Task CreateDatabaseAndCollectionAsync();
	}
}
