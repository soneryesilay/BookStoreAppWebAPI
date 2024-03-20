using AspNetCoreRateLimit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NLog;
using Presentation.Action_Filters;
using Repositrories.Contracts;
using Repositrories.EF_Core;
using Services;
using Services.Contracts;
using WebAPI.Extensions;



namespace WebAPI
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			LogManager.LoadConfiguration(String.Concat(Directory.GetCurrentDirectory(), "/Nlog.config"));
			// Add services to the container.

			builder.Services.AddControllers();
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();

			builder.Services.AddControllers(config =>
			{
				config.RespectBrowserAcceptHeader = true; //default false gelir ama biz içerik pazarlýðýný þimdi aktif ettik
				config.ReturnHttpNotAcceptable = true;  //bir itek geldiðinde kabul etmediðimizi bildiriyoruz
				config.CacheProfiles.Add("5mins", new CacheProfile() { Duration = 300 });
			})
				.AddXmlDataContractSerializerFormatters()   //artýk xml formatýnda da response verebileceðiz (isteyene)
				.AddCustomCvsFormatter()
				.AddApplicationPart(typeof(Presentation.AssemblyReference).Assembly)
				.AddNewtonsoftJson(opt =>
				opt.SerializerSettings.ReferenceLoopHandling = 
				Newtonsoft.Json.ReferenceLoopHandling.Ignore
				);

			

			builder.Services.Configure<ApiBehaviorOptions>(Options =>
			{
				Options.SuppressModelStateInvalidFilter= true;
			});

			builder.Services.ConfigureSwagger();
			builder.Services.ConfigureSqlContext(builder.Configuration);
			builder.Services.ConfigureRepositoryManager();
			builder.Services.ConfigureServiceManager();
			builder.Services.ConfigureLoggerService();
			builder.Services.AddAutoMapper(typeof(Program));
			builder.Services.ConfigureActionFilters();
			builder.Services.ConfigureCors();
			builder.Services.ConfigureDataShaper();
			builder.Services.AddCustomMediaTypes();
			builder.Services.AddScoped<IBookLinks, BookLinks>();
			builder.Services.ConfigureVersioning();
			builder.Services.ConfigureResponseCaching();
			builder.Services.ConfiugreHttpCacheHeaders();
			builder.Services.AddMemoryCache();
			builder.Services.ConfigureRateLimitingOptions();
			builder.Services.AddHttpContextAccessor();
			builder.Services.ConfigureIdentity();
			builder.Services.ConfigureJWT(builder.Configuration);
			builder.Services.RegisterRepositories();
			builder.Services.RegisterServices();

			var app = builder.Build();

			var logger = app.Services.GetRequiredService<ILoggerService>();
			app.ConfigureExceptionHandler(logger);


			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI(s =>
				{
					s.SwaggerEndpoint("/swagger/v1/swagger.json", "BookAPI v1");
					s.SwaggerEndpoint("/swagger/v2/swagger.json", "BookAPI v2");
				});
			}

			if(app.Environment.IsProduction())
			{
			app.UseHsts();
			}

			app.UseHttpsRedirection();

			app.UseIpRateLimiting();
			app.UseCors("CorsPolicy");
			app.UseResponseCaching(); //MÝCROSOFT CORS DAN SONRA CACHÝNG ÝFADESÝNÝN ÇAÐIRILMASINI ÖNERÝYOR!
			app.UseHttpCacheHeaders();

			app.UseAuthentication();
			app.UseAuthorization();

			app.MapControllers();

			app.Run();
		}
	}
}
