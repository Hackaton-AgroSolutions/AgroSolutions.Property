using AgroSolutions.Property.Domain.Repositories;
using AgroSolutions.Property.Infrastructure.Messaging;
using AgroSolutions.Property.Infrastructure.Persistence;
using AgroSolutions.Property.Infrastructure.Persistence.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace AgroSolutions.Property.Infrastructure;

public static class InfrastructureModule
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddInfrastructure(IConfiguration configuration)
        {
            services
                .AddMessageBroker(configuration)
                .AddAuthentication(configuration)
                .AddPersistence(configuration)
                .AddRepositories()
                .AddUnitOfWork();

            return services;
        }

        private IServiceCollection AddMessageBroker(IConfiguration configuration)
        {
            services.Configure<RabbitMqOptions>(configuration.GetSection("Messaging"));

            services.AddHttpContextAccessor();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddSingleton<IRabbitConnectionProvider, RabbitConnectionProvider>();
            services.AddScoped<IMessagingConnectionFactory, RabbitChannelFactory>();

            return services;
        }

        private IServiceCollection AddAuthentication(IConfiguration configuration)
        {
            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = configuration["Jwt:Issuer"],
                        ValidAudience = configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Secret"]!))
                    };
                });

            services.AddHttpClient("AuthService", client =>
            {
                client.BaseAddress = new Uri(configuration["AuthService:Url"]!);
                client.Timeout = TimeSpan.FromSeconds(int.Parse(configuration["AuthService:TimeoutInSeconds"]!));
            });

            return services;
        }

        private IServiceCollection AddPersistence(IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("DefaultConnection")!;
            services.AddDbContext<AgroSolutionsPropertyDbContext>(options => options.UseSqlServer(connectionString));

            return services;
        }

        private IServiceCollection AddRepositories()
        {
            services.AddScoped<IPropertyRepository, PropertyRepository>();
            services.AddScoped<ICropRepository, CropRepository>();

            return services;
        }

        private IServiceCollection AddUnitOfWork()
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}
