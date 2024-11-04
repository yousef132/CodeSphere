using CodeSphere.Application.Behaviors;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace CodeSphere.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            var assembly = typeof(DependencyInjection).Assembly;

            // mediator
            services.AddMediatR(configuration =>
                configuration.RegisterServicesFromAssembly(assembly));

            // fluent validations
            services.AddValidatorsFromAssembly(assembly);
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            // auto mapper
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            return services;
        }

    }
}
