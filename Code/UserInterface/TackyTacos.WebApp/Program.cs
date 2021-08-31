using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace TackyTacos.WebApp
{
	public class Program
	{
		public static async Task Main(string[] args)
		{
			var builder = WebAssemblyHostBuilder.CreateDefault(args);

			builder.RootComponents.Add<App>("#app");
			//extract the URIs to settings or config
			builder.Services.AddHttpClient("FoodService", client =>
			{
				client.BaseAddress = new Uri("https://localhost:5071/menuservice/");
			});

			builder.Services.AddHttpClient("OrderService", client =>
			{
				client.BaseAddress = new Uri("https://localhost:5071/orderservice/");
			});

			await builder.Build().RunAsync();


		}


	}
}