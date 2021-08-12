using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using TanzuTacos.OrderService.Models;
using TanzuTacos.OrderService.Data;
using TanzuTacos.OrderService.Messaging;
using TanzuTacos.OrderService.Services;
using TanzuTacos.Messaging;

namespace TanzuTacos.OrderService
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
			services.Configure<DbSettings>(Configuration.GetSection("DbSettings"));

			services.SetUpRabbitMQ(Configuration);
			services.AddSingleton<RabbitSender>();
			services.AddSingleton<CosmosDbContext>();
			services.AddSingleton<OrdersRepository>();
			services.AddSingleton<OrdersService>();

			services.AddCors(options =>
			{
				options.AddDefaultPolicy(
								  builder =>
								  {
									  builder.WithOrigins("http://localhost:23125");
								  });
			});

			services.AddControllers();
			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo { Title = "TanzuTacos.OrderService", Version = "v1" });
			});

		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				app.UseSwagger();
				app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "TanzuTacos.OrderService v1"));
			}

			app.UseHttpsRedirection();

			app.UseRouting();

			app.UseCors();

			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}
