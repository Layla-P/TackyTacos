using Yarp.ReverseProxy.Configuration;

namespace Microsoft.Extensions.DependencyInjection
{
	public static class InMemoryConfigProviderExtensions
	{
		public static IReverseProxyBuilder LoadFromMemory(this IReverseProxyBuilder builder, IReadOnlyList<RouteConfig> routes, IReadOnlyList<ClusterConfig> clusters)
		{
			builder.Services.AddSingleton<IProxyConfigProvider>(new InMemoryConfigProvider(routes, clusters));
			return builder;
		}
	}
}