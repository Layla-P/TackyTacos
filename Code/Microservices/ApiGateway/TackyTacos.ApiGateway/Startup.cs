using Yarp.ReverseProxy.Forwarder;
using Steeltoe.Discovery.Client;

namespace TackyTacos.ApiGateway
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddDiscoveryClient();
			services.AddSingleton<IForwarderHttpClientFactory, ServiceDiscoveryForwarderHttpClientFactory>();
			var proxyBuilder = services.AddReverseProxy();
			// Initialize the reverse proxy from the "ReverseProxy" section of configuration
			proxyBuilder.LoadFromConfig(Configuration.GetSection("ReverseProxy"));

		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			// Enable endpoint routing, required for the reverse proxy
			app.UseRouting();
			// Register the reverse proxy routes
			app.UseEndpoints(endpoints =>
			{
				endpoints.MapReverseProxy();
			});
		}
	}
}
