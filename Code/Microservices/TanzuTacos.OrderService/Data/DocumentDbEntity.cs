using System;

namespace TanzuTacos.OrderService.Data
{
	public class DocumentDbEntity : IDocumentDbEntity
	{
		public Type EntityType { get; set; }
		public string Name { get; set; }
	}
}
