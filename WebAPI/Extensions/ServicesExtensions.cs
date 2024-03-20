using AspNetCoreRateLimit;
using Entities.DataTransferObject;
using Entities.Models;
using Marvin.Cache.Headers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Bson;
using Presentation.Action_Filters;
using Presentation.ActionFilters;
using Presentation.Controllers;
using Repositories.Contracts;
using Repositories.EFCore;
using Repositrories.Contracts;
using Repositrories.EF_Core;
using Services;
using Services.Contracts;
using System.Runtime.CompilerServices;
using System.Text;

namespace WebAPI.Extensions
{
	public static class ServicesExtensions
	{
		public static void ConfigureSqlContext(this IServiceCollection services,
			IConfiguration configuration) =>
				services.AddDbContext<RepositoryContext>(options =>
					options.UseSqlServer(configuration.GetConnectionString("sqlConnection")));

		public static void ConfigureRepositoryManager(this IServiceCollection services) =>
			services.AddScoped<IRepositoryManager, RepositoryManager>();

		public static void ConfigureServiceManager(this IServiceCollection services) =>
			services.AddScoped<IServiceManager, ServiceManager>();

		public static void ConfigureLoggerService(this IServiceCollection services) =>
			services.AddSingleton<ILoggerService, LoggerManager>();
		

		public static void ConfigureActionFilters(this IServiceCollection services)
		{
			services.AddScoped<ValidationFilterAttribute>();
			services.AddSingleton<LogFilterAttribute>();
			services.AddScoped<ValidateMediaTypeAttribute>();
		}

	    public static void ConfigureCors(this IServiceCollection services)
		{
			services.AddCors(options =>
			{
				options.AddPolicy("CorsPolicy", builder =>
				builder.AllowAnyOrigin()
				.AllowAnyMethod()
				.AllowAnyHeader()
				.WithExposedHeaders("X-Pagination")
				);
			});
		}


		public static void ConfigureDataShaper(this IServiceCollection services)
		{
			services.AddScoped<IDataShaper<BookDto>, DataShaper<BookDto>>();
		}

		public static void AddCustomMediaTypes(this IServiceCollection services)
		{
			services.Configure<MvcOptions>(config =>
			{
				var systemTextJsonOutputFormatter = config
				  .OutputFormatters
				  .OfType<SystemTextJsonInputFormatter>()?.FirstOrDefault();

				if(systemTextJsonOutputFormatter is not null)
				{
					systemTextJsonOutputFormatter.SupportedMediaTypes
					.Add("application/vdn.webapikursu.hateoas+json");

					systemTextJsonOutputFormatter.SupportedMediaTypes
					.Add("application/vdn.webapikursu.apiroot+json");
				}

				var xmlOutputFormatter = config
				.OutputFormatters
				.OfType<XmlDataContractSerializerInputFormatter>()?.FirstOrDefault();

				if (xmlOutputFormatter is not null)
				{
					xmlOutputFormatter.SupportedMediaTypes
				   .Add("application/vdn.webapikursu.hateoas+xml");
					
					 xmlOutputFormatter.SupportedMediaTypes
					.Add("application/vdn.webapikursu.apiroot+xml");;
				}																			  

			});
		}

		public static void ConfigureVersioning(this IServiceCollection services)
		{
			services.AddApiVersioning(opt =>
			{
				opt.ReportApiVersions = true;
				opt.AssumeDefaultVersionWhenUnspecified=true;
				opt.DefaultApiVersion = new ApiVersion(1, 0);
				opt.ApiVersionReader = new HeaderApiVersionReader("api-version");
				opt.Conventions.Controller<BooksController>()
				.HasApiVersion(new ApiVersion(1,0));

				opt.Conventions.Controller<BooksController>()
				.HasApiVersion(new ApiVersion(1,0));

				opt.Conventions.Controller<BooksController>()
				.HasDeprecatedApiVersion(new ApiVersion(2, 0));
			});
		}

		public static void ConfigureResponseCaching(this IServiceCollection services) =>
			services.AddResponseCaching();

		public static void ConfiugreHttpCacheHeaders(this IServiceCollection services) =>
			services.AddHttpCacheHeaders(expirationOpt =>
			{
				expirationOpt.MaxAge = 60;
				expirationOpt.CacheLocation = CacheLocation.Public;
			},
			validationOpt =>
			{
				validationOpt.MustRevalidate = false;
			});

		public static void ConfigureRateLimitingOptions(this IServiceCollection services)
		{
			var rateLimitRules = new List<RateLimitRule>() {
				new RateLimitRule()
				{
					Endpoint = "*",
					Limit = 10,
					Period = "1m"
				}
		  
		   };

			services.Configure<IpRateLimitOptions>(opt =>
			{
				opt.GeneralRules = rateLimitRules;
				
			});

			services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
			services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
			services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
			services.AddSingleton<IProcessingStrategy,AsyncKeyLockProcessingStrategy>();
		}

		public static void ConfigureIdentity(this IServiceCollection services)
		{
			var builder = services.AddIdentity<User, IdentityRole>(opts =>
			{
				opts.Password.RequireDigit  = true;
				opts.Password.RequireLowercase = true;
				opts.Password.RequireUppercase = false;
				opts.Password.RequireNonAlphanumeric = false;
				opts.Password.RequiredLength = 6;

				opts.User.RequireUniqueEmail = true;
			})
			   .AddEntityFrameworkStores<RepositoryContext>()
			   .AddDefaultTokenProviders();
		}

		public static void ConfigureJWT(this IServiceCollection services,
			IConfiguration configuration)
		{
			var jwtSettings = configuration.GetSection("JwtSettings");
			var secretKey = jwtSettings["secretKey"];

			services.AddAuthentication(options =>
			{
				options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

			}).AddJwtBearer(options =>
			options.TokenValidationParameters = new TokenValidationParameters
			{
				ValidateIssuer = true,
				ValidateAudience = true,
				ValidateLifetime = true,
				ValidateIssuerSigningKey = true,
				ValidIssuer = jwtSettings["validIssuer"],
				ValidAudience = jwtSettings["validAudience"],
				IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
			});

		}

		public static void ConfigureSwagger(this IServiceCollection services)
		{
			services.AddSwaggerGen(s =>
			{
				s.SwaggerDoc("v1",
					new OpenApiInfo 
					{ Title = "BookAPI",
						Version = "v1",
						Description= "BookStoreApp WebAPI",
						TermsOfService= new Uri("https://www.btkakademi.gov.tr/portal/course/asp-net-core-web-api-23993"),
						Contact = new OpenApiContact
						{
							Name ="Soner Yeşilay",
							Email ="soneryesilay@outlook.com"
						},
					});
				s.SwaggerDoc("v2",
					new OpenApiInfo
					{
						Title = "BookAPI",
						Version = "v2",
						Description = "BookStoreApp WebAPI",
						TermsOfService = new Uri("https://www.btkakademi.gov.tr/portal/course/asp-net-core-web-api-23993"),
						Contact = new OpenApiContact
						{
							Name = "Soner Yeşilay",
							Email = "soneryesilay@outlook.com"
						},
					});

				s.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
				{
				  In = ParameterLocation.Header,
				  Description="Place to add JWT with Bearer",
				  Name = "Authorization",
				  Type = SecuritySchemeType.ApiKey,
				  Scheme ="Bearer"
				});

				s.AddSecurityRequirement(new OpenApiSecurityRequirement()
				{
					{
						new OpenApiSecurityScheme
						{
							Reference = new OpenApiReference
							{
								Type = ReferenceType.SecurityScheme,
								Id="Bearer"
							},
							Name = "Bearer"
						},
						new List<string>()
					}
				});
			});
		}

		public static void RegisterRepositories(this IServiceCollection services)
		{
			services.AddScoped<IBookRepository, BookRepository>();
			services.AddScoped<ICategoryRepository, CategoryRepository>();
		}
		public static void RegisterServices(this IServiceCollection services)
		{
			services.AddScoped<IBookService, BookManager>();
			services.AddScoped<ICategoryService, CategoryManager>();
			services.AddScoped<IAuthenticationService, AuthenticationManager>();
		}
	}
}
