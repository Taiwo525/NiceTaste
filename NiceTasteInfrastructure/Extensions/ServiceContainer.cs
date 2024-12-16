//using Auth.Application.Interfaces;
//using Auth.Infrastructure.Persistence;
//using Auth.Infrastructure.Repositories;
//using Ecommerce.SharedLibrary.DependencyInjection;
//using Microsoft.AspNetCore.Builder;
//using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace NiceTasteInfrastructure.Extensions
{
    public static class ServiceContainer
    {
        public static IServiceCollection AddInfrastructureService(this IServiceCollection services, IConfiguration config)
        {
            //SharedServiceContainer.AddSharedService<AuthDbContext>(services, config, config["MySerilog:FileName"]);

            //services.AddScoped<IUser, UserRepo>();
            var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", true)
            .Build();

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();

            try
            {
                Log.Information("Starting web application");
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Application terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            }


            return services;
        }

        public static IApplicationBuilder UseInfrastructurePolicy(this IApplicationBuilder app)
        {
            //register middleware such as:
            // global exception to handle external error,
            // listen to api gateway only to block all outsiders call
            //SharedServiceContainer.UseSharedPolicies(app);

            return app;
        }
    }
}
