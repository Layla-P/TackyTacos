using FluentAssertions;
using Microsoft.Extensions.Configuration.EnvironmentVariables;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using System;
using System.Threading.Tasks;
using TanzuTacos.OrderService.Data;
using TanzuTacos.OrderService.Models;

namespace OrderService.Tests
{
	public class OrdersRepositoryTests
	{
		private DbSettings _settings;
		private DocumentDbContext _context;
		private Guid _id1 = Guid.NewGuid();
		private Guid _id2 = Guid.NewGuid();
		private Guid _userId1 = Guid.NewGuid();
		private Guid _userId2 = Guid.NewGuid();
		private Order _order1;
		private Order _order2;
		private OrdersRepository _sut;
		

		[SetUp]
		public void Setup()
		{
			_settings = new DbSettings
			{
				DatabaseId = "OrdersTestDB",
				EndpointUri = "https://localhost:8081",
				AuthKey = "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==" // this key is from a local emulator - not a live instance!!!!
			};
			var options = Options.Create<DbSettings>(_settings);

			_context = new DocumentDbContext(options);

			_order1 = new Order
			{
				Id = _id1,
				UserId = _userId1,
				CreatedTime = DateTime.Now,
				TotalPrice = 10M,
				OrderPlaced = DateTime.Now,
				OrderPaid = false
			};

			_order2 = new Order
			{
				Id = _id2,
				UserId = _userId2,
				CreatedTime = DateTime.Now,
				TotalPrice = 20M,
				OrderPlaced = DateTime.Now,
				OrderPaid = false
			};

			_sut = new OrdersRepository(_context);
		}

		[Test]
		public async Task AddOrder_ShouldReturnOrder()
		{			
			var result = await _sut.AddOrUpdateAsync(_order1);

			result.Should().BeEquivalentTo(_order1);
		}
	}
}