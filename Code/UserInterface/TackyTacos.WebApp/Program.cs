using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using TackyTacos.WebApp.DummyData;

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
				client.BaseAddress = new Uri("https://localhost:5011/");
			});

			builder.Services.AddHttpClient("OrderService", client =>
			{
				client.BaseAddress = new Uri("https://localhost:5031/");
			});

			await builder.Build().RunAsync();


		}


	}
}