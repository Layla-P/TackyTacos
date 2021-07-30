using System;

namespace TanzuTacos.OrderService.Data
{
	public interface IDocumentDbEntity
	{
		Type EntityType { get; set; }
		string Name { get; set; }
	}
}
