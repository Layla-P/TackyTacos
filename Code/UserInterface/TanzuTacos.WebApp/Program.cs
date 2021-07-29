using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace TanzuTacos.WebApp
{
	public class Program
	{
		public static async Task Main(string[] args)
		{
			var builder = WebAssemblyHostBuilder.CreateDefault(args);
			builder.RootComponents.Add<App>("#app");


			builder.Services.AddHttpClient("WebApiProject", client =>
			{
				client.BaseAddress = new Uri("https://localhost:44347/");
			});

			await builder.Build().RunAsync();


		}


	}
}