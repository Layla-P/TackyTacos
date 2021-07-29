using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using TanzuTacos.WebApp.DummyData;

namespace TanzuTacos.WebApp
{
	public class Program
	{
		public static async Task Main(string[] args)
		{
			var builder = WebAssemblyHostBuilder.CreateDefault(args);
			builder.RootComponents.Add<App>("#app");

			
			builder.Services.AddHttpClient("FoodService", client =>
			{
				client.BaseAddress = new Uri("https://localhost:44332/");
			});

			await builder.Build().RunAsync();


		}


	}
}