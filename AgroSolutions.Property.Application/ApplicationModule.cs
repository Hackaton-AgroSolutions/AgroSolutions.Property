using AgroSolutions.Property.Application.Behaviors;
using AgroSolutions.Property.Application.Commands.CreateProperty;
using AgroSolutions.Property.Application.Notifications;
using AgroSolutions.Property.Domain.Notifications;
using AgroSolutions.Property.Infrastructure.Subscribers;
using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace AgroSolutions.Property.Application;

public static class ApplicationModule
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddApplication()
        {
            services
                .AddSubscribers()
                .AddMediatR()
                .AddFluentValidation()
                .AddNotification();

            return services;
        }

        private IServiceCollection AddSubscribers()
        {
            services.AddHostedService<DeletedUserSubscriber>();

            return services;
        }

        private IServiceCollection AddMediatR()
        {
            services.AddMediatR(config => config.RegisterServicesFromAssemblyContaining<CreatePropertyCommand>());
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            return services;
        }

        private IServiceCollection AddFluentValidation()
        {
            services
                .AddFluentValidationAutoValidation(o => o.DisableDataAnnotationsValidation = true)
                .AddValidatorsFromAssemblyContaining<CreatePropertyCommandValidator>();

            return services;
        }

        private IServiceCollection AddNotification()
        {
            services.AddScoped<INotificationContext, NotificationContext>();

            return services;
        }
    }
}
