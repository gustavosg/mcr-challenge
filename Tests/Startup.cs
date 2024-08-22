using System.Reflection;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Infrastructure.UnitOfWork;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Builder;
using Infrastructure;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using Core;
using Application.DTO.Configuration;

namespace Tests
{
    public class Startup
    {
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; set; }

		public void Configure(
		 IApplicationBuilder app,
		 IConfiguration configuration
			)
		{
			this.Configuration = configuration;
			
		}
		

        public void ConfigureServices(IServiceCollection services) {

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            var connection = Configuration["AppSettings:Database:ConnectionString"];

            services.AddDbContext<AppDbContext>(
                opts =>
                    opts.UseNpgsql(connection,
                    optsBuilder =>
                    {
                        optsBuilder.MigrationsAssembly(Assembly.Load(Constants.INFRASTRUCTURE_PROJECT_NAME).FullName);
                        optsBuilder.SetPostgresVersion(13, 0);
                    }
                )
            );
            services.AddScoped<AppDbContext>();

            services.AddCors();
            services.AddControllers(options =>
            {
                //options.Conventions.Add(new RouteTokenTransformerConvention(new SlugifyParameterTransformer()));
            });

            services.AddMvc().AddApplicationPart(Assembly.Load("Api"));

            // JWT Security
            JwtOptionsSettings jwtSettings = new JwtOptionsSettings();
            new ConfigureFromConfigurationOptions<JwtOptionsSettings>(
                Configuration.GetSection("AppSettings:JwtOptions"))
                .Configure(jwtSettings);

            // Adiciona configuração de JwtSettings
            services.Configure<JwtOptionsSettings>(Configuration.GetSection("AppSettings:JwtOptions"));
        }
    }
}