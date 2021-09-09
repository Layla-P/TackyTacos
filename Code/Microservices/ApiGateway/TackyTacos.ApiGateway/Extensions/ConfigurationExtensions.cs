using Microsoft.Extensions.Primitives;
using Steeltoe.Discovery;
using Steeltoe.Discovery.Eureka;
//https://docs.microsoft.com/en-us/aspnet/core/fundamentals/host/hosted-services?view=aspnetcore-5.0&tabs=visual-studio
//https://stackoverflow.com/questions/64517214/timer-task-run-vs-while-loop-task-delay-in-asp-net-core-hosted-service
namespace Yarp.ReverseProxy.Configuration
{
	public class InMemoryConfigProvider : IProxyConfigProvider, IHostedService, IDisposable
	{
		private Timer _timer;
		private volatile InMemoryConfig _config;
		private readonly DiscoveryClient _discoveryClient;
		private readonly RouteConfig[] _routes;

		public InMemoryConfigProvider(IDiscoveryClient discoveryClient)
		{
			//_config = new InMemoryConfig(routes, clusters);
			_discoveryClient = discoveryClient as DiscoveryClient;

			_routes = new[]
					   {
							new RouteConfig()
							{
								RouteId = "orders-route",
								ClusterId = "TACKYTACOS.ORDERSERVICE",
								Match = new RouteMatch
								{
									Path = "orderservice/{**catchall}"
								},
								Transforms =  new List<Dictionary<string, string>>
								{
									new Dictionary<string, string>
									{
										{ "PathPattern", "{**catchall}"}
									}
								}
		},
							new RouteConfig()
							{
								RouteId = "menu-route",
								ClusterId = "TACKYTACOS.FOODSERVICE",
								Match = new RouteMatch
								{
									Path = "menuservice/{**catchall}"
								},
								Transforms =  new List<Dictionary<string, string>>
								{
									new Dictionary<string, string>
									{
										{ "PathPattern", "{**catchall}"}
									}
								}
		}
						};
		}

		public IProxyConfig GetConfig() => _config;

		public Task StartAsync(CancellationToken cancellationToken)
		{
			_timer = new Timer(DoWork, null, TimeSpan.Zero,
			TimeSpan.FromSeconds(30));
			return Task.CompletedTask;
		}

		public Task StopAsync(CancellationToken cancellationToken)
		{
			_timer?.Change(Timeout.Infinite, 0);
			return Task.CompletedTask;
		}
		private void DoWork(object state)
		{
			Update();
		}

		public void Update()
		{


			var apps = _discoveryClient.Applications.GetRegisteredApplications();

			List<ClusterConfig> clusters = new();

			foreach (var app in apps)
			{
				var cluster = new ClusterConfig
				{
					ClusterId = app.Name,
					Destinations = app.Instances
					.Select(x =>
						(x.InstanceId,
							new DestinationConfig()
							{
								Address = $"https://{x.HostName}:{x.SecurePort}"
							}))
					.ToDictionary(y => y.InstanceId, y => y.Item2)
				};

				clusters.Add(cluster);
			}

			var oldConfig = _config;
			_config = new InMemoryConfig(_routes, clusters);
			oldConfig?.SignalChange();
		}

		public void Dispose()
		{
			_timer?.Dispose();
		}

		private class InMemoryConfig : IProxyConfig
		{
			private readonly CancellationTokenSource _cts = new CancellationTokenSource();

			public InMemoryConfig(IReadOnlyList<RouteConfig> routes, IReadOnlyList<ClusterConfig> clusters)
			{
				Routes = routes;
				Clusters = clusters;
				ChangeToken = new CancellationChangeToken(_cts.Token);
			}

			public IReadOnlyList<RouteConfig> Routes { get; }

			public IReadOnlyList<ClusterConfig> Clusters { get; }

			public IChangeToken ChangeToken { get; }

			internal void SignalChange()
			{
				_cts.Cancel();
			}
		}
	}
}
