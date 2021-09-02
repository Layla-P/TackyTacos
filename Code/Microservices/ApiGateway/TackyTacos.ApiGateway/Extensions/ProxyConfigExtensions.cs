using Microsoft.Extensions.DependencyInjection;
using System.Runtime.CompilerServices;
using Yarp.ReverseProxy.Configuration;

namespace TackyTacos.ApiGateway.Extensions
{
	public static class ProxyConfigExtensions
	{
		public static IServiceCollection AddProxyConfig(this IServiceCollection services)
		{

			var transforms = new List<Dictionary<string, string>>
								{
									new Dictionary<string, string>
									{
										{ "PathPattern", "{**catchall}"}
									}
								};


			var routes = new[]
					   {
							new RouteConfig()
							{
								RouteId = "orders-route",
								ClusterId = "orders-cluster",
								Match = new RouteMatch
								{
									Path = "orderservice/{**catchall}"
								},
								Transforms = transforms
							},
							new RouteConfig()
							{
								RouteId = "menu-route",
								ClusterId = "menu-cluster",
								Match = new RouteMatch
								{
									Path = "menuservice/{**catchall}"
								},
								Transforms = transforms
							}
						};
			var clusters = new[]
			{
						new ClusterConfig()
						{
							ClusterId = "orders-cluster",
							Destinations = new Dictionary<string, DestinationConfig>(StringComparer.OrdinalIgnoreCase)
							{
								{ "destination1", new DestinationConfig() { Address = "https://localhost:5031" } }
							}
						},
						new ClusterConfig()
						{
							ClusterId = "menu-cluster",
							Destinations = new Dictionary<string, DestinationConfig>(StringComparer.OrdinalIgnoreCase)
							{
								{ "destination1", new DestinationConfig() { Address = "https://localhost:5011" } }
							}
						}
					};

			services.AddReverseProxy()
				.LoadFromMemory(routes, clusters);

			return services;
		}

	}
}
