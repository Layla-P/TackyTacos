using Steeltoe.Discovery.Client;

namespace TackyTacos.ApiGateway.Extensions
{
	public static class IServiceCollectionExtensions
	{

		public static IServiceCollection AddEurekaServiceDiscovery(this IServiceCollection services)
		{
			services.AddServiceDiscovery();
			services.AddProxyConfig();

			return services;
		}
	}
}
