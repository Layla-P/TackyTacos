using Yarp.ReverseProxy.Configuration;

namespace Microsoft.Extensions.DependencyInjection
{
	public static class InMemoryConfigProviderExtensions
	{
		public static IReverseProxyBuilder LoadFromMemory(this IReverseProxyBuilder builder)
		{

			builder.Services
				.AddSingleton<InMemoryConfigProvider>();

			builder.Services
				.AddSingleton<IHostedService>(ctx => ctx.GetRequiredService<InMemoryConfigProvider>());

			builder.Services
				.AddSingleton<IProxyConfigProvider>(ctx => ctx.GetRequiredService<InMemoryConfigProvider>());

			return builder;
		}
	}
}
